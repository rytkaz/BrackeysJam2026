using UnityEngine;

namespace BlocksGame.Gameplay
{
    public abstract class PieceFactory : ScriptableObject
    {
        public abstract IPiece CreatePiece(Grid grid);
    }
}
