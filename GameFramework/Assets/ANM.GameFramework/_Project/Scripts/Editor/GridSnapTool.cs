#if UNITY_EDITOR
/*
 * GridSnapTool - 
 * Created by : Allan N. Murillo
 * Last Edited : 9/22/2021
 */

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using ANM.Framework.Extensions;

namespace ANM.EditorUtils
{
    public class GridSnapTool : EditorWindow
    {
        [MenuItem("Tools/ANM/Grid Snapper")]
        public static void OpenEditorWindow() => GetWindow<GridSnapTool>("Grid Snap");

        public enum GridType
        {
            Cartesian,
            Polar
        }

        public float gridSize = 1f;
        public int angularDivisions = 24;
        public GridType gridType = GridType.Cartesian;

        private SerializedObject _saveData;
        private SerializedProperty saveableGridType;
        private SerializedProperty saveableGridSize;
        private SerializedProperty saveableAngularDivisions;
        private const float Tau = 6.28318530718f; // PI * 2 


        #region Unity Events

        private void OnEnable()
        {
            _saveData = new SerializedObject(this);
            saveableGridSize = _saveData.FindProperty("gridSize");
            saveableGridType = _saveData.FindProperty("gridType");
            saveableAngularDivisions = _saveData.FindProperty("angularDivisions");

            //  Load
            gridSize = EditorPrefs.GetFloat("SNAPPER_TOOL_GridSize", 1f);
            gridType = (GridType) EditorPrefs.GetInt("SNAPPER_TOOL_GridType", 0);
            angularDivisions = EditorPrefs.GetInt("SNAPPER_TOOL_AngularDivisions", 24);

            Selection.selectionChanged += Repaint;
            SceneView.duringSceneGui += DuringSceneGUI;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= Repaint;
            SceneView.duringSceneGui -= DuringSceneGUI;

            //  Save
            EditorPrefs.SetFloat("SNAPPER_TOOL_GridSize", gridSize);
            EditorPrefs.SetInt("SNAPPER_TOOL_GridType", (int) gridType);
            EditorPrefs.SetInt("SNAPPER_TOOL_AngularDivisions", angularDivisions);
        }

        private void OnGUI()
        {
            _saveData.Update();

            EditorGUILayout.PropertyField(saveableGridType);
            EditorGUILayout.PropertyField(saveableGridSize);
            if (gridType == GridType.Polar)
            {
                EditorGUILayout.PropertyField(saveableAngularDivisions);
                saveableAngularDivisions.intValue = Mathf.Max(4, saveableAngularDivisions.intValue);
            }

            if (_saveData.ApplyModifiedProperties()) SceneView.RepaintAll();

            using (new EditorGUI.DisabledScope(Selection.gameObjects.Length == 0))
            {
                if (GUILayout.Button("Snap Selection"))
                    SnapSelection();
            }
        }

        #endregion

        private void DuringSceneGUI(SceneView sceneView)
        {
            if (Event.current.type != EventType.Repaint) return;
            Handles.zTest = CompareFunction.LessEqual;
            const float drawBounds = 16;

            if (gridType == GridType.Cartesian)
                DrawCartesianGrid(drawBounds);
            else DrawPolarGrid(drawBounds);

            sceneView.Repaint();
        }

        private void DrawCartesianGrid(float gridDrawExtent)
        {
            int lineCount = Mathf.RoundToInt((gridDrawExtent * 2) / gridSize);
            if (lineCount % 2 == 0) lineCount++;
            int halfLineCount = lineCount / 2;

            for (int i = 0; i < lineCount; i++)
            {
                int intOffset = i - halfLineCount;
                float xCoord = intOffset * gridSize;
                float zCoord0 = halfLineCount * gridSize;
                float zCoord1 = -halfLineCount * gridSize;
                var p0 = new Vector3(xCoord, 0f, zCoord0);
                var p1 = new Vector3(xCoord, 0f, zCoord1);

                Handles.DrawAAPolyLine(p0, p1);

                p0 = new Vector3(zCoord0, 0f, xCoord);
                p1 = new Vector3(zCoord1, 0f, xCoord);

                Handles.DrawAAPolyLine(p0, p1);
            }
        }

        private void DrawPolarGrid(float gridDrawExtent)
        {
            int ringCount = Mathf.RoundToInt(gridDrawExtent / gridSize);
            float maxRadius = (ringCount - 1) * gridSize;

            //  Radial Grid - rings
            for (int i = 1; i < ringCount; i++)
            {
                Handles.DrawWireDisc(Vector3.zero, Vector3.up, i * gridSize);
            }

            //  Angular Grid - lines
            for (int i = 0; i < angularDivisions; i++)
            {
                float t = i / (float) angularDivisions;
                float angRad = t * Tau;
                float x = Mathf.Cos(angRad);
                float y = Mathf.Sin(angRad);
                Vector3 dir = new Vector3(x, 0f, y) * maxRadius;
                Handles.DrawAAPolyLine(Vector3.zero, dir);
            }
        }

        private void SnapSelection()
        {
            foreach (var gameObject in Selection.gameObjects)
            {
                Undo.RecordObject(gameObject.transform, "Snap Objects");
                gameObject.transform.position = GetSnappedPosition(gameObject.transform.position);
            }
        }

        private Vector3 GetSnappedPosition(Vector3 original)
        {
            if (gridType == GridType.Cartesian)
                return original.SnapTo(gridSize);

            if (gridType == GridType.Polar)
            {
                //  Polar Coords
                var vec = new Vector2(original.x, original.z);

                //  Calculate Snap Distance
                float dist = vec.magnitude;
                float snapDistance = dist.Round(gridSize);

                //  Calculate Angle from Direction
                float angRad = Mathf.Atan2(vec.y, vec.x); //  returns 0 - TAU
                float angTurns = angRad / Tau; //  returns 0 - 1

                //  Calculate Snap Angle
                float angTurnSnapped = angTurns.Round(1f / angularDivisions);
                float angRadSnapped = angTurnSnapped * Tau;

                //  Calculate Snapped Direction from Angle
                var snappedDir = new Vector2(Mathf.Cos(angRadSnapped), Mathf.Sin(angRadSnapped));
                var snappedVector = snappedDir * snapDistance;

                //  Convert Polar Coords back to Cartesian to apply
                return new Vector3(snappedVector.x, original.y, snappedVector.y);
            }

            return default;
        }
    }
}
#endif