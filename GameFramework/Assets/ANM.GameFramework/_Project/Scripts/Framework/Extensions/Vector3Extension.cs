/*
 * Vector3Extension - Useful extended functionality for Unity Vector3
 * Created by : Allan N. Murillo
 * Last Edited : 9/22/2021
 */

using UnityEngine;

namespace ANM.Framework.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
        }

        public static Vector3 Flat(this Vector3 original)
        {
            return new Vector3(original.x, 0f, original.z);
        }

        public static bool IsFacingTowards(this Vector3 source, Vector3 target)
        {
            return Vector3.Dot(source, target) >= 0.96f;
        }

        public static Vector3 DirectionTo(this Vector3 source, Vector3 destination)
        {
            return Vector3.Normalize(destination - source);
        }

        public static Vector3 Round(this Vector3 source)
        {
            source.x = Mathf.Round(source.x);
            source.y = Mathf.Round(source.y);
            source.z = Mathf.Round(source.z);
            return source;
        }

        public static Vector3 SnapTo(this Vector3 source, float step)
        {
            return (source / step).Round() * step;
        }
    }
}
