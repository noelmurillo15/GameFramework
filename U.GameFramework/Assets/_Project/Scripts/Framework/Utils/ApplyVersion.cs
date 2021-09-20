/*
 * ApplyVersion - Used to apply the current version to a TMP_Text component
 * Created by : Allan N. Murillo
 * Last Edited : 7/4/2021
 */

using TMPro;
using UnityEngine;

namespace ANM.Framework.Utils
{
    [RequireComponent(typeof(TMP_Text))]
    public class ApplyVersion : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TMP_Text>().text = "v" + Application.version;
        }
    }
}
