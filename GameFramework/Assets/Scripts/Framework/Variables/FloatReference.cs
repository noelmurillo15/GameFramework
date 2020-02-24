/*
 * FloatReference - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/17/2020
 */

namespace ANM.Framework.Variables
{
    [System.Serializable]
    public class FloatReference
    {
        public bool useConstant = true;
        public float constantValue;
        public FloatVariable variable;

        public float Value => useConstant ? constantValue : variable.value;
    }
}