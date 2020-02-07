/*
 * ApplyApplicationName - Used to apply the Game Name to a TMP_Text component
 * Created by : Allan N. Murillo
 * Last Edited : 2/7/2020
 */

using TMPro;
using UnityEngine;

namespace ANM.Framework
{
    [RequireComponent(typeof(TMP_Text))]
    public class ApplyApplicationName : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TMP_Text>().text = Application.productName;
        }
    }
}
