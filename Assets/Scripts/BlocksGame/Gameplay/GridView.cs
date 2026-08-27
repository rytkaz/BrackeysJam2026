using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace BlocksGame.Gameplay
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private int playableGridHeight;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2 spacing;
        [SerializeField] private GameObject gridSlotBg;

        public int PlayableGridHeight  => playableGridHeight;
        public Vector2Int GridSize => gridSize;
        
        private Grid grid;
        private Vector2 centerOffset;

        private void Awake()
        {
            centerOffset = new Vector2(gridSize.x * spacing.x / 2, playableGridHeight * spacing.y / 2);
        }

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
            centerOffset = new Vector2(gridSize.x * spacing.x / 2, playableGridHeight * spacing.y / 2);
            for (int y = 0; y < playableGridHeight; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    var position = new Vector3(x * spacing.x - centerOffset.x, y * spacing.y - centerOffset.y, 0);
                    var slot = PrefabUtility.InstantiatePrefab(gridSlotBg, transform) as GameObject;
                    slot.transform.localPosition = position;
                }
            }
        }
#endif
    }
}
