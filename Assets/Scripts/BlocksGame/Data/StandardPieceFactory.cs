using System.Collections.Generic;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public enum Rotation : int
    {
        Deg0 = 1,
        Deg90 = 2,
        Deg180 = 3,
        Deg270 = 4,
    }
    
    [CreateAssetMenu(fileName = "Piece", menuName = "BlockGame/Piece")]
    public class StandardPieceFactory : PieceFactory
    {
        [SerializeField] private PieceView pieceViewPrefab;
        [SerializeField] private Color color;
        [SerializeField, HideInInspector] private Vector2Int[] shapeCoordinates = new Vector2Int[] { Vector2Int.zero };
        [SerializeField, HideInInspector] private Vector2Int[] shape90 = new Vector2Int[] { Vector2Int.zero };
        [SerializeField, HideInInspector] private Vector2Int[] shape180 = new Vector2Int[] { Vector2Int.zero };
        [SerializeField, HideInInspector] private Vector2Int[] shape270 = new Vector2Int[] { Vector2Int.zero };
        [SerializeField, HideInInspector] private Vector2Int[] shapeDefault = new Vector2Int[] { Vector2Int.zero };

        [SerializeField, HideInInspector] private int gridSize = 7;

        public override IPiece CreatePiece(Grid grid)
        {
            var view = Instantiate(pieceViewPrefab, grid.View.transform);
            var piece = new StandardPiece(shapeCoordinates, grid, new Dictionary<Rotation, Vector2Int[]>()
            {
                {Rotation.Deg0, shapeDefault},
                {Rotation.Deg90, shape90},
                {Rotation.Deg180, shape180},
                {Rotation.Deg270, shape270},
            });
            view.SetPiece(piece, grid, color);
            return piece;
        }
    }

}
