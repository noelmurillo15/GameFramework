/*
* ApplyProductName - Used to apply the Game Name to a TMP_Text component
* Created by : Allan N. Murillo
*/

using TMPro;
using UnityEngine;

namespace ANM.Framework
{
    [RequireComponent(typeof(TMP_Text))]
    public class ApplyProductName : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TMP_Text>().text = Application.productName;
        }
    }
}
