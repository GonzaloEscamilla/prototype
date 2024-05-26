using System;

namespace _Project.Scripts.Utilities.Math
{
    public static class Range
    {
        /// <summary>
        /// Evaluates if the value is inside a defined value range with exclusive bounds.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rangeMin"></param>
        /// <param name="rangeMax"></param>
        /// <returns></returns>
        public static bool IsInsideRangeExclusive(float value, float rangeMin, float rangeMax)
        {
            return value > rangeMin && value < rangeMax;
        }
        
        /// <summary>
        /// Given an array of thresholds and a current value, this method returns the index of the range that the value belongs to. 
        /// The array can be ordered or not, if the isOrdered parameter is passed as false the method will order the array before performing the calculations
        /// </summary>
        /// <param name="thresholds">Array of floats representing the thresholds</param>
        /// <param name="currentValue">A float value between 0 and 1 to check which range it belongs</param>
        /// <param name="isOrdered">Boolean parameter indicating whether the array is already ordered</param>
        /// <returns>An integer representing the index of the range that the current value belongs to</returns>
        public static int GetRangeIndex(float[] thresholds, float currentValue, bool isOrdered = true)
        {
            if(!isOrdered)
                Array.Sort(thresholds);

            for (int i = 0; i < thresholds.Length; i++)
            {
                if (currentValue <= thresholds[i])
                    return i;
            }
            
            return thresholds.Length;
        }
    }
}