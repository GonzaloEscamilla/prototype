using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core
{
    [CreateAssetMenu(menuName = "Prototype/Create Character Settings", fileName = "CharacterSettings", order = 0)]
    public class CharacterSettings : ScriptableObject
    {
        [Header("Movement Settings")]
        public float rotationFactorPerFrame;
        public float speed;
        public float runMultiplier;
        public float runEnergyCost;
    
        [Header("Dash Settings")] 
        public bool usesDashPoints;
        public int dashPoints;
        public float dashEnergyCost;
        [OnValueChanged("UpdateDashDistanceValue")]public float dashDuration;
        [OnValueChanged("UpdateDashDistanceValue")]public float dashSpeed;
        [ReadOnly] public float dashDistance;
        public float dashCooldown;
        [SerializeField] [Range(0.0f, 1.0f)] public float bufferedDashPercentage;
        [MinMaxSlider(0f,1f)][SerializeField] public Vector2 dashInvulnerabilityRange;

        [Header("Detection")] 
        public LayerMask interactionLayerMask;
        
        private void UpdateDashDistanceValue()
        {
            dashDistance = dashSpeed * dashDuration;
        }
    }
}