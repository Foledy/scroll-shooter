using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(BoxCollider2D))]
    class EnemyPatrolTrigger : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemies;

        private event Action<Transform> OnEnemyEntered;
        private event Action<GameObject> OnEnemyLeft;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            foreach (Enemy enemy in _enemies)
            {
                OnEnemyEntered += enemy.EnemyInTrigger;
                OnEnemyLeft += enemy.EnemyLeftTrigger;
                enemy.OnEnemyDie += UnSubscribeEnemy;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnEnemyEntered?.Invoke(collision.transform);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnEnemyLeft?.Invoke(other.gameObject);
        }

        private void UnSubscribeEnemy(Enemy enemy)
        {
            OnEnemyEntered -= enemy.EnemyInTrigger;
            OnEnemyLeft -= enemy.EnemyLeftTrigger;
        }
    }
}
