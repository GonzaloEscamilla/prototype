using UnityEngine;

namespace _Project.Scripts.Utilities
{
    [CreateAssetMenu(menuName = "Utils/DetectorSettings", fileName = "DetectorSettings", order = 0)]
    public class DetectorSettings : ScriptableObject
    {
        [Header("Settings")] 
        public LayerMask detectionMask;
        public float radius;
        public float frameRate = 0.25f;
        public int maxDetectableColliders = 10;
        public bool sortByDistance;
    }
}