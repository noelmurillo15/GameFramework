/*
    *   GameEvent - Manages GameSettings UI & Functionality
    *   Created by : Allan N. Murillo
 */
using UnityEngine;
using System.Collections.Generic;

namespace GameFramework.Core
{
    [CreateAssetMenu(menuName = "GameEvent")]
    public class GameEvent : ScriptableObject
    {
        public string eventName = string.Empty;
        private readonly List<GameEventListeners> _listeners = new List<GameEventListeners>();


        public void Raise()
        {
            for (int x = 0; x < _listeners.Count; x++)
            {
                _listeners[x].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListeners listener)
        {
            if (_listeners.Contains(listener))
            {
                Debug.Log(eventName + " Event already contains this Listener!");
            }
            else
            {
                _listeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListeners listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
            else
            {
                Debug.Log(eventName + " Event is not contained in this Listener!");
            }
        }
        public void UnregisterAllListeners()
        {
            if (_listeners.Count <= 0) return;
            for (int x = _listeners.Count - 1; x >= 0; x--)
            {
                _listeners.RemoveAt(x);
            }
        }
    }
}
