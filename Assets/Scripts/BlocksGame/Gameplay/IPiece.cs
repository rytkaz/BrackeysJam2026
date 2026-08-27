using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public interface IPiece
    {
        public event Action OnFinishedMovement;
        public event Action OnCoordinatesChanged;
        public event Action OnShapeChanged;
        public IList<Vector2Int> Size { get; }
        public Vector2Int CenterCoordinates { get; }
        public void Rotate();
        public void MoveHorizontal(int direction);
        public void ChangeCoordinates(Vector2Int coordinates);
        public void ClearSingleBlock(Vector2Int coordinates);
        public void MoveSingleBlock(Vector2Int blockCoordinates, Vector2Int targetCoordinates);
        public void Cleanup();
    }
}
