/*
 * RaycastManager -
 * Created by : Allan N. Murillo
 * Last Edited : 7/4/2021
 */

using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace ANM.Framework.Input
{
    public class RaycastManager : MonoBehaviour
    {
        public LayerMask groundMask;

        [Serializable]
        private struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private CursorMapping[] cursorMappings;

        private static RaycastHit[] _nonAllocArray;
        private IRaycastable _lastRaycast;
        private EventSystem _eventSystem;
        private bool _isUiActive;


        #region Unity Funcs

        private void Awake()
        {
            // Debug.Log("[PlayerController]: Awake");
            _lastRaycast = null;
            _nonAllocArray = new RaycastHit[5];
            _eventSystem = EventSystem.current;
        }

        private void Start()
        {
            // Debug.Log("[PlayerController]: Start");
        }

        private void OnEnable()
        {
            //GameManager.GetResources().GetInput().OnClickEvent += OnClickEventCalled;
        }

        private void OnDisable()
        {
            //GameManager.GetResources().GetInput().OnClickEvent -= OnClickEventCalled;
        }

        private void Update()
        {
            _isUiActive = InteractWithUi();
            if (_isUiActive) return;
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        #endregion

        private bool InteractWithUi()
        {
            if (!_eventSystem.IsPointerOverGameObject()) return false; //  refers to UI game object
            SetCursor(CursorType.UI);
            return true;
        }

        private bool InteractWithComponent()
        {
            var hits = RayCastAllSorted();
            foreach (var hit in hits)
            {
                if (hit.transform == null) continue;
                var interfaces = hit.transform.GetComponents<IRaycastable>();
                foreach (var interactable in interfaces)
                {
                    if (!interactable.HandleRayCast()) continue;
                    SetCursor(interactable.GetCursorType());
                    _lastRaycast = interactable;
                    return true;
                }
            }

            if (_lastRaycast == null) return false;
            _lastRaycast.OnRaycastExit();
            _lastRaycast = null;
            return false;
        }

        //  TODO : assign movement here
        private bool InteractWithMovement()
        {
            if (!RaycastNavMesh(out var target)) return false;
            SetCursor(CursorType.Movement);
            //_charMover.StartMoveAction(target);
            return true;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            var hasHit = Physics.Raycast(InputController.GetMouseRay(), out var hit, groundMask);
            if (!hasHit) return false;

            var hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out var navMeshHit, maxNavMeshProjectionDistance,
                NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;
            return true;
        }

        private void SetCursor(CursorType type)
        {
            var mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (var mapping in cursorMappings)
                if (mapping.type == type)
                    return mapping;

            return cursorMappings[0];
        }

        private static IEnumerable<RaycastHit> RayCastAllSorted()
        {
            var size = Physics.RaycastNonAlloc(InputController.GetMouseRay(), _nonAllocArray);

            var distances = new float[size];
            for (var x = 0; x < size; x++) distances[x] = _nonAllocArray[x].distance;
            Array.Sort(distances, _nonAllocArray);

            return _nonAllocArray;
        }
    }
}
