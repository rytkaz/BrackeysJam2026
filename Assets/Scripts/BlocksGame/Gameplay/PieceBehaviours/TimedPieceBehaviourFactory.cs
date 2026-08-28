using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlocksGame.Gameplay
{
    [Serializable]
    public class TimedPieceBehaviourFactory : IPieceBehaviourFactory
    {
        [SerializeField] private Vector2Int durationRange;
            
        public IPieceBehaviour CreateBehaviour(IPiece piece, Grid grid)
        {
            return new TimedPieceBehaviour(piece, Random.Range(durationRange.x, durationRange.y));
        }
    }
}
