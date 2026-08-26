using UnityEngine;

namespace BlocksGame.Gameplay
{
    [CreateAssetMenu(fileName = "Piece", menuName = "BlockGame/Piece")]
    public class StandardPieceFactory : ScriptableObject, IPieceFactory
    {
        [SerializeField] private PieceView pieceViewPrefab;
        [SerializeField] private Color color;
        [SerializeField] private Vector2Int[] shapeCoordinates = new Vector2Int[] { Vector2Int.zero };
        
        public IPiece CreatePiece(Grid grid)
        {
            var view = Instantiate(pieceViewPrefab);
            var piece = new StandardPiece(shapeCoordinates, grid);
            view.SetPiece(piece, grid, color);
            return piece;
        }
    }

}
