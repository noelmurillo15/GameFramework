/*
 * EventDestructor - Used by GameEventListeners to destroy root gameobjects upon Application Quit
 * Created by : Allan N. Murillo
 */

using UnityEngine;

namespace ANM.Framework
{
    public class EventDestructor : MonoBehaviour
    {
        public void OnApplicationQuitEvent()
        {    //    Handled by onApplicationQuit ScriptableObject
            Destroy(gameObject);
        }
    }
}
