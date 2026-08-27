using System;
using System.Collections.Generic;
using MessagePipe;
using R3;
using UnityEngine;
using DisposableBag = R3.DisposableBag;

namespace BlocksGame.Gameplay
{
    public class StandardPiece : IPiece
    {
        public event Action OnFinishedMovement;
        public event Action OnShapeChanged;
        public event Action OnCoordinatesChanged;
        public Vector2Int[] Size => shapeRotations[currentRotation];
        public Vector2Int Coordinates { get; private set; } = Vector2Int.zero;

        private Rotation currentRotation = Rotation.Deg0;
        private DisposableBag disposable;
        private bool isActive = true;
        private readonly Grid grid;
        private readonly Dictionary<Rotation, Vector2Int[]> shapeRotations;

        public StandardPiece(Vector2Int[] size, Grid grid, Dictionary<Rotation, Vector2Int[]> shapeRotations)
        {
            this.grid = grid;
            this.shapeRotations =  shapeRotations;
            GlobalMessagePipe.GetSubscriber<MGameplayTick>().Subscribe(OnGameplayTick).AddTo(ref disposable);
        }
        
        private void OnGameplayTick(MGameplayTick tick)
        {
            if (isActive)
            {
                if (!grid.CheckIsMoveValid(this, Coordinates + Vector2Int.down))
                {
                    isActive = false;
                    OnFinishedMovement?.Invoke();
                }
                else
                {
                    Move(Vector2Int.down);
                }
            }
        }

        public void ChangeCoordinates(Vector2Int newCoordinates)
        {
            foreach (var coordOffset in Size)
            {
                grid.FreeSlot(this, Coordinates + coordOffset);
            }
            foreach (var coordOffset in Size)
            {
                grid.OccupySlot(this, newCoordinates + coordOffset);
            }
            Coordinates = newCoordinates;
            OnCoordinatesChanged?.Invoke();
        }
        
        private void Move(Vector2Int direction)
        {
            grid.MovePiece(this, Coordinates + direction);
        }
        
        public void Rotate()
        {
            Rotation newRotation = Rotation.Deg0;
            if (currentRotation != Rotation.Deg270)
            {
                newRotation = currentRotation + 1;
            }
            if (!grid.CheckIsRotationValid(this, shapeRotations[newRotation])) return;
            foreach (var coordOffset in Size)
            {
                grid.FreeSlot(this, Coordinates + coordOffset);
            }
            foreach (var coordOffset in shapeRotations[newRotation])
            {
                grid.OccupySlot(this, Coordinates + coordOffset);
            }
            currentRotation = newRotation;
            OnShapeChanged?.Invoke();
        }
        
        public void MoveHorizontal(int direction)
        {
            Move(Vector2Int.right * direction);
        }

        public void Cleanup()
        {
            disposable.Dispose();
        }
    }
}
