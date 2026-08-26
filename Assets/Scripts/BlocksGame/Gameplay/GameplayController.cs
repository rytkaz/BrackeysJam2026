using System;
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
        
        private ReactiveProperty<int> secondsElapsed = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> score = new ReactiveProperty<int>(0);

        private float gameplayTickInterval = 1;
        private float timeSinceLastTick = 0;
        private Grid grid;
        private IPiece activePiece;
        
        public void Start()
        {
            grid = new Grid(gridView);
            SpawnRandomPiece();
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => secondsElapsed.Value++);
        }
        
        private void SpawnRandomPiece()
        {
            activePiece = config.StandardPieceFactories[UnityEngine.Random.Range(0, config.StandardPieceFactories.Length)].CreatePiece(grid);
            activePiece.OnFinishedMovement += OnActivePieceFinishedMovement;
        }

        private void OnActivePieceFinishedMovement()
        {
            activePiece.OnFinishedMovement -= OnActivePieceFinishedMovement;
            
        }
        
        private void TickGameplayLoop()
        {
            GlobalMessagePipe.GetPublisher<MGameplayTick>().Publish(new MGameplayTick());
        }
        
        private void Update()
        {
            timeSinceLastTick += Time.deltaTime;
            if (timeSinceLastTick >= gameplayTickInterval)
            {
                timeSinceLastTick = 0;
                TickGameplayLoop();
            }
        }
    }
}
