using TMPro;
using UnityEngine;

namespace GameFramework.Core
{
    public class ApplicationNameTMP : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TMP_Text>().text = Application.productName;
        }
    }
}
