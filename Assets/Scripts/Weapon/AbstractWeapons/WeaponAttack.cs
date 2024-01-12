using System;
using DefaultNamespace;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    abstract class WeaponAttack : MonoBehaviour
    {
        [Header("Weapon Parameters")]
        [SerializeField] protected float _damage;
        [SerializeField] protected float _attackDelay;
        [SerializeField] protected float _attackDistance;
        
        [HideInInspector] public float AttackDistance => _attackDistance;
        
        [Header("Other")]
        [SerializeField] protected Transform[] _attackPositions;
        [SerializeField] protected GameEntity _owner;
        [SerializeField] protected bool _isEnemy;

        public abstract void Attack(float direction);

        protected abstract void AttackDelay();

        protected Vector2 GetAttackPosition(float direction) => direction >= 0 ? _attackPositions[1].position : _attackPositions[0].position;

        protected bool HasEntityInList(GameObject gObject) => Enemy.Enemy.AllEnemies.Contains(gObject);
    }
}
