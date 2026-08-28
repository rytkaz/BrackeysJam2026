using UnityEngine;

namespace BlocksGame.Gameplay
{
    [CreateAssetMenu(fileName = "EnemyPiece", menuName = "BlockGame/EnemyPiece")]
    public class EnemyPieceFactory : PieceFactory
    {
        [ReorderableList, SerializeReference, ReferencePicker] private IPieceBehaviourFactory[] behaviours = { new MoverPieceBehaviourFactory() }; 
        [SerializeField, ReorderableList] private PieceFactory[] basePieceFactories;

        public override IPiece CreatePiece(Grid grid)
        {
            var basePiece = basePieceFactories[Random.Range(0, basePieceFactories.Length)].CreatePiece(grid);
            foreach (var behaviour in behaviours)
            {
                behaviour.CreateBehaviour(basePiece, grid);
            }
            return basePiece;
        }
    }
}
