using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public static class ArrayUtils
    {
        public static bool ExistsIndexOf<T>(Vector2Int coords, ref T[,] array)
        {
            return array.GetLowerBound(0) <= coords.x && array.GetUpperBound(0) >= coords.x &&
                   (array.GetLowerBound(1) <= coords.y && array.GetUpperBound(1) >= coords.y);
        }

        public static Vector2Int GetReflexElementOf<T>(Vector2Int coords, ref T[,] array)
        {
            int n = array.GetLength(0); // 7
            int m = array.GetLength(1); // 12
            
            int n1 = coords.x;
            int m1 = coords.y;

            int n2 = 0; 
            int m2 = 0;

            if (n % 2 == 0)  // Veo si N es par
                n2 = n - n1 + 1;     // En caso de que lo sea sólo calculo Si es impar, puede que n1 caiga al medio
            else
            {
                if (n1 != (n / 2 + 1f / 2f)) //Verifico que no caiga al medio
                    n2 = n - n1 + 1;
                else //Si cae al medio, no transformo
                    n2 = n1;
            }


            if (m % 2 == 0) //Veo si M es par
                m2 = m - m1 + 1;     //En caso de que lo sea sólo calculo Si es impar, puede que n1 caiga al medio
            else
            {
                if (m1 != (m / 2 + 1f/2f)) //Verifico que no caiga al medio
                    m2 = m - m1 + 1;
                else //Si cae al medio, no transformo
                    m2 = m1;
            }

            n2 -= 2;
            m2 -= 2;
            
            return new Vector2Int(n2, m2);
        }
    }
}