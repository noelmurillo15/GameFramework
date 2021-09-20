/*
 * QuitOptionsPanel - Handles Displaying the Quit Ui Panel
 * Created by : Allan N. Murillo
 * Last Edited : 7/10/2020
 */

using UnityEngine;

namespace ANM.Framework.Options
{
    public class QuitOptionsPanel : MonoBehaviour
    {
        private GameObject _panel;


        private void Start()
        {
            _panel = gameObject;
            _panel.SetActive(false);
        }

        public void TurnOnPanel()
        {
            if (!_panel.activeSelf)
                _panel.SetActive(true);
        }

        public void TurnOffPanel()
        {
            if (_panel.activeSelf)
                _panel.SetActive(false);
        }
    }
}
