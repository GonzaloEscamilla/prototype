using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.DamageSystem
{
    public class ExampleDestructible : MonoBehaviour, IDamageable
    {
        [SerializeField] 
        private float MaxHealth;

        [SerializeField]
        private float hitImpulseMultiplier;
        
        [ShowInInspector]
        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;

                if (_currentHealth <= 0)
                {
                    _currentHealth = 0;
                    OnDestroyed();
                }
            }
        }
        
        public Action Destroyed;
        private float _currentHealth;

        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _currentHealth = MaxHealth;
            _rigidbody = GetComponent<Rigidbody>();
        }
        

        public void GetHit(AttackData attackData)
        {
            CurrentHealth -= attackData.Damage;
            _rigidbody.AddForce(attackData.AttackRelativeImpulse * hitImpulseMultiplier, ForceMode.Impulse);
            Debug.Log($"Being Attacked by {attackData.Dealer}", this.gameObject);
        }
        
        private void OnDestroyed()
        {
            Destroyed?.Invoke();
            Destroy(this.gameObject);
        }
    }
}