/*
 * GameEvent - Manages GameSettings UI & Functionality
 * Created by : Allan N. Murillo
 */

using UnityEngine;
using System.Collections.Generic;

namespace ANM.Framework
{
    [CreateAssetMenu(menuName = "GameEvent")]
    public class GameEvent : ScriptableObject
    {
        public string eventName = string.Empty;
        private readonly List<GameEventListeners> _listeners = new List<GameEventListeners>();


        public void Raise()
        {
            for (var x = 0; x < _listeners.Count; x++)
            {
                _listeners[x].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListeners listener)
        {
            if (_listeners.Contains(listener))
            {
                Debug.Log(listener.gameObject.name + " is already contained in " + eventName);
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
                Debug.Log(listener.gameObject.name + " has already been removed from " + eventName);
            }
        }
        private void UnregisterAllListeners()
        {
            if (_listeners.Count <= 0) return;
            for (var x = _listeners.Count - 1; x >= 0; x--)
            {
                _listeners.RemoveAt(x);
            }
        }

        private void OnDisable()
        {
            UnregisterAllListeners();
        }
    }
}
