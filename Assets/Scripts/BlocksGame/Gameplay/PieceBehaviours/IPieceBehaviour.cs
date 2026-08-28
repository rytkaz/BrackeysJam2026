using R3;

namespace BlocksGame.Gameplay
{
    public interface IPieceBehaviour
    {
        public ReadOnlyReactiveProperty<bool> BlockPieceActivityEnd { get; }
        public void Cleanup();
    }
}
