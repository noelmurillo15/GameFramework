/*
 * GameEventListeners - Listens for Game Events raised and responds via a UnityEvent
 * Created by : Allan N. Murillo
 * Last Edited : 2/17/2020
 */

using UnityEngine;
using UnityEngine.Events;

namespace ANM.Framework.Events
{
    public class GameEventListeners : MonoBehaviour
    {
        public GameEvent @event;
        public UnityEvent response;


        private void OnEnable()
        {
            @event.RegisterListener(this);
        }

        private void OnDisable()
        {
            @event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}
