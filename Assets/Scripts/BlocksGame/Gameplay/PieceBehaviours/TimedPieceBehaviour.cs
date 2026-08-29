using System;
using System.Collections.Generic;
using MessagePipe;
using R3;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class TimedPieceBehaviour : IPieceBehaviour
    {
        public ReadOnlyReactiveProperty<bool> BlockPieceActivityEnd => blockPieceActivityEnd;

        private readonly ReactiveProperty<bool> blockPieceActivityEnd = new ReactiveProperty<bool>(false);
        private IDisposable disposable;
        private IPiece piece;
        private ReactiveProperty<int> durationRemaining;

        public TimedPieceBehaviour(IPiece piece, int duration)
        {
            this.piece = piece;
            disposable = piece.State.Subscribe(OnPieceStateChange);
            //Add 1 since this piece placement also ticks down duration
            durationRemaining = new ReactiveProperty<int>(duration + 1);
        }

        private void OnPieceStateChange(PieceState state)
        {
            if (state != PieceState.MovementFinished) return;
            foreach (var blockCoords in piece.Size)
            {
                piece.View.GetBlockByCoordinates(blockCoords).ShowClock(durationRemaining);
            }
            disposable?.Dispose();
            disposable = GlobalMessagePipe.GetSubscriber<MGameplayPieceFinished>().Subscribe(_ =>
            {
                try
                {
                    durationRemaining.Value--;
                    if (durationRemaining.Value > 0) return;
                    disposable?.Dispose();
                    var size = new List<Vector2Int>(piece.Size);
                    foreach (var blockCoords in size)
                    {
                        piece.ClearSingleBlock(blockCoords + piece.CenterCoordinates);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    throw;
                }
            });
        }

        public void Cleanup()
        {
            disposable?.Dispose();
        }
    }
}
