using UnityEngine;

namespace BlocksGame.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "BlockGame/GameplayConfig")]
    public class GameplayConfig : ScriptableObject
    {
        [SerializeField, ReorderableList, ReferencePicker(TypeGrouping = TypeGrouping.None)] private PieceFactory[] standardPieceFactories;
        [SerializeField] private float dropSpeedMultiplier;
        
        public PieceFactory[] StandardPieceFactories => standardPieceFactories;
        public float DropSpeedMultiplier => dropSpeedMultiplier;
    }
}
