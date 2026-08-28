namespace BlocksGame.Gameplay
{
    public interface IPieceBehaviourFactory
    {
        public IPieceBehaviour CreateBehaviour(IPiece piece, Grid grid);
    }
}
