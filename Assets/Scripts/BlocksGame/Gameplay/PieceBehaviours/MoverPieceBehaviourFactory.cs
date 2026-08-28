using System;

namespace BlocksGame.Gameplay
{
    [Serializable]
    public class MoverPieceBehaviourFactory : IPieceBehaviourFactory
    {
        public IPieceBehaviour CreateBehaviour(IPiece piece, Grid grid)
        {
            return new MoverPieceBehaviour(piece, grid);
        }
    }
}
