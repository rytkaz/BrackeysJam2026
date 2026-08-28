using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlocksGame.Gameplay
{
    public class MoverPieceBehaviour : IPieceBehaviour
    {
        private readonly IPiece piece;
        private readonly Grid grid;
        
        public MoverPieceBehaviour(IPiece piece, Grid grid)
        {
            this.piece = piece;
            this.grid = grid;
            piece.OnFinishedMovement += TriggerBehaviour;
            Debug.Log("Setup move behaviour");
        }

        private void TriggerBehaviour()
        {
            Debug.Log("Trigger move");
            piece.OnFinishedMovement -= TriggerBehaviour;
            var blocksToMove = Random.Range(1, piece.Size.Count);
            var blocks = new List<Vector2Int>(piece.Size);
            while (blocksToMove > 0 && blocks.Count > 0)
            {
                var availableSlots = grid.GetTopEmptySlots();
                if (availableSlots.Count == 0)
                {
                    break;
                }
                int blockIndex = Random.Range(0, blocks.Count);
                piece.MoveSingleBlock(blocks[blockIndex] + piece.CenterCoordinates, availableSlots[Random.Range(0, availableSlots.Count)]);
                blocks.RemoveAt(blockIndex);
                blocksToMove--;
            }
            Debug.Log("Move finished");
        }
    }
}
