using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MessagePipe;
using R3;
using UnityEngine;
using DisposableBag = R3.DisposableBag;

namespace BlocksGame.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private GameplayConfig config;
        [SerializeField] private GridView gridView;
        
        public ReadOnlyReactiveProperty<int> SecondsElapsed => secondsElapsed;
        public ReadOnlyReactiveProperty<int> Score => score;
        public IPiece ActivePiece => activePiece;
        [HideInInspector] public bool DropHeld = false;
        
        private ReactiveProperty<int> secondsElapsed = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> score = new ReactiveProperty<int>(0);

        private float gameplayTickInterval = 1;
        private float timeSinceLastTick = 0;
        private Grid grid;
        private InputHandler inputHandler;
        private IPiece activePiece;
        private bool tickGameplay = true;
        private IDisposable timer;
        private IDisposable pieceStateDisposable;
        
        public void Start()
        {
            grid = new Grid(gridView);
            inputHandler = new InputHandler(this);
            SpawnRandomPiece();
            timer = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => { if (tickGameplay) secondsElapsed.Value++; });
        }
        
        private void SpawnRandomPiece()
        {
            activePiece = config.Pieces.GetRandom().CreatePiece(grid);
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
                grid.MoveRowsDown(highestRowCleared + 1, rowsCleared);
                score.Value += rowsCleared;
            }
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
            if (timeSinceLastTick >= (DropHeld ? gameplayTickInterval * config.DropSpeedMultiplier : gameplayTickInterval))
            {
                timeSinceLastTick = 0;
                TickGameplayLoop();
            }
        }

        private void OnDestroy()
        {
            pieceStateDisposable?.Dispose();
            timer?.Dispose();
            inputHandler?.Cleanup();
        }
    }
}
