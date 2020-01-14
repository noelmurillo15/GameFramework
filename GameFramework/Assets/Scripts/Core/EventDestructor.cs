/*
    *   EventDestructor - Used by GameEventListeners to destroy root gameobjects upon Application Quit
    *   Created by : Allan N. Murillo
 */

using UnityEngine;

namespace GameFramework.Core
{
    public class EventDestructor : MonoBehaviour
    {
        public void OnApplicationQuitEvent()
        {    //    Handled by onApplicationQuit ScriptableObject
            Destroy(gameObject);
        }
    }
}
