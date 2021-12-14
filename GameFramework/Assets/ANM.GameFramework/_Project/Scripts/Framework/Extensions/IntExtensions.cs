/*
 * IntExtensions - Useful extended functionality for Unity Int
 * Created by : Allan N. Murillo
 * Last Edited : 9/22/2021
 */

using UnityEngine;

namespace ANM.Framework.Extensions
{
    public static class IntExtensions
    {
        public static int AtLeast(this int source, int min) => Mathf.Max(source, min);
    }
}
