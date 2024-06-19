using UnityEngine;

namespace _Project.Scripts.Core.DamageSystem
{
    public struct AttackData
    {
        public GameObject Dealer { get; private set; }
        public Vector3 AttackDirection { get; private set; }
        public float Damage { get; private set; }
        public Vector3 AttackRelativeImpulse { get; private set; }
        
        public AttackData(GameObject dealer, Vector3 attackDirection, float damage)
        {
            Dealer = dealer;
            AttackDirection = attackDirection;
            Damage = damage;
            AttackRelativeImpulse = attackDirection * damage;
        }
    }
    
    public interface IDamageable
    {
        public void GetHit(AttackData attackData);
    }
}