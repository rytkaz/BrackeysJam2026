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
        private Dictionary<Vector2Int, SpriteRenderer> blocks = new Dictionary<Vector2Int, SpriteRenderer>();
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
            if (piece.Size.Count == 0)
            {
                Destroy(gameObject);
                return;
            }
            blocks.Clear();
            for (int i = 0; i < piece.Size.Count; i++)
            {
                if (spawnedBlocks.Count <= i)
                {
                    spawnedBlocks.Add(Instantiate(singleBlockPrefab, transform));
                    spawnedBlocks[i].color = color;
                }
                spawnedBlocks[i].transform.position = grid.View.GetGridSlotPosition(piece.CenterCoordinates + piece.Size[i]);
                spawnedBlocks[i].gameObject.SetActive(true);
                blocks.Add(piece.Size[i], spawnedBlocks[i]);
            }
            for (int i = piece.Size.Count; i < spawnedBlocks.Count; i++)
            {
                spawnedBlocks[i].gameObject.SetActive(false);
            }
        }

        public SpriteRenderer GetBlockByCoordinates(Vector2Int coordinates)
        {
            return blocks[coordinates];
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
