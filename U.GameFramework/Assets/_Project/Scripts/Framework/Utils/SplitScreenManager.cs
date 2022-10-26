/*
 * SplitScreenManager -
 * Created by : Allan N. Murillo
 * Last Edited : 7/29/2021
 */

using UnityEngine;
using System.Collections.Generic;

namespace ANM.Framework.Utils
{
    public class SplitScreenManager : MonoBehaviour
    {
        [SerializeField] private SplitScreenType splitScreenType;
        private List<SplitScreenController> _controllers;
        private bool _canUseSplitScreen;
        private bool _splitScreenActive;
        private int _count;


        public void Initialize()
        {
            Debug.Log("[SplitScreenManager]: Initialize");
            _count = 0;
            _canUseSplitScreen = false;
            _splitScreenActive = false;

#if NET_STANDARD_2_0
            if (_controllers == null) _controllers = new List<SplitScreenController>();
#else
            _controllers ??= new List<SplitScreenController>();
#endif

            var container = FindObjectsOfType<SplitScreenController>();
            foreach (var controller in container)
            {
                _count++;
                _controllers.Add(controller);
            }

            if (_count <= 0) Debug.LogError("[SplitScreenManager]: no controllers found");
            else if (_count == 1)
            {
                Debug.LogWarning("[SplitScreenManager]: split screen disabled (only 1 controller found)");
            }
            else
            {
                _canUseSplitScreen = true;
                _splitScreenActive = false;
                Debug.Log("[SplitScreenManager]: split screen ready");
            }
        }

        public void EnableSplitScreen(SplitScreenType type = SplitScreenType.Horizontal)
        {
            if (!_canUseSplitScreen) return;
            if (_splitScreenActive) return;
            _splitScreenActive = true;
            splitScreenType = type;
            _controllers[0].SetCameraViewport(0, splitScreenType);
            _controllers[1].SetCameraViewport(1, splitScreenType);
        }

        public void SwapSplitScreen()
        {
            splitScreenType = splitScreenType == SplitScreenType.Horizontal
                ? SplitScreenType.Vertical
                : SplitScreenType.Horizontal;
            _controllers[0].SetCameraViewport(0, splitScreenType);
            _controllers[1].SetCameraViewport(1, splitScreenType);
        }

        public void DisableSplitScreen()
        {
            if (!_splitScreenActive) return;
            var i = 0;
            _splitScreenActive = false;
            foreach (var controller in _controllers)
            {
                i++;
                controller.ResetViewport();
                if (i == 1) return;
                controller.Disable();
            }
        }
    }

    public enum SplitScreenType
    {
        Horizontal,
        Vertical
    }
}
