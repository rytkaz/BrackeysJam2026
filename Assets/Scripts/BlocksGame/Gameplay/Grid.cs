using System.Linq;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class Grid
    {
        private IPiece[,] pieces;
        
        public Grid(GridView gridView)
        {
            pieces = new IPiece[gridView.GridSize.x, gridView.GridSize.y];
        }

        public void MovePiece(IPiece piece, Vector2Int coordinatesChange)
        {
            if (!CheckIsMoveValid(piece, coordinatesChange))
            {
                return;
            }
            piece.ChangeCoordinates(piece.Coordinates.CurrentValue + coordinatesChange);
        }

        public bool CheckIsMoveValid(IPiece piece, Vector2Int coordinatesChange)
        {
            return piece.Size.All(coord => IsGridSlotEmpty(piece.Coordinates.CurrentValue + coord));
        }
        
        public bool IsGridSlotEmpty(Vector2Int coordinates)
        {
            return pieces[coordinates.x, coordinates.y] == null;
        }
    }
}
