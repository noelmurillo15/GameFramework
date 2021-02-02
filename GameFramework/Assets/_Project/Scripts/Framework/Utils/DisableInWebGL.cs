
using UnityEngine;

namespace ANM
{
    public class DisableInWebGL : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_WEBGL && ! UNITY_EDITOR
            gameObject.SetActive(false);
#else
            Destroy(this);
#endif
        }
    }
}
