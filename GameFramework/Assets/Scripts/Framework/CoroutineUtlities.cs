using UnityEngine;
using System.Collections;

namespace ANM.Framework
{
    public static class CoroutineUtilities
    {
        public static IEnumerator WaitForRealTime(float delay)
        {
            while (true)
            {
                var endTime = Time.realtimeSinceStartup + delay;
                while (Time.realtimeSinceStartup < endTime) yield return 0;
                break;
            }
        }
    }
}
