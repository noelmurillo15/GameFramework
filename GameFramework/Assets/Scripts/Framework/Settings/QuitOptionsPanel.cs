/*
 * QuitOptionsPanel - Backbone of the game application
 * Contains data that needs to persist and be accessed from anywhere
 * Created by : Allan N. Murillo
 * Last Edited : 2/24/2020
 */

using UnityEngine;

namespace ANM.Framework.Settings
{
    public class QuitOptionsPanel : MonoBehaviour
    {
        [SerializeField] private Animator quitOptionsPanelAnimator = null;

        public void TurnOffPanel()
        {
            if (quitOptionsPanelAnimator == null) return;
            if (quitOptionsPanelAnimator.transform.GetChild(0).gameObject.activeSelf)
                quitOptionsPanelAnimator.Play("QuitPanelOut");
        }

        public void TurnOnPanel()
        {
            if (quitOptionsPanelAnimator == null) return;
            if (!quitOptionsPanelAnimator.transform.GetChild(0).gameObject.activeSelf)
                quitOptionsPanelAnimator.Play("QuitPanelIn");
        }
    }
}