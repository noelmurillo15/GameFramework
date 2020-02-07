/*
 * GameEventListeners - Listens for Game Events raised and responds
 * Created by : Allan N. Murillo
 * Last Edited : 2/7/2020
 */

using UnityEngine;
using UnityEngine.Events;

namespace ANM.Framework
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
