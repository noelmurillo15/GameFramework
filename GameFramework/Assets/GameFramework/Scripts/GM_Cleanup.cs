using UnityEngine;


namespace GameFramework.Managers
{
    public class GM_Cleanup : MonoBehaviour
    {
        void Start()
        {
            if (GameManager.Instance != null)
            {
                Destroy(GameManager.Instance.gameObject);
            }
            Invoke("End", 3f);
        }

        void End()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
