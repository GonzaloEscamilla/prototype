using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace _Project.Scripts.GameServices
{
    [CreateAssetMenu(menuName = "ND/GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings: ScriptableObject
    {
        [Title("General")]
        [SerializeField] 
        private int levelSceneIndex;

        [Title("Environment & Background")] 
        [SerializeField]
        private float initialBaseBackgroundSpeed;
        
        [SerializeField]
        private float backgroundSpeed;

        [SerializeField]
        private float treeLayerSpeed;
        
        [SerializeField]
        private float buildingsLayerSpeed;
        
        [Title("Character")] 
        [SerializeField]
        private int characterInitialHealth;
        
        [SerializeField]
        private float characterBaseSpeed;
        
        [SerializeField]
        private float characterJumpForce;
        
        [SerializeField]
        private float characterJumpDuration;

        [SerializeField] 
        private float characterJumpForceMedium;
        
        [SerializeField] 
        private float characterJumpDurationMedium;
        
        [SerializeField] 
        private float characterJumpForceHigh;
        
        [SerializeField] 
        private float characterJumpDurationHigh;
        
        [SerializeField]
        private Ease characterJumpEaseType;
        
        [SerializeField]
        private float characterShadowMinSize;
        
        [SerializeField]
        private Ease characterShadowOutEaseType;
        
        [SerializeField]
        private Ease characterShadowInEaseType;
        
        public int LevelSceneIndex => levelSceneIndex;
        public float InitialBaseBackgroundSpeed => initialBaseBackgroundSpeed;
        public float BackgroundSpeed => backgroundSpeed;
        public float TreeLayerSpeed => treeLayerSpeed;
        public float BuildingsLayerSpeed => buildingsLayerSpeed;
        public int CharacterInitialHealth => characterInitialHealth; 
        public float CharacterBaseSpeed => characterBaseSpeed;
        public float CharacterJumpForce => characterJumpForce;
        public float CharacterJumpDuration => characterJumpDuration;
        public Ease CharacterJumpEaseType => characterJumpEaseType;
        public float CharacterShadowMinSize => characterShadowMinSize;
        public Ease CharacterShadowOutEaseType => characterShadowOutEaseType;
        public Ease CharacterShadowInEaseType => characterShadowInEaseType;
        public float CharacterJumpForceMedium => characterJumpForceMedium;
        public float CharacterJumpDurationMedium => characterJumpDurationMedium;
        public float CharacterJumpForceHigh => characterJumpForceHigh;
        public float CharacterJumpDurationHigh => characterJumpDurationHigh;
    }
}