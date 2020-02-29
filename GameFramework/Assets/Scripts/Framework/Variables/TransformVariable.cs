/*
 * TransformVariable SO - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/28/2020
 */

using UnityEngine;

namespace ANM.Framework.Variables
{
    [CreateAssetMenu(menuName = "Variables/Transform")]
    public class TransformVariable : ScriptableObject
    {
        public Transform value;

        public void Set(Transform transform)
        {
            value = transform;
        }
    }
}