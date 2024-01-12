using Assets.Scripts.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    abstract class Enemy : GameEntity
    {
        [Header("Enemy Parameters")] 
        [SerializeField] protected int _scoreForKill;
        [SerializeField] protected float _passDistance;
        [SerializeField] protected float _idleDelay;
        [SerializeField] protected float _speed;
        
        [Header("Other")]
        [SerializeField] protected Transform[] _patrolPoints;
        [SerializeField] protected WeaponAttack _weapon;

        public static List<GameObject> AllEnemies { get; private set; }
        
        public static event Action<int> OnEnemyDead;
        public event Action<Enemy> OnEnemyDie;

        public GameObject Entity => gameObject;

        private Health _health;
        private SpriteRenderer _sprite;
        private Coroutine _waitCoroutine;
        private Transform _enemyTransform;
        private Vector2 _currentPatrolTargetPosition;
        private Rigidbody2D _rigidbody;
        private int _currentPatrolIndex;
        private float _currentDirection;
        private bool _enemyIsNear;
        private bool _isIdleActive;
        
        private void Start()
        {
            _health = GetComponent<Health>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();

            if (AllEnemies == null)
            {
                AllEnemies = new List<GameObject>();
            }

            AllEnemies.Add(gameObject);
            
            _health.OnEntityDead += EnemyDead;
            
            _isIdleActive = false;
            _enemyIsNear = false;
            _currentPatrolIndex = 0;
            _currentPatrolTargetPosition = _patrolPoints[_currentPatrolIndex].position;
        }
        
        private void Update()
        {
            if (_enemyIsNear == true)
            {
                if (Vector2.Distance(_enemyTransform.position, transform.position) > _weapon.AttackDistance)
                {
                    Move(_enemyTransform.position);
                }
                else
                {
                    Attack();
                }
            }
            else
            {
                if (_isIdleActive == false)
                {
                    Move(_currentPatrolTargetPosition);
                }
            }
        }

        public void EnemyInTrigger(Transform enemyTransform)
        {
            if (enemyTransform == null)
            {
                return;
            }
            
            if (enemyTransform.TryGetComponent(out PlayerController playerController) == false)
            {
                return;
            }
            
            _enemyTransform = enemyTransform;
            _enemyIsNear = true;

            if (_waitCoroutine != null)
            {
                _isIdleActive = false;
                
                StopCoroutine(_waitCoroutine);
            }
        }

        public void EnemyLeftTrigger(GameObject entity)
        {
            if (_enemyTransform == null || _enemyIsNear == false)
            {
                return;
            }
            
            if (entity.TryGetComponent(out PlayerController playerController) == false)
            {
                return;
            }
            
            if (_enemyIsNear == true && _enemyTransform != null)
            {
                _enemyIsNear = false;
                _enemyTransform = null;
            }
        }

        protected void EnemyDead()
        {
            OnEnemyDead?.Invoke(_scoreForKill);
            
            OnEnemyDie?.Invoke(this);
            
            Destroy(gameObject);
        }

        protected void Attack()
        {
            _weapon.Attack(_enemyTransform.position.x > transform.position.x ? 1 : -1);
        }

        protected void Move(Vector2 target)
        {
            bool targetIsToRight = target.x > transform.position.x;
            Vector2 direction = targetIsToRight ? Vector2.right : Vector2.left;
            _rigidbody.velocity = new Vector2(direction.x * _speed, _rigidbody.velocity.y);

            _sprite.flipX = !targetIsToRight;

            if (_enemyIsNear == false)
            {
                if (Vector2.Distance(_currentPatrolTargetPosition, transform.position) < _passDistance)
                {
                    _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
                    
                    GetNextPatrolPosition();
                
                    Wait();
                }
            }
        }

        protected void Wait()
        {
            _isIdleActive = true;

            _waitCoroutine = StartCoroutine(StartIdleWait());
        }

        private IEnumerator StartIdleWait()
        {
            yield return new WaitForSeconds(_idleDelay);

            _isIdleActive = false;
        }

        private void GetNextPatrolPosition()
        {
            if (_currentPatrolIndex + 1 >= _patrolPoints.Length)
            {
                _currentPatrolIndex = 0;
            }
            else
            {
                _currentPatrolIndex += 1;
            }

            _currentPatrolTargetPosition = _patrolPoints[_currentPatrolIndex].position;
        }
    }
}
