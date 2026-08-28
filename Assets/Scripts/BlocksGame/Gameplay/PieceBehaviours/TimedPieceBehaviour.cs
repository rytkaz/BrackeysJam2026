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
        private int durationRemaining = 0;

        public TimedPieceBehaviour(IPiece piece, int duration)
        {
            this.piece = piece;
            disposable = piece.State.Subscribe(OnPieceStateChange);
            //Add 1 since this piece placement also ticks down duration
            durationRemaining = duration + 1;
        }

        private void OnPieceStateChange(PieceState state)
        {
            if (state != PieceState.MovementFinished) return;
            disposable?.Dispose();
            disposable = GlobalMessagePipe.GetSubscriber<MGameplayPieceFinished>().Subscribe(_ =>
            {

                durationRemaining--;
                if (durationRemaining > 0) return;
                disposable?.Dispose();
                var size = new List<Vector2Int>(piece.Size);
                foreach (var coordsOffset in size)
                {
                    piece.ClearSingleBlock(coordsOffset + piece.CenterCoordinates);
                }

            });
        }

        public void Cleanup()
        {
            disposable?.Dispose();
        }
    }
}
