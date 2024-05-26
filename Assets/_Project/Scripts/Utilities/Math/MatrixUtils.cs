using UnityEngine;

namespace _Project.Scripts.Utilities.Math
{
    public static class MatrixUtils
    {
        public static Vector2Int Up(int reach)
        {
            return new Vector2Int(1, 0) * reach;
        }
        public static Vector2Int Down(int reach)
        {
            return new Vector2Int(-1, 0) * reach;
        }
        public static Vector2Int Left(int reach)
        {
            return new Vector2Int(0, -1) * reach;
        }
        public static Vector2Int Right(int reach)
        {
            return new Vector2Int(0, 1) * reach;
        }
    }
}