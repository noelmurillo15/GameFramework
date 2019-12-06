using UnityEngine;


namespace GameFramework.Managers
{
    public class GM_Cleanup : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (GameManager.Instance != null)
            {
                Destroy(GameManager.Instance.gameObject);
            }
        }
    }
}
