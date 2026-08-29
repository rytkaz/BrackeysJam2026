using System;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    [Serializable]
    public class EnemyBehaviourFactory : IPieceBehaviourFactory
    {
        [SerializeField] private bool revealAtMovementEnd = true;
        
        public IPieceBehaviour CreateBehaviour(IPiece piece, Grid grid)
        {
            return new EnemyBehaviour(piece, revealAtMovementEnd);
        }
    }
}
