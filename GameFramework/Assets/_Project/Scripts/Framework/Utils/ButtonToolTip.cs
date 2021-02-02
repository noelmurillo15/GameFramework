/*
 * ButtonToolTip - Handles viewing a tooltip for buttons that are disabled in-game
 * Created by : Allan N. Murillo
 * Last Edited : 2/17/2020
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ANM.Ui
{
    public class ButtonToolTip : MonoBehaviour
    {
        [SerializeField] private Button button = null;
        [SerializeField] private TMP_Text tooltipText = null;
        [SerializeField] private string tooltipMsg = string.Empty;
        [SerializeField] private GameObject tooltipPanel = null;


        private void Awake()
        {
            button = GetComponent<Button>();
            tooltipText.text = tooltipMsg;
        }

        private void Update()
        {
            tooltipPanel.SetActive(!button.interactable);
        }
    }
}
