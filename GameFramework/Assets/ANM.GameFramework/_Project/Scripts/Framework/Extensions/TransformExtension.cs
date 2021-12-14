/*
 * TransformExtension - Useful extended functionality for Unity Transforms
 * Created by : Allan N. Murillo
 * Last Edited : 2/17/2020
 */

using UnityEngine;

namespace ANM.Framework.Extensions
{
    public static class TransformExtension
    {
        public static Vector3 DirectionTo(this Transform source, Transform other)
        {
            return source.position.DirectionTo(other.position);
        }

        public static bool IsFacingTowards(this Transform source, Transform target)
        {
            return source.position.IsFacingTowards(target.position);
        }
    }
}
