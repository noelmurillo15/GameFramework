/*
 * EventExtension - Useful extended functionality for Unity Events
 * Created by : Allan N. Murillo
 * Last Edited : 2/17/2020
 */

namespace ANM.Framework.Extensions
{
    public static class EventExtension 
    {
        public static void MuteEventListener(UnityEngine.Events.UnityEventBase eventBase)
        {
            var count = eventBase.GetPersistentEventCount();
            for (var x = 0; x < count; x++)
            {
                eventBase.SetPersistentListenerState(x, UnityEngine.Events.UnityEventCallState.Off);
            }
        }

        public static void UnMuteEventListener(UnityEngine.Events.UnityEventBase eventBase)
        {
            var count = eventBase.GetPersistentEventCount();
            for (var x = 0; x < count; x++)
            {
                eventBase.SetPersistentListenerState(x, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            }
        }
    }
}
