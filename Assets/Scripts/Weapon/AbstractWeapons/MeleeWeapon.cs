using System.Collections;
using Assets.Scripts.Enemy;
using Audio;
using DefaultNamespace;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    abstract class MeleeWeapon : WeaponAttack
    {
        [SerializeField] private float _attackRadius;

        [Header("Other")] 
        [SerializeField] protected LayerMask _entityMask;

        private bool _canAttack;

        private void Start()
        {
            _canAttack = true;
        }

        public override void Attack(float direction)
        {
            if (_canAttack == true)
            {
                AudioManager.KatanaAttackSound();

                var hits = GetObjects(direction);

                if (hits.Length > 0)
                {
                    foreach (var hit in hits)
                    {
                        HitEntity(hit.collider.gameObject);
                    }
                }

                _canAttack = false;

                AttackDelay();
            }
        }
        
        private void HitEntity(GameObject entity)
        {
            if (entity == null)
            {
                return;
            }

            if (_isEnemy == true && HasEntityInList(entity) == true)
            {
                return;
            }
            
            if (entity.TryGetComponent(out Health health) == true)
            {
                health.TakeDamage(_damage);

                AudioManager.KatanaHitSound();
            }
        }

        private RaycastHit2D[] GetObjects(float direction)
            => Physics2D.CircleCastAll(transform.position, _attackRadius, direction >= 0 ? Vector2.right : Vector2.left, AttackDistance, _entityMask);

        protected override void AttackDelay() => StartCoroutine(StartDelay());

        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(_attackDelay);

            _canAttack = true;
        }
    }
}
