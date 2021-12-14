#if UNITY_EDITOR
/*
 * ScatterPrefabTool - 
 * Created by : Allan N. Murillo
 * Last Edited : 9/23/2021
 */

using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using ANM.Framework.Extensions;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace ANM.EditorUtils
{
    public class ScatterPrefabTool : EditorWindow
    {
        [MenuItem("Tools/ANM/Scatter Prefab Tool")]
        public static void OpenEditorWindow() => GetWindow<ScatterPrefabTool>("Scatter Tool");

        #region Class Members

        public float radius = 2f;
        public int spawnCount = 4;
        public bool useCustomMesh;
        public string searchFolder;
        public GameObject spawnPrefab;

        private SerializedObject _saveData;
        private SerializedProperty saveableRadius;
        private SerializedProperty saveableSpawnCount;
        private SerializedProperty saveableSpawnPrefab;
        private SerializedProperty saveablesearchFolder;
        private SerializedProperty saveableUseCustomMesh;
        private const float Tau = 6.28318530718f; // PI * 2

        public class SpawnPoint
        {
            public PreviewData previewData;
            public Vector3 position;
            public Quaternion rotation;
            public bool isValid;

            public Vector3 Up => rotation * Vector3.up;

            public SpawnPoint(Vector3 position, Quaternion rotation, PreviewData data)
            {
                this.position = position;
                this.rotation = rotation;
                previewData = data;
                
                //  Check validity
                
            }
        }

        public struct PreviewData
        {
            public Vector2 PointInDisc;
            public float RandAngle;

            public void RandomizeData()
            {
                PointInDisc = Random.insideUnitCircle;
                RandAngle = Random.value * 360;
            }
        }

        private PreviewData[] _spawnPreviewData;
        private GameObject[] _prefabs;

        #endregion


        #region Unity Events

        private void OnEnable()
        {
            _saveData = new SerializedObject(this);
            saveableRadius = _saveData.FindProperty("radius");
            saveableSpawnCount = _saveData.FindProperty("spawnCount");
            saveableSpawnPrefab = _saveData.FindProperty("spawnPrefab");
            saveablesearchFolder = _saveData.FindProperty("searchFolder");
            saveableUseCustomMesh = _saveData.FindProperty("useCustomMesh");
            GenerateRandomPoints();

            SceneView.duringSceneGui += DuringSceneGUI;
            if (searchFolder.Length == 0)
            {
                Debug.LogError("[ScatterPrefabTool]: set a folder path for prefabs you want to spawn");
                return;
            }

            LoadPrefabs();
        }

        private void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;

        private void OnGUI()
        {
            _saveData.Update();
            EditorGUILayout.PropertyField(saveableRadius);
            saveableRadius.floatValue = saveableRadius.floatValue.AtLeast(1f);
            EditorGUILayout.PropertyField(saveableSpawnCount);
            saveableSpawnCount.intValue = saveableSpawnCount.intValue.AtLeast(1);

            EditorGUILayout.PropertyField(saveableSpawnPrefab);
            EditorGUILayout.PropertyField(saveablesearchFolder);
            EditorGUILayout.PropertyField(saveableUseCustomMesh);

            if (_saveData.ApplyModifiedProperties())
            {
                GenerateRandomPoints();
                SceneView.RepaintAll();
            }

            if (Event.current.type != EventType.MouseDown || Event.current.button != 0) return;
            GUI.FocusControl(null);
            Repaint();
        }

        #endregion

        #region Private Methods
        
        private void LoadPrefabs()
        {
            var guids = AssetDatabase.FindAssets("t:prefab", new[] {searchFolder});
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath);
            _prefabs = paths.Select(AssetDatabase.LoadAssetAtPath<GameObject>).ToArray();
            if (spawnPrefab == null && _prefabs[0] != null) spawnPrefab = _prefabs[0];
            foreach (var path in paths) Debug.Log("[ScatterPrefabTool]: " + path);
        }
        
        private void GenerateRandomPoints()
        {
            _spawnPreviewData = new PreviewData[spawnCount];
            for (int i = 0; i < spawnCount; i++) _spawnPreviewData[i].RandomizeData();
        }

        private void DuringSceneGUI(SceneView sceneView)
        {
            HandleGui();
            HandleScene(sceneView);
        }

        private void HandleGui()
        {
            Handles.BeginGUI();

            var rect = new Rect(8, 8, 64, 64);
            foreach (var prefab in _prefabs)
            {
                if (GUI.Button(rect, AssetPreview.GetAssetPreview(prefab)))
                {
                    spawnPrefab = prefab;
                    Repaint();
                }

                rect.y += rect.height + 2;
            }

            Handles.EndGUI();
        }

        private void HandleScene(SceneView sceneView)
        {
            Handles.zTest = CompareFunction.LessEqual;

            if (Event.current.type == EventType.MouseMove)
                sceneView.Repaint();

            //  Modify radius
            bool altHeld = (Event.current.modifiers & EventModifiers.Alt) != 0;
            if (Event.current.type == EventType.ScrollWheel && altHeld)
            {
                float scrollDir = Mathf.Sign(Event.current.delta.y);
                _saveData.Update();
                saveableRadius.floatValue *= 1f + scrollDir * 0.025f;
                _saveData.ApplyModifiedProperties();
                Repaint();
                Event.current.Use();
            }

            var camTf = sceneView.camera.transform;
            if (TryRaycastFromCamera(camTf.up, out Matrix4x4 tangentToWorld))
            {
                var previewPoses = GetSpawnPoses(tangentToWorld);

                if (Event.current.type == EventType.Repaint)
                {
                    DrawRadiusBorder(tangentToWorld);
                    HandleSpawnPreviews(previewPoses, sceneView.camera);
                }

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
                    TrySpawnPrefabs(previewPoses);
            }
        }

        private void DrawRadiusBorder(Matrix4x4 localToWorldMtx)
        {
            //  Draw radius adapted to terrain
            const int circleDetail = 64;
            Handles.color = Color.white;
            var ringPts = new Vector3[circleDetail];
            for (int i = 0; i < circleDetail; i++)
            {
                var t = i / ((float) circleDetail - 1);
                float angRad = t * Tau;
                var dir = new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));
                var ptRay = GetCircleRay(localToWorldMtx, dir);
                if (Physics.Raycast(ptRay, out var circleHit))
                    ringPts[i] = circleHit.point;
                else ringPts[i] = ptRay.origin;
            }

            Handles.DrawAAPolyLine(ringPts);
        }

        private void HandleSpawnPreviews(List<Pose> previewPoses, Camera cam)
        {
            if (spawnPrefab == null) useCustomMesh = false;

            foreach (var pose in previewPoses)
            {
                if (useCustomMesh)
                {
                    var poseToWorld = Matrix4x4.TRS(pose.position, pose.rotation, Vector3.one);
                    DrawPrefab(spawnPrefab, poseToWorld, cam);
                }
                else
                {
                    DrawSphere(pose.position);
                    Handles.DrawAAPolyLine(pose.position, pose.rotation * Vector3.up);
                }
            }
        }

        private void TrySpawnPrefabs(List<Pose> poses)
        {
            if (spawnPrefab == null) return;
            foreach (var pose in poses)
            {
                var spawnRef = (GameObject) PrefabUtility.InstantiatePrefab(spawnPrefab);
                Undo.RegisterCreatedObjectUndo(spawnRef, "Spawn Objs");
                spawnRef.transform.position = pose.position;
                spawnRef.transform.rotation = pose.rotation;
            }

            GenerateRandomPoints();
        }

        private List<Pose> GetSpawnPoses(Matrix4x4 tangentToWorldMtx)
        {
            var poseResults = new List<Pose>();
            foreach (var data in _spawnPreviewData)
            {
                var ptRay = GetCircleRay(tangentToWorldMtx, data.PointInDisc);

                if (Physics.Raycast(ptRay, out var ptHit))
                {
                    Quaternion randRot = Quaternion.Euler(0f, 0f, data.RandAngle);
                    Quaternion rot = Quaternion.LookRotation(ptHit.normal) * (randRot * Quaternion.Euler(90f, 0f, 0f));
                    var pose = new Pose(ptHit.point, rot);
                    poseResults.Add(pose);
                }
            }

            return poseResults;
        }

        private Ray GetCircleRay(Matrix4x4 tangentToWorldMtx, Vector2 pointInCircle)
        {
            Vector3 normal = tangentToWorldMtx.MultiplyVector(Vector3.forward);
            Vector3 origin = tangentToWorldMtx.MultiplyPoint3x4(pointInCircle * radius);
            origin += normal * 2;
            Vector3 dir = -normal;
            return new Ray(origin, dir);
        }

        private static bool TryRaycastFromCamera(Vector2 cameraUp, out Matrix4x4 tangentToWorldMtx)
        {
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out var mouseHit))
            {
                //  Calculate Tangent Space
                var hitNormal = mouseHit.normal;
                var hitTangent = Vector3.Cross(hitNormal, cameraUp).normalized;
                var hitBiTangent = Vector3.Cross(hitNormal, hitTangent);

                //  Draw tangent space representations on raycast hit
                Handles.color = Color.red;
                Handles.DrawAAPolyLine(6, mouseHit.point, mouseHit.point + hitTangent);
                Handles.color = Color.green;
                Handles.DrawAAPolyLine(6, mouseHit.point, mouseHit.point + hitBiTangent);
                Handles.color = Color.blue;
                Handles.DrawAAPolyLine(6, mouseHit.point, mouseHit.point + hitNormal);

                tangentToWorldMtx = Matrix4x4.TRS(mouseHit.point,
                    Quaternion.LookRotation(hitNormal, hitBiTangent), Vector3.one);

                return true;
            }

            tangentToWorldMtx = default;
            return false;
        }

        private static void DrawPrefab(GameObject prefab, Matrix4x4 poseToWorldMtx, Camera cam)
        {
            var childMeshFilters = prefab.GetComponentsInChildren<MeshFilter>();
            foreach (var filter in childMeshFilters)
            {
                Matrix4x4 childToPoseMtx = filter.transform.localToWorldMatrix;
                Matrix4x4 childToWorldMtx = poseToWorldMtx * childToPoseMtx;
                Mesh mesh = filter.sharedMesh;
                Material mat = filter.GetComponent<MeshRenderer>().sharedMaterial;
                mat.SetPass(0);
                Graphics.DrawMesh(mesh, childToWorldMtx, mat, 0, cam);
            }
        }

        private static void DrawSphere(Vector3 worldPos) => Handles
            .SphereHandleCap(-1, worldPos, Quaternion.identity, 0.1f, EventType.Repaint);

        #endregion
    }
}
#endif