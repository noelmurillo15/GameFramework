using UnityEngine;
using System.Collections.Generic;


namespace GameFramework.Events
{
    [CreateAssetMenu(menuName = "GameEvent")]
    public class GameEvent : ScriptableObject
    {
        public string eventName = string.Empty;
        List<GameEventListeners> listeners = new List<GameEventListeners>();


        public void Raise()
        {
            // Debug.Log(eventName + " Event has been Raised! # of listeners : " + listeners.Count.ToString());
            for (int x = 0; x < listeners.Count; x++)
            {
                listeners[x].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListeners listener)
        {
            if (listeners.Contains(listener))
            {
                Debug.Log(eventName + " Event already contains this Listener!");
            }
            else
            {
                listeners.Add(listener);
                // Debug.Log(eventName + " Event added a Listener! : " + listeners.Count.ToString());
            }
        }

        public void UnregisterListener(GameEventListeners listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
                // Debug.Log(eventName + " Event removed a Listener! : " + listeners.Count.ToString());
            }
            else
            {
                Debug.Log(eventName + " Event is not contained in this Listener!");
            }
        }
        public void UnregisterAllListeners()
        {
            Debug.Log(eventName + " Unregistering All Listeners!");
            if (listeners.Count > 0)
            {
                for (int x = listeners.Count - 1; x >= 0; x--)
                {
                    Debug.Log(eventName + " Game Event listeners is being unregistered @ index : " + x);
                    listeners.RemoveAt(x);
                }
            }
        }
    }
}
