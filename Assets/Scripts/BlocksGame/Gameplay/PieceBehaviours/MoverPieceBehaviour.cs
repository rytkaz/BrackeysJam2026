using System;
using System.Collections.Generic;
using DG.Tweening;
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
                var blockIndex = Random.Range(0, blocks.Count);
                var targetCoords  = availableSlots[Random.Range(0, availableSlots.Count)];
                piece.MoveSingleBlock(blocks[blockIndex] + piece.CenterCoordinates, targetCoords, true);
                var targetPos = grid.View.GetGridSlotPosition(targetCoords);
                piece.View.GetBlockByCoordinates(blocks[blockIndex]).transform.DOJump(targetPos, 1, 1, 0.4f).SetEase(Ease.OutQuad);
                blocks.RemoveAt(blockIndex);
                blocksToMove--;
            }
            Observable.Timer(TimeSpan.FromSeconds(0.6f)).Subscribe(_ =>
            {
                blockPieceActivityEnd.Value = false;
            });
        }

        public void Cleanup()
        {
            disposable?.Dispose();
        }
    }
}
