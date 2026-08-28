using System;
using System.Collections.Generic;
using MessagePipe;
using R3;
using UnityEngine;
using DisposableBag = R3.DisposableBag;

namespace BlocksGame.Gameplay
{
    public enum PieceState
    {
        Moving,
        MovementFinished,
        Inactive
    }
    
    public class StandardPiece : IPiece
    {
        public event Action OnShapeChanged;
        public event Action OnCoordinatesChanged;
        
        public IList<Vector2Int> Size => state.Value == PieceState.Moving ? shapeRotations[currentRotation] : modifiedShape;
        public Vector2Int CenterCoordinates { get; private set; } = Vector2Int.zero;
        public ReadOnlyReactiveProperty<PieceState> State => state;
            
        private ReactiveProperty<PieceState> state = new(PieceState.Moving);
        private List<Vector2Int> modifiedShape = new();
        private Rotation currentRotation = Rotation.Deg0;
        private DisposableBag disposable;
        private readonly Grid grid;
        private readonly Dictionary<Rotation, Vector2Int[]> shapeRotations;
        private List<IPieceBehaviour> behaviours =  new();
        
        public StandardPiece(Grid grid, Dictionary<Rotation, Vector2Int[]> shapeRotations)
        {
            this.grid = grid;
            this.shapeRotations =  shapeRotations;
            GlobalMessagePipe.GetSubscriber<MGameplayTick>().Subscribe(OnGameplayTick).AddTo(ref disposable);
        }
        
        private void OnGameplayTick(MGameplayTick tick)
        {
            try
            {
                if (state.Value != PieceState.Moving) return;
            
                if (!grid.CheckIsMoveValid(this, CenterCoordinates + Vector2Int.down))
                {
                    modifiedShape.AddRange(shapeRotations[currentRotation]);
                    state.Value = PieceState.MovementFinished;
                    if (behaviours.Count > 0)
                    {
                        var isActivityEndBlocked = new ReactiveProperty<bool>(false);
                        foreach (var behaviour in behaviours)
                        {
                            isActivityEndBlocked.CombineLatest(behaviour.BlockPieceActivityEnd, (value1, value2) => value1 && value2);
                        }
                        isActivityEndBlocked.Subscribe((value) =>
                        {
                            if (!value)
                            {
                                BecomeInactive();
                            }
                        }).AddTo(ref disposable);
                    }
                    else
                    {
                        BecomeInactive();
                    }
                }
                else
                {
                    Move(Vector2Int.down);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        private void BecomeInactive()
        {
            state.Value = PieceState.Inactive;
        }
        
        public void ChangeCoordinates(Vector2Int newCoordinates)
        {
            foreach (var coordOffset in Size)
            {
                grid.FreeSlot(this, CenterCoordinates + coordOffset);
            }
            foreach (var coordOffset in Size)
            {
                grid.OccupySlot(this, newCoordinates + coordOffset);
            }
            CenterCoordinates = newCoordinates;
            OnCoordinatesChanged?.Invoke();
        }

        public void ClearSingleBlock(Vector2Int blockCoordinates)
        {
            if (!modifiedShape.Contains(blockCoordinates - CenterCoordinates)) return;
            modifiedShape.Remove(blockCoordinates - CenterCoordinates);
            grid.FreeSlot(this, blockCoordinates);
            OnShapeChanged?.Invoke();
        }

        public void MoveSingleBlock(Vector2Int blockCoordinates, Vector2Int targetCoordinates)
        {
            if (!modifiedShape.Contains(blockCoordinates - CenterCoordinates))
            {
                Debug.LogError($"Shape DOes not contain block: {blockCoordinates}");
                return;
            }
            modifiedShape.Remove(blockCoordinates - CenterCoordinates);
            modifiedShape.Add(targetCoordinates - CenterCoordinates);
            grid.FreeSlot(this, blockCoordinates);
            grid.OccupySlot(this, targetCoordinates);
            OnShapeChanged?.Invoke();
        }
        
        private void Move(Vector2Int direction)
        {
            if (state.Value != PieceState.Moving) return;
            grid.MovePiece(this, CenterCoordinates + direction);
        }
        
        public void Rotate()
        {
            if (state.Value != PieceState.Moving) return;
            var newRotation = Rotation.Deg0;
            if (currentRotation != Rotation.Deg270)
            {
                newRotation = currentRotation + 1;
            }
            if (!grid.CheckIsRotationValid(this, shapeRotations[newRotation])) return;
            foreach (var coordOffset in Size)
            {
                grid.FreeSlot(this, CenterCoordinates + coordOffset);
            }
            foreach (var coordOffset in shapeRotations[newRotation])
            {
                grid.OccupySlot(this, CenterCoordinates + coordOffset);
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
            foreach (var behaviour in behaviours)
            {
                behaviour.Cleanup();
            }
        }

        public void AddBehaviour(IPieceBehaviourFactory behaviourFactory)
        {
            var behaviour = behaviourFactory.CreateBehaviour(this, grid);
            behaviours.Add(behaviour);
        }
    }
}
