using UnityEngine;

namespace BlocksGame.Gameplay
{
    [CreateAssetMenu(fileName = "GameplayConfig", menuName = "BlockGame/GameplayConfig")]
    public class GameplayConfig : ScriptableObject
    {
        [SerializeField] private IPieceFactory[] standardPieceFactories;
        
        public IPieceFactory[] StandardPieceFactories => standardPieceFactories;
    }
}
