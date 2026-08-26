using System;
using MessagePipe;
using R3;
using UnityEngine;
using DisposableBag = R3.DisposableBag;

namespace BlocksGame.Gameplay
{
    public class StandardPiece : IPiece
    {
        public event Action OnFinishedMovement;
        public Vector2Int[] Size { get; }
        public ReadOnlyReactiveProperty<Vector2Int> Coordinates => coordinates;
        
        private DisposableBag disposable;
        private bool isActive = true;
        private ReactiveProperty<Vector2Int> coordinates = new ReactiveProperty<Vector2Int>(Vector2Int.zero);
        private readonly Grid grid;
        
        public StandardPiece(Vector2Int[] size, Grid grid)
        {
            this.grid = grid;
            this.Size = size;
            GlobalMessagePipe.GetSubscriber<MGameplayTick>().Subscribe(OnGameplayTick).AddTo(ref disposable);
        }
        
        private void OnGameplayTick(MGameplayTick tick)
        {
            if (isActive)
            {
                Move(Vector2Int.down);
                if (grid.CheckIsMoveValid(this, Vector2Int.down))
                {
                    isActive = false;
                    OnFinishedMovement?.Invoke();
                }
            }
        }

        public void ChangeCoordinates(Vector2Int newCoordinates)
        {
            coordinates.Value = newCoordinates;
        }
        
        private void Move(Vector2Int direction)
        {
            grid.MovePiece(this, direction);
        }
        
        public void Rotate()
        {
            //TODO: Implement piece rotation
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
