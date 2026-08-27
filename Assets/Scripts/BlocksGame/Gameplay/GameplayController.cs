using System;
using System.Collections.Generic;
using MessagePipe;
using R3;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private GameplayConfig config;
        [SerializeField] private GridView gridView;
        
        public ReadOnlyReactiveProperty<int> SecondsElapsed => secondsElapsed;
        public ReadOnlyReactiveProperty<int> Score => score;
        public IPiece ActivePiece => activePiece;
        public bool DropHeld = false;
        
        private ReactiveProperty<int> secondsElapsed = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> score = new ReactiveProperty<int>(0);

        private float gameplayTickInterval = 1;
        private float timeSinceLastTick = 0;
        private Grid grid;
        private InputHandler inputHandler;
        private IPiece activePiece;
        private bool tickGameplay = true;
        
        public void Start()
        {
            grid = new Grid(gridView);
            inputHandler = new InputHandler(this);
            SpawnRandomPiece();
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => { if (tickGameplay) secondsElapsed.Value++; });
        }
        
        private void SpawnRandomPiece()
        {
            activePiece = config.StandardPieceFactories[UnityEngine.Random.Range(0, config.StandardPieceFactories.Length)].CreatePiece(grid);
            var startingCoords = new Vector2Int(grid.View.GridSize.x / 2, grid.View.PlayableGridHeight);
            if (grid.CheckIsMoveValid(activePiece, startingCoords))
            {
                activePiece.OnFinishedMovement += OnActivePieceFinishedMovement;
                grid.MovePiece(activePiece, startingCoords);
            }
            else
            {
                //TODO: Implement game end
                Debug.Log("GAME END");
                tickGameplay = false;
            }
        }

        private void OnActivePieceFinishedMovement()
        {
            activePiece.OnFinishedMovement -= OnActivePieceFinishedMovement;
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
    }
}
