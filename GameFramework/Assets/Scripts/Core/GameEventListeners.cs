using UnityEngine;
using UnityEngine.Events;

namespace GameFramework.Events
{
    public class GameEventListeners : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;


        void OnEnable()
        {
            Event.RegisterListener(this);
        }

        void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}
