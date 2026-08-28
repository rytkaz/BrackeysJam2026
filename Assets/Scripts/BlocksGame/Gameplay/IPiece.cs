using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public interface IPiece
    {
        public event Action OnCoordinatesChanged;
        public event Action OnShapeChanged;
        public ReadOnlyReactiveProperty<PieceState> State { get; }
        public IList<Vector2Int> Size { get; }
        public Vector2Int CenterCoordinates { get; }
        public PieceView View { get; }
        public void Rotate();
        public void MoveHorizontal(int direction);
        public void ChangeCoordinates(Vector2Int coordinates);
        public void ClearSingleBlock(Vector2Int coordinates);
        public void MoveSingleBlock(Vector2Int blockCoordinates, Vector2Int targetCoordinates, bool skipShapeChangeTrigger = false);
        public void AddBehaviour(IPieceBehaviourFactory behaviourFactory);
        public void Cleanup();
    }
}
