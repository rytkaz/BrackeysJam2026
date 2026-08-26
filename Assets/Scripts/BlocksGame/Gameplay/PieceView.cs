using System;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer singleBlockPrefab;
        
        private IPiece piece;
        
        public void SetPiece(IPiece piece, Grid grid, Color color = default)
        {
            this.piece = piece;
            
        }

        private void OnDestroy()
        {
            piece.Cleanup();
        }
    }
}
