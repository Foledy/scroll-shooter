using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime;

    public event Action<GameObject> OnBulletHit;
    public event Action<Bullet> OnBulletDestroy;

    private Coroutine _routine;
    
    private void Start()
    {
        _routine = StartCoroutine(Lifetime());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnBulletHit?.Invoke(other.gameObject);
        OnBulletDestroy?.Invoke(this);
        
        StopCoroutine(_routine);
        
        Destroy(gameObject);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(_lifeTime);

        OnBulletDestroy?.Invoke(this);
        
        Destroy(gameObject);
    }
}
