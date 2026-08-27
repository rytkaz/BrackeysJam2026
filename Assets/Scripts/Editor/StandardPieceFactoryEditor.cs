using UnityEditor;
using UnityEngine;

namespace BlocksGame.Gameplay
{
    [CustomEditor(typeof(StandardPieceFactory))]
    public class StandardPieceFactoryEditor : Editor
    {
        private const int cellSize = 30;
        private SerializedProperty shapeDefaultProperty;
        private SerializedProperty shape90Property;
        private SerializedProperty shape180Property;
        private SerializedProperty shape270Property;
        private SerializedProperty gridSizeProperty;

        private void OnEnable()
        {
            shapeDefaultProperty = serializedObject.FindProperty("shapeDefault");
            shape90Property = serializedObject.FindProperty("shape90");
            shape180Property = serializedObject.FindProperty("shape180");
            shape270Property = serializedObject.FindProperty("shape270");
            gridSizeProperty = serializedObject.FindProperty("gridSize");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Shape Editor", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(gridSizeProperty);
            EditorGUILayout.Space(5);
            if (GUILayout.Button("Clear Shape"))
            {
                ClearShapeProperty(shapeDefaultProperty);
                ClearShapeProperty(shape90Property);
                ClearShapeProperty(shape180Property);
                ClearShapeProperty(shape270Property);
            }
            DrawShapeGrid(shapeDefaultProperty);
            EditorGUILayout.Space(5);
            DrawShapeGrid(shape90Property);
            EditorGUILayout.Space(5);
            DrawShapeGrid(shape180Property);
            EditorGUILayout.Space(5);
            DrawShapeGrid(shape270Property);
            
            serializedObject.ApplyModifiedProperties();
        }

        private void ClearShapeProperty(SerializedProperty targetProperty)
        {
            targetProperty.ClearArray();
            targetProperty.InsertArrayElementAtIndex(0);
            targetProperty.GetArrayElementAtIndex(0).vector2IntValue = Vector2Int.zero;
        }
        
        private void DrawShapeGrid(SerializedProperty targetProperty)
        {
            var coordinates = GetCurrentCoordinates(targetProperty);

            EditorGUILayout.BeginVertical();
            for (int y = gridSizeProperty.intValue - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < gridSizeProperty.intValue; x++)
                {
                    var coord = GridIndexToCenteredCoord(x, y);
                    bool isActive = coordinates.Contains(coord);

                    var prevColor = GUI.backgroundColor;
                    GUI.backgroundColor = isActive ? Color.cyan : Color.gray;

                    if (GUILayout.Button("", GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                    {
                        ToggleCoordinate(targetProperty, coord);
                    }

                    GUI.backgroundColor = prevColor;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private Vector2Int GridIndexToCenteredCoord(int x, int y)
        {
            int halfSize = gridSizeProperty.intValue / 2;
            return new Vector2Int(x - halfSize, y - halfSize);
        }

        private System.Collections.Generic.List<Vector2Int> GetCurrentCoordinates(SerializedProperty targetProperty)
        {
            var coords = new System.Collections.Generic.List<Vector2Int>();
            for (int i = 0; i < targetProperty.arraySize; i++)
            {
                coords.Add(targetProperty.GetArrayElementAtIndex(i).vector2IntValue);
            }
            return coords;
        }

        private void ToggleCoordinate(SerializedProperty targetProperty, Vector2Int coord)
        {
            var coords = GetCurrentCoordinates(targetProperty);
            int index = coords.IndexOf(coord);

            if (index >= 0)
            {
                targetProperty.DeleteArrayElementAtIndex(index);
            }
            else
            {
                int newIndex = targetProperty.arraySize;
                targetProperty.InsertArrayElementAtIndex(newIndex);
                targetProperty.GetArrayElementAtIndex(newIndex).vector2IntValue = coord;
            }
        }
    }
}