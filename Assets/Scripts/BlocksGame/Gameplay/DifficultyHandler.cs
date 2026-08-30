using System;
using MessagePipe;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlocksGame.Gameplay
{
    public class DifficultyHandler
    {
        private readonly GameplayController controller;
        private readonly GameplayConfig config;
        private readonly IDisposable disposable;
        private int piecesUntilNextBoss = 0;
        private WeightedList<PieceFactory> unusedBosses;
        private int totalPiecesSpawned = 0;
        
        public DifficultyHandler(GameplayController controller, GameplayConfig config)
        {
            this.controller = controller;
            this.config = config;
            piecesUntilNextBoss = Random.Range(config.PiecesUntilBoss.x,  config.PiecesUntilBoss.y);
            disposable = GlobalMessagePipe.GetSubscriber<MGameplayPieceFinished>().Subscribe(_ =>
            {
                piecesUntilNextBoss--;
                totalPiecesSpawned++;
            });
        }

        public float GetGameplayTickInterval()
        {
            return config.GameplayTickInterval;
        }

        private float GetEnemyFrequency()
        {
            return Mathf.Min(
                config.InitialEnemyFrequency * Mathf.Pow(config.EnemyFrequencyMultiplierPerMin, controller.SecondsElapsed.CurrentValue / 30),
                config.MaxEnemyFrequency);
        }

        public PieceFactory GetNextPiece()
        {
            if (piecesUntilNextBoss <= 0)
            {
                piecesUntilNextBoss = Random.Range(config.PiecesUntilBoss.x,  config.PiecesUntilBoss.y);
                if (unusedBosses == null || unusedBosses.Count == 0)
                {
                    unusedBosses = new WeightedList<PieceFactory>(config.BossPieces);
                }
                return unusedBosses.GetRandom(true);
            }
            if (totalPiecesSpawned >= config.PiecesUntilEnemiesStartSpawn && Random.Range(0f, 1f) < GetEnemyFrequency())
            {
                return config.EnemyPieces.GetRandom();
            }
            return config.Pieces.GetRandom();
        }

        public void Cleanup()
        {
            disposable?.Dispose();
        }
    }
}
