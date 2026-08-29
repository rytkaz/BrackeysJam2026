using System;
using R3;

namespace BlocksGame.Gameplay
{
    public class EnemyBehaviour : IPieceBehaviour
    {
        public ReadOnlyReactiveProperty<bool> BlockPieceActivityEnd => blockPieceActivityEnd;
        
        private readonly ReactiveProperty<bool> blockPieceActivityEnd = new ReactiveProperty<bool>(false);
        private readonly IDisposable disposable;
        private readonly IPiece piece;
        
        public EnemyBehaviour(IPiece piece, bool revealAtMovementEnd)
        {
            this.piece = piece;
            disposable = piece.State.Subscribe(OnPieceStateChange);
            if (!revealAtMovementEnd)
            {
                ShowEnemySprite();
            }
        }

        private void OnPieceStateChange(PieceState state)
        {
            if (state == PieceState.MovementFinished)
            {
                ShowEnemySprite();
            }
        }

        private void ShowEnemySprite()
        {
            piece.View.ShowEnemyIcon();
        }
        
        public void Cleanup()
        {
            disposable?.Dispose();
        }
    }
}
