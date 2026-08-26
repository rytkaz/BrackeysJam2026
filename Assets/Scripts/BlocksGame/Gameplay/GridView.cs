using NaughtyAttributes;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2 spacing;
        [SerializeField] private GameObject gridSlotBg;

        public Vector2 Spacing => spacing;
        public Vector2Int GridSize => gridSize;

        private Grid grid;
        private Vector2 centerOffset;
        
        public Vector3 GetGridSlotPosition(Vector2Int coordinates)
        {
            return new Vector3(coordinates.x * spacing.x - centerOffset.x, coordinates.y * spacing.y - centerOffset.y, 0);
        }

#if UNITY_EDITOR
        [Button("Rebuild Grid")]
        private void BuildGridBackground()
        {
            // Clear existing slots
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
                else
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
            }
            centerOffset = new Vector2(gridSize.x * spacing.x / 2, gridSize.y * spacing.y / 2);
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    var position = new Vector3(x * spacing.x - centerOffset.x, y * spacing.y - centerOffset.y, 0);
                    var slot = Instantiate(gridSlotBg, transform);
                    slot.transform.localPosition = position;
                }
            }
        }
#endif
    }
}
