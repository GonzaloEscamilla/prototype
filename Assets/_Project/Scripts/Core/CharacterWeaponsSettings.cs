using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core
{
    [CreateAssetMenu(menuName = "Prototype/Create Character Weapon Settings", fileName = "CharacterWeaponSettings", order = 0)]
    public class CharacterWeaponsSettings : ScriptableObject
    {
        [SerializeField]
        private float damage;
        
        [SerializeField]
        private float attackRadius;
        
        [SerializeField]
        private float attackDuration;
        
        [SerializeField] 
        private float attackCooldown;
        
        [SerializeField]
        [Range(0, 1)]
        private float attackSpeedDebuff;
       
        [SerializeField] 
        private LayerMask attackLayerMask;

        [SerializeField] 
        private bool showDebug;
        
        public float AttackDuration => attackDuration;
        public float AttackCooldown => attackCooldown;
        public float AttackSpeedDebuff => attackSpeedDebuff;
        public float Damage => damage;
        public LayerMask AttackLayerMask => attackLayerMask;
        public float AttackRadius => attackRadius;
        public bool ShowDebug => showDebug;
    }
}