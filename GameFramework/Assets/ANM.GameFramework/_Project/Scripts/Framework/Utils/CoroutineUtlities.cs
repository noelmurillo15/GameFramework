/*
 * CoroutineUtilities - Used for co-routines to be able to run when TimeScale = 0
 */

using UnityEngine;
using System.Collections;

namespace ANM.Framework.Utils
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
