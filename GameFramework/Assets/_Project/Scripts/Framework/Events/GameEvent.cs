/*
 * GameEvent - An event that is saved as a Scriptable Object (can be accessed from any scene)
 * Use a GameEventListener.cs to call a certain function when a GameEvent is Raised()
 * Created by : Allan N. Murillo
 * Last Edited : 4/8/2021
 */

using UnityEngine;
using System.Collections.Generic;

namespace ANM.Framework.Events
{
    [CreateAssetMenu(menuName = "GameEvent", fileName = "New Game Event")]
    public class GameEvent : ScriptableObject
    {
        //public string eventName = string.Empty;
        private readonly HashSet<GameEventListeners> _listeners = new HashSet<GameEventListeners>();


        public void Raise()
        {
            foreach (var listener in _listeners)
            {
                listener.OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListeners listener) => _listeners.Add(listener);

        public void UnregisterListener(GameEventListeners listener) => _listeners.Remove(listener);

        private void UnregisterAllListeners()
        {
            foreach (var listener in _listeners) _listeners.Remove(listener);
        }

        private void OnDisable() => UnregisterAllListeners();
    }
}
