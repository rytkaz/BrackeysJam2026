using UnityEditor;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    [CustomEditor(typeof(StandardPieceFactory))]
    public class StandardPieceFactoryEditor : Editor
    {
        private const int GridSize = 7;
        private const int CellSize = 30;
        private SerializedProperty shapeCoordinatesProperty;

        private void OnEnable()
        {
            shapeCoordinatesProperty = serializedObject.FindProperty("shapeCoordinates");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Shape Editor", EditorStyles.boldLabel);

            DrawShapeGrid();

            EditorGUILayout.Space(5);
            if (GUILayout.Button("Clear Shape"))
            {
                shapeCoordinatesProperty.ClearArray();
                shapeCoordinatesProperty.InsertArrayElementAtIndex(0);
                shapeCoordinatesProperty.GetArrayElementAtIndex(0).vector2IntValue = Vector2Int.zero;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawShapeGrid()
        {
            var coordinates = GetCurrentCoordinates();
            int halfSize = GridSize / 2;

            EditorGUILayout.BeginVertical();
            for (int y = GridSize - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < GridSize; x++)
                {
                    var coord = GridIndexToCenteredCoord(x, y);
                    bool isActive = coordinates.Contains(coord);

                    var prevColor = GUI.backgroundColor;
                    GUI.backgroundColor = isActive ? Color.cyan : Color.gray;

                    if (GUILayout.Button("", GUILayout.Width(CellSize), GUILayout.Height(CellSize)))
                    {
                        ToggleCoordinate(coord);
                    }

                    GUI.backgroundColor = prevColor;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private Vector2Int GridIndexToCenteredCoord(int x, int y)
        {
            int halfSize = GridSize / 2;
            return new Vector2Int(x - halfSize, y - halfSize);
        }

        private System.Collections.Generic.List<Vector2Int> GetCurrentCoordinates()
        {
            var coords = new System.Collections.Generic.List<Vector2Int>();
            for (int i = 0; i < shapeCoordinatesProperty.arraySize; i++)
            {
                coords.Add(shapeCoordinatesProperty.GetArrayElementAtIndex(i).vector2IntValue);
            }
            return coords;
        }

        private void ToggleCoordinate(Vector2Int coord)
        {
            var coords = GetCurrentCoordinates();
            int index = coords.IndexOf(coord);

            if (index >= 0)
            {
                shapeCoordinatesProperty.DeleteArrayElementAtIndex(index);
            }
            else
            {
                int newIndex = shapeCoordinatesProperty.arraySize;
                shapeCoordinatesProperty.InsertArrayElementAtIndex(newIndex);
                shapeCoordinatesProperty.GetArrayElementAtIndex(newIndex).vector2IntValue = coord;
            }
        }
    }
}