using System;
using System.Collections.Generic;
using BlocksGame.UI;
using MessagePipe;
using R3;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private GameplayConfig config;
        [SerializeField] private GridView gridView;
        [SerializeField] private AudioClip[] rowClearSFX;
        [SerializeField] private AudioClip gameEndSFX;
        [SerializeField] private AudioClip piecePlacedSFX;
        
        public ReadOnlyReactiveProperty<int> SecondsElapsed => secondsElapsed;
        public ReadOnlyReactiveProperty<int> Score => score;
        public IPiece ActivePiece => activePiece;
        [HideInInspector] public bool DropHeld = false;
        
        private ReactiveProperty<int> secondsElapsed = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> score = new ReactiveProperty<int>(0);

        private InputHandler inputHandler;
        private Grid grid;
        private DifficultyHandler difficultyHandler;

        private float timeSinceLastTick = 0;
        private IPiece activePiece;
        private bool tickGameplay = true;
        private IDisposable disposable;
        private IDisposable pieceStateDisposable;

        public void Start()
        {
            difficultyHandler = new DifficultyHandler(this, config);
            grid = new Grid(gridView);
            inputHandler = new InputHandler(this);
            SpawnRandomPiece();
            var d1 = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => { if (tickGameplay) secondsElapsed.Value++; });
            var d2 = GlobalMessagePipe.GetSubscriber<MGamePauseStateChanged>().Subscribe(args => { tickGameplay = !args.IsPaused; });
            disposable = Disposable.Combine(d1, d2);
        }

        private void SpawnRandomPiece()
        {
            activePiece = difficultyHandler.GetNextPiece().CreatePiece(grid);
            var startingCoords = new Vector2Int(grid.View.GridSize.x / 2, grid.View.PlayableGridHeight);
            if (grid.CheckIsMoveValid(activePiece, startingCoords))
            {
                pieceStateDisposable = activePiece.State.Subscribe(OnPieceStateChange);
                grid.MovePiece(activePiece, startingCoords);
            }
            else
            {
                //TODO: Implement game end
                Debug.Log("GAME END");
                GlobalMessagePipe.GetPublisher<MPlayAudio>().Publish(new MPlayAudio { Type = AudioType.Sfx, Clip = gameEndSFX });
                GlobalMessagePipe.GetPublisher<MToggleGameUI>().Publish(new MToggleGameUI {ScreenType = GameUIScreenType.GameOver});
                tickGameplay = false;
            }
        }

        private void OnPieceStateChange(PieceState state)
        {
            if (state != PieceState.Inactive) return;
            pieceStateDisposable.Dispose();
            HashSet<int> rowsToScan = new HashSet<int>();
            foreach (var coordOffset in activePiece.Size)
            {
                rowsToScan.Add((activePiece.CenterCoordinates + coordOffset).y);
            }
            int rowsCleared = 0;
            int highestRowCleared = int.MinValue;
            foreach (var y in rowsToScan)
            {
                if (grid.CheckAndClearRow(y))
                {
                    if (highestRowCleared < y)
                    {
                        highestRowCleared = y;
                    }
                    rowsCleared++;
                }
            }
            if (rowsCleared > 0)
            {
                if (rowsCleared - 1 >= rowClearSFX.Length)
                {
                    GlobalMessagePipe.GetPublisher<MPlayAudio>().Publish(new MPlayAudio { Type =  AudioType.Sfx, Clip = rowClearSFX[rowClearSFX.Length - 1] });
                }
                else
                {
                    GlobalMessagePipe.GetPublisher<MPlayAudio>().Publish(new MPlayAudio { Type =  AudioType.Sfx, Clip = rowClearSFX[rowsCleared - 1] });
                }
                grid.MoveRowsDown(highestRowCleared + 1, rowsCleared);
                score.Value += rowsCleared;
            }
            else
            {
                GlobalMessagePipe.GetPublisher<MPlayAudio>().Publish(new MPlayAudio { Type =  AudioType.Sfx, Clip = piecePlacedSFX });
            }
            GlobalMessagePipe.GetPublisher<MGameplayPieceFinished>().Publish(new MGameplayPieceFinished());
            SpawnRandomPiece();
        }
        
        private void TickGameplayLoop()
        {
            GlobalMessagePipe.GetPublisher<MGameplayTick>().Publish(new MGameplayTick());
        }
        
        private void Update()
        {
            if (!tickGameplay)
            {
                return;
            }
            
            timeSinceLastTick += Time.deltaTime;
            if (timeSinceLastTick >= (DropHeld ? difficultyHandler.GetGameplayTickInterval() * config.DropSpeedMultiplier : difficultyHandler.GetGameplayTickInterval()))
            {
                timeSinceLastTick = 0;
                TickGameplayLoop();
            }
        }

        private void OnDestroy()
        {
            pieceStateDisposable?.Dispose();
            disposable?.Dispose();
            inputHandler?.Cleanup();
            difficultyHandler?.Cleanup();
        }
    }
}
