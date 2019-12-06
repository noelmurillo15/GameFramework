using TMPro;
using UnityEngine;

public class ApplicationNameTMP : MonoBehaviour
{
    void Awake()
    {
        GetComponent<TMP_Text>().text = Application.productName.ToString();
    }
}
