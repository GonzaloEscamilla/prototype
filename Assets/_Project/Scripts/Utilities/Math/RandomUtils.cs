using Sirenix.Utilities;
using UnityEngine;

namespace _Project.Scripts.Utilities.Math
{
    public static class RandomUtils
    {
        public static bool Chance(float probabilityPercentage)
        {
            if (probabilityPercentage is < 1 and > 0)
                probabilityPercentage *= 100;
            
            float chanceValue = Random.Range(0f, 100f);
            return chanceValue <= probabilityPercentage;
        }
        
        public static bool Chance(float probabilityPercentage, float minThreshold, float maxThreshold)
        {
            float chanceValue = Random.Range(minThreshold, maxThreshold);
            return chanceValue <= probabilityPercentage;
        }

        public static float GetChance(float probabilityPercentage, float maxThreshold)
        {
            return probabilityPercentage/maxThreshold;
        }
        
        public static int FromWeightedRangeToRandomIndex(float[] weightedRange)
        {
            weightedRange.Sort();

            var randomValue = Random.Range(0f, 1f);

            for (int i = 0; i < weightedRange.Length; i++)
            {
                if (randomValue <= weightedRange[i])
                    return i;
            }
            
            return 0;
        }
    }
}