/*
 * StringVariable SO - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/28/2020
 */

using UnityEngine;

namespace ANM.Framework.Variables
{
    [CreateAssetMenu(menuName = "Variables/String")]
    public class StringVariable : ScriptableObject
    {
        public string value;
    }
}