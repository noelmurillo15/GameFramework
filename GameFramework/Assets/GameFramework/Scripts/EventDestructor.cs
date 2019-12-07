/*
    *   EventDestructor - Used by GameEventListeners to destroy root gameobjects upon Application Quit
    *   Created by : Allan N. Murillo
 */
using UnityEngine;


namespace GameFramework
{
    public class EventDestructor : MonoBehaviour
    {
        public void DestroyThis()
        {
            Destroy(this.gameObject);
        }
    }
}
