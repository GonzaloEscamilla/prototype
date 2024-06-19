using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.DamageSystem;
using KinematicCharacterController;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class CharacterWeaponController : MonoBehaviour
    {
        [SerializeField]
        private bool isAttacking;

        [SerializeField]
        private CharacterWeaponsSettings settings;

        [SerializeField] 
        private Transform attackSpawnPoint;
        
        [SerializeField] 
        private StaffViewController staffView;

        [SerializeField] 
        private KinematicCharacterMotor characterMotor;
        
        public Action AttackStarted;
        public Action AttackPerformed;
        public Action AttackFinished;
        
        private float _attackElapsedTime;
        public bool IsAttacking => isAttacking;

        public void Attack()
        {
            if (isAttacking)
            {
                return;
            }

            StartCoroutine(Attacking());
        }

        private IEnumerator Attacking()
        {
            isAttacking = true;
            AttackStarted?.Invoke();

            _attackElapsedTime = 0;
            
            staffView.AttackAnimation(settings.AttackDuration);

            TryToHit();
                
            while (_attackElapsedTime <= settings.AttackDuration)
            {
                _attackElapsedTime += Time.deltaTime;
                
                AttackPerformed?.Invoke();
                yield return null;
            }

            isAttacking = false;
            AttackFinished?.Invoke();
        }

        private void TryToHit()
        {
            var hits = Physics.OverlapSphere(attackSpawnPoint.position, settings.AttackRadius, settings.AttackLayerMask);

            var damageReceiver = new List<IDamageable>();
            foreach (var hit in hits)
            {
                var hitReceiver = hit.GetComponent<IDamageable>();
                if (hitReceiver == null) continue;
                
                var dmgInfo = new AttackData(this.gameObject, characterMotor.CharacterForward.normalized ,settings.Damage);
                hitReceiver.GetHit(dmgInfo);
            }
        }

        private void OnDrawGizmos()
        {
            if (!settings && !settings.ShowDebug)
            {
                return;
            }

            Color debugSphereColor = isAttacking ? Color.red : Color.gray;

            Gizmos.color = debugSphereColor;

            if (isAttacking)
            {
                Gizmos.DrawSphere(attackSpawnPoint.position, settings.AttackRadius);
                return;
            }
            Gizmos.DrawWireSphere(attackSpawnPoint.position, settings.AttackRadius);
        }
    }
}