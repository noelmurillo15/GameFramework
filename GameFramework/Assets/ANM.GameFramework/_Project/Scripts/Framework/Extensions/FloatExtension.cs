/*
 * FloatExtension - Useful extended functionality for Unity Float
 * Created by : Allan N. Murillo
 * Last Edited : 9/22/2021
 */

using UnityEngine;

namespace ANM.Framework.Extensions
{
    public static class FloatExtension
    {
        public static float AtLeast(this float source, float min) => Mathf.Max(source, min);
        public static float Round(this float source, float step) => Mathf.Round(source / step) * step;
    }
}
