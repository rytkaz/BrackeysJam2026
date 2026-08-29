using UnityEngine;

namespace BlocksGame.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "BlockGame/GameplayConfig")]
    public class GameplayConfig : ScriptableObject
    {
        [SerializeField] private WeightedList<PieceFactory> pieces;
        [SerializeField] private WeightedList<PieceFactory> enemyPieces;
        [SerializeField] private WeightedList<PieceFactory> bossPieces;
        [Space(5)]
        [Header(("Balance values"))]
        [SerializeField] private float gameplayTickInterval = 1;
        [SerializeField] private float dropSpeedMultiplier;
        [SerializeField] private int piecesUntilEnemiesStartSpawn = 5;
        [SerializeField, Range(0,1)] private float initialEnemyFrequency = 0.5f;
        [SerializeField] private Vector2Int piecesUntilBoss;
        [SerializeField] private float enemyFrequencyMultiplierPerMin = 1.05f;
        [SerializeField, Range(0,1)] private float maxEnemyFrequency = 0.95f;
        
        public WeightedList<PieceFactory> Pieces => pieces;
        public WeightedList<PieceFactory> EnemyPieces => enemyPieces;
        public WeightedList<PieceFactory> BossPieces => bossPieces;
        public float InitialEnemyFrequency => initialEnemyFrequency;
        public int PiecesUntilEnemiesStartSpawn => piecesUntilEnemiesStartSpawn;
        public Vector2Int PiecesUntilBoss => piecesUntilBoss;
        public float MaxEnemyFrequency => maxEnemyFrequency;
        public float EnemyFrequencyMultiplierPerMin => enemyFrequencyMultiplierPerMin;
        public float DropSpeedMultiplier => dropSpeedMultiplier;
        public float GameplayTickInterval => gameplayTickInterval;
    }
}
