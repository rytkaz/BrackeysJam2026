using System;
using R3;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public interface IPiece
    {
        public event Action OnFinishedMovement;
        public Vector2Int[] Size { get; }
        public ReadOnlyReactiveProperty<Vector2Int> Coordinates { get; }
        public void Rotate();
        public void MoveHorizontal(int direction);
        public void ChangeCoordinates(Vector2Int coordinates);
        public void Cleanup();
    }
}
