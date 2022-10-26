/*
 * SplitScreenController - 
 * Created by : Allan N. Murillo
 * Last Edited : 7/29/2021
 */

using UnityEngine;

namespace ANM.Framework.Utils
{
    [RequireComponent(typeof(Camera))]
    public class SplitScreenController : MonoBehaviour
    {
        [SerializeField] private Camera camera;


        private void OnEnable()
        {
            if (camera == null) camera = GetComponent<Camera>();
        }

        public void SetCameraViewport(int index, SplitScreenType type)
        {
            Enable();
            switch (index)
            {
                case 0:
                    camera.rect = type == SplitScreenType.Horizontal
                        ? new Rect(0f, 0f, 0.5f, 1f)
                        : new Rect(0.5f, 0f, 0.5f, 1f);
                    Debug.Log("[SplitScreenController]: assigning camera 1 viewport");
                    break;
                case 1:
                    camera.rect = type == SplitScreenType.Horizontal
                        ? new Rect(0.5f, 0f, 0.5f, 1f)
                        : new Rect(0f, 0f, 0.5f, 1f);
                    Debug.Log("[SplitScreenController]: assigning camera 2 viewport");
                    break;
                default:
                    Debug.LogError("[SplitScreenController]: index is out of bounds");
                    break;
            }
        }

        public void ResetViewport()
        {
            camera.rect = new Rect(0, 0, 1f, 1f);
        }

        public void Disable()
        {
            camera.enabled = false;
        }

        private void Enable()
        {
            camera.enabled = true;
        }
    }
}
