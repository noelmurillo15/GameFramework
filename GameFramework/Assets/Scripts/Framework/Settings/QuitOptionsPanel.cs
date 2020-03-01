/*
 * MenuManager - Handles interactions with the Menu Ui
 * Created by : Allan N. Murillo
 * Last Edited : 3/1/2020
 */

using UnityEngine;

namespace ANM.Framework.Settings
{
    public class QuitOptionsPanel : MonoBehaviour, IPanel
    {
        private GameObject _panel;
        private Animator _quitOptionsPanelAnimator;
    
        private void Start()
        {
            _quitOptionsPanelAnimator = GetComponent<Animator>();
            _panel = _quitOptionsPanelAnimator.transform.GetChild(0).gameObject;
        }

        public void TurnOnPanel()
        {
            if (!_panel.activeSelf)
                _quitOptionsPanelAnimator.Play("QuitPanelIn");
        }

        public void TurnOffPanel()
        {
            if (_panel.activeSelf)
                _quitOptionsPanelAnimator.Play("QuitPanelOut");
        }
    }
}
