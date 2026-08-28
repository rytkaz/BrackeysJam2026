using UnityEngine;

namespace BlocksGame.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "BlockGame/GameplayConfig")]
    public class GameplayConfig : ScriptableObject
    {
        [SerializeField, ReorderableList] private WeightedList<PieceFactory> pieces;
        
        [SerializeField] private float dropSpeedMultiplier;
        
        public  WeightedList<PieceFactory> Pieces => pieces;
        public float DropSpeedMultiplier => dropSpeedMultiplier;
    }
}
