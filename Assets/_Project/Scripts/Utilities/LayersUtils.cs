using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public static class LayersUtils
    {
        public static readonly int DefaultLayer = LayerMask.NameToLayer("Default");
        public static readonly int TransparentFXLayer = LayerMask.NameToLayer("TransparentFX");
        public static readonly int IgnoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        public static readonly int WaterLayer = LayerMask.NameToLayer("Water");
        public static readonly int UILayer = LayerMask.NameToLayer("UI");
        public static readonly int BallLayer = LayerMask.NameToLayer("Ball");
        public static readonly int NonCollisionLayer = LayerMask.NameToLayer("NonCollision");
        public static readonly int TeamALayer = LayerMask.NameToLayer("TeamA");
        public static readonly int TeamBLayer = LayerMask.NameToLayer("TeamB");
        public static readonly int CatchedBallLayer = LayerMask.NameToLayer("CatchedBall");
        public static readonly int PlayerWallLayer = LayerMask.NameToLayer("PlayerWall");
        public static readonly int StaticEnvironmentLayer = LayerMask.NameToLayer("StaticEnvironment");
        public static readonly int InvulnerableLayer = LayerMask.NameToLayer("Invulnerable");
        public static readonly int CustomizableLayer = LayerMask.NameToLayer("Customizable");

        public static LayerMask GetLayerMask(string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            return 1 << layer;
        }

        public static bool IsSameLayer(GameObject obj1, GameObject obj2)
        {
            return obj1.layer == obj2.layer;
        }

        public static LayerMask GetCommonLayer(LayerMask mask1, LayerMask mask2)
        {
            int layer = mask1 & mask2;
            return 1 << layer;
        }
    }
}
