using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer singleBlockPrefab;
        
        private IDisposable disposable;
        private IPiece piece;
        private List<SpriteRenderer> spawnedBlocks = new List<SpriteRenderer>();
        private Grid grid;
        private Color color;
        
        public void SetPiece(IPiece piece, Grid grid, Color color = default)
        {
            this.piece = piece;
            this.grid = grid;
            this.color = color;
            piece.OnShapeChanged += UpdateBlocks;
            piece.OnCoordinatesChanged += OnCoordinatesChanged;
            OnCoordinatesChanged();
            UpdateBlocks();
        }

        private void OnCoordinatesChanged()
        {
            transform.position = grid.View.GetGridSlotPosition(piece.CenterCoordinates);
        }
        
        private void UpdateBlocks()
        {
            for (int i = 0; i < piece.Size.Count; i++)
            {
                if (spawnedBlocks.Count <= i)
                {
                    spawnedBlocks.Add(Instantiate(singleBlockPrefab, transform));
                    spawnedBlocks[i].color = color;
                }
                spawnedBlocks[i].transform.position = grid.View.GetGridSlotPosition(piece.CenterCoordinates + piece.Size[i]);
                spawnedBlocks[i].gameObject.SetActive(true);
            }
            for (int i = piece.Size.Count; i < spawnedBlocks.Count; i++)
            {
                spawnedBlocks[i].gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (piece == null) return;
            piece.OnShapeChanged -= UpdateBlocks;
            piece.OnCoordinatesChanged -= OnCoordinatesChanged;
            piece.Cleanup();
        }
    }
}
