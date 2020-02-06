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
