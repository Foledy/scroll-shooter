using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audio;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    abstract class ShooterWeapon : WeaponAttack
    {
        [Header("Weapon Parameters")]
        [SerializeField] private float _reloadDelay;
        [SerializeField] private float _fireForce;
        [SerializeField] private int _magazineAmmo;

        [Header("Other")] 
        [SerializeField] private GameObject _bulletPrefab;

        public event Action<int> OnAmmoChanged;
        public event Action OnPlayerReload;

        public int CurrentMagazineAmmo { get; private set; }
        public int MaxMagazineAmmo { get; private set; }
        public bool CanAttack { get; private set; }

        private void Start()
        {
            CurrentMagazineAmmo = _magazineAmmo;
            MaxMagazineAmmo = _magazineAmmo;

            CanAttack = true;

            OnAmmoChanged?.Invoke(CurrentMagazineAmmo);
        }

        public override void Attack(float direction)
        {
            if (CanAttack == true)
            {
                var bullet = Instantiate(_bulletPrefab, GetAttackPosition(direction), Quaternion.identity);

                SubscribeBullet(bullet.GetComponent<Bullet>());

                var rb = bullet.GetComponent<Rigidbody2D>();

                rb.velocity = new Vector2(_fireForce * (direction < 0 ? -1 : 1), rb.velocity.y);
                
                AudioManager.FireAttackSound();

                CurrentMagazineAmmo -= 1;

                if (CurrentMagazineAmmo > 0)
                {
                    OnAmmoChanged?.Invoke(CurrentMagazineAmmo);
                    
                    AttackDelay();
                }
                else
                {
                    OnPlayerReload?.Invoke();
                    
                    Reload();
                }

                CanAttack = false;
            }
        }

        public void Reload()
        {
            CanAttack = false;

            StartCoroutine(StartReloadDelay(_reloadDelay));
        }

        protected override void AttackDelay() => StartCoroutine(StartAttackDelay(_attackDelay));

        private IEnumerator StartAttackDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            CanAttack = true;
        }

        private IEnumerator StartReloadDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            CanAttack = true;

            CurrentMagazineAmmo = _magazineAmmo;

            OnAmmoChanged?.Invoke(CurrentMagazineAmmo);
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

                AudioManager.FireHitSound();
            }
        }

        private void UnSubscribeBullet(Bullet bullet)
        {
            bullet.GetComponent<Bullet>().OnBulletHit -= HitEntity;
            bullet.GetComponent<Bullet>().OnBulletDestroy -= UnSubscribeBullet;
        }

        private void SubscribeBullet(Bullet bullet)
        {
            bullet.GetComponent<Bullet>().OnBulletHit += HitEntity;
            bullet.GetComponent<Bullet>().OnBulletDestroy += UnSubscribeBullet;
            
        }
    }
}
