using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlocksGame.Gameplay
{
    public class MoverPieceBehaviour : IPieceBehaviour
    {
        public ReadOnlyReactiveProperty<bool> BlockPieceActivityEnd => blockPieceActivityEnd;

        private readonly ReactiveProperty<bool> blockPieceActivityEnd = new ReactiveProperty<bool>(true);
        private readonly IPiece piece;
        private readonly Grid grid;
        private readonly IDisposable disposable;
        
        public MoverPieceBehaviour(IPiece piece, Grid grid)
        {
            this.piece = piece;
            this.grid = grid;
            disposable = piece.State.Subscribe(OnPieceStateChange);
        }

        private void OnPieceStateChange(PieceState state)
        {
            if (state != PieceState.MovementFinished) return;
            var blocksToMove = Random.Range(1, piece.Size.Count);
            var blocks = new List<Vector2Int>(piece.Size);
            while (blocksToMove > 0 && blocks.Count > 0)
            {
                var availableSlots = grid.GetTopEmptySlots();
                if (availableSlots.Count == 0)
                {
                    break;
                }
                int blockIndex = Random.Range(0, blocks.Count);
                piece.MoveSingleBlock(blocks[blockIndex] + piece.CenterCoordinates, availableSlots[Random.Range(0, availableSlots.Count)]);
                blocks.RemoveAt(blockIndex);
                blocksToMove--;
            }
            blockPieceActivityEnd.Value = false;
        }

        public void Cleanup()
        {
            disposable?.Dispose();
        }
    }
}
