using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] private SingeBlockView singleBlockPrefab;
        
        private IDisposable disposable;
        private IPiece piece;
        private List<SingeBlockView> spawnedBlocks = new List<SingeBlockView>();
        private Dictionary<Vector2Int, SingeBlockView> blocks = new Dictionary<Vector2Int, SingeBlockView>();
        private Grid grid;
        private Color color;
        private bool isEnemy = false;
        
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
                    var newBlock = Instantiate(singleBlockPrefab, transform);
                    spawnedBlocks.Add(newBlock);
                    newBlock.SetColor(color);
                    if (isEnemy)
                    {
                        newBlock.EnableEnemySprite();
                    }
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

        public void ShowEnemyIcon()
        {
            isEnemy = true;
            foreach (var block in spawnedBlocks)
            {
                block.EnableEnemySprite();
            }
        }
        
        public SingeBlockView GetBlockByCoordinates(Vector2Int coordinates)
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
