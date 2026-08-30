using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class Grid
    {
        public GridView View { get; }
        
        private IPiece[,] pieces;
        
        public Grid(GridView gridView)
        {
            View = gridView;
            pieces = new IPiece[gridView.GridSize.x, gridView.GridSize.y];
        }

        public void MovePiece(IPiece piece, Vector2Int targetCordinates)
        {
            if (!CheckIsMoveValid(piece, targetCordinates))
            {
                return;
            }
            piece.ChangeCoordinates(targetCordinates);
        }

        public void OccupySlot(IPiece piece, Vector2Int coordinates)
        {
            if (coordinates.x >= pieces.GetLength(0) || coordinates.y >= pieces.GetLength(1))
            {
                Debug.LogError($"Trying to occupy invalid slot: {coordinates.x} ; {coordinates.y}");
                return;
            }
            if (pieces[coordinates.x, coordinates.y] != null)
            {
                pieces[coordinates.x, coordinates.y].ClearSingleBlock(new Vector2Int(coordinates.x, coordinates.y));
                Debug.LogWarning($"Trying to occupy already taken grid slot: {coordinates.x} ; {coordinates.y}");
                //return;
            }
            pieces[coordinates.x, coordinates.y] = piece;
        }

        public void FreeSlot(IPiece piece, Vector2Int coordinates)
        {
            if (coordinates.x >= pieces.GetLength(0) 
                || coordinates.y >= pieces.GetLength(1)
                || coordinates.x < 0
                || coordinates.y < 0)
            {
                return;
            }
            if (pieces[coordinates.x, coordinates.y] == null || pieces[coordinates.x, coordinates.y] != piece)
            {
                return;
            }
            pieces[coordinates.x, coordinates.y] = null;
        }

        public bool CheckAndClearRow(int y)
        {
            for (int x = 0; x < pieces.GetLength(0); x++)
            {
                if (pieces[x, y] == null)
                {
                    return false;
                }
            }
            for (int x = 0; x < pieces.GetLength(0); x++)
            {
                pieces[x,y].ClearSingleBlock(new Vector2Int(x,y));
            }
           
            return true;
        }

        public void MoveRowsDown(List<int> rowsCleared)
        {
            foreach (var rowCleared in rowsCleared)
            {
                //Move all other rows down
                for (int y = rowCleared; y < pieces.GetLength(1); y++)
                {
                    for (int x = 0; x < pieces.GetLength(0); x++)
                    {
                        if (pieces[x, y] == null)
                        {
                            continue;
                        }
                        pieces[x,y].MoveSingleBlock(new Vector2Int(x,y), new Vector2Int(x,y - 1));
                    }
                }
            }
        }
        
        public bool CheckIsRotationValid(IPiece piece, Vector2Int[] newSize)
        {
            return newSize.All(coordOffset =>
            {
                var targetCoords =  piece.CenterCoordinates + coordOffset;
                if (CheckOutOfBounds(targetCoords))
                {
                    Debug.Log($"Out of bounds: {targetCoords} ; GridSize: {pieces.GetLength(0)} ; {pieces.GetLength(1)}");
                    return false;
                }
                return IsGridSlotEmpty(targetCoords) || pieces[targetCoords.x, targetCoords.y] == piece;
            });
        }
        
        public bool CheckIsMoveValid(IPiece piece, Vector2Int targetCoordinates)
        {
            return piece.Size.All(coordOffset =>
            {
                var targetCoords =  new Vector2Int(targetCoordinates.x, targetCoordinates.y) + coordOffset;
                if (CheckOutOfBounds(targetCoords))
                {
                    return false;
                }
                return IsGridSlotEmpty(targetCoords) || pieces[targetCoords.x, targetCoords.y] == piece;
            });
        }

        public bool CheckOutOfBounds(Vector2Int coordinates)
        {
            return coordinates.x >= pieces.GetLength(0)
                   || coordinates.y >= pieces.GetLength(1)
                   || coordinates.x < 0
                   || coordinates.y < 0;
        }

        public bool IsGridSlotEmpty(Vector2Int coordinates)
        {
            return pieces[coordinates.x, coordinates.y] == null;
        }

        public List<Vector2Int> GetTopEmptySlots()
        {
            var emptySlots = new List<Vector2Int>();
            for (int x = 0; x < pieces.GetLength(0); x++)
            {
                for (int y = 0; y < pieces.GetLength(1); y++)
                {
                    if (pieces[x, y] != null)
                    {
                        continue;
                    }
                    emptySlots.Add(new Vector2Int(x, y));
                    break;
                }
            }
            return emptySlots;
        }
    }
}
