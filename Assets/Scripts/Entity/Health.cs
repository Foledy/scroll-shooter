using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _startHealth;  

    public float Value { get; private set; }
    public float MaxHealth => _maxHealth;

    public event Action OnEntityDead;
    public event Action<float> OnEntityHeal;
    public event Action<float> OnEntityTakeDamage;

    private void Awake()
    {
        Value = _startHealth;

        OnEntityHeal?.Invoke(Value);
    }

    public void TakeDamage(float damage)
    {
        if(damage >= 0)
        {
            Value -= damage;

            OnEntityTakeDamage?.Invoke(Value);

            if (IsAlive == false)
            {
                OnEntityDead?.Invoke();
            }
        }
    }

    public void Heal(float amount)
    {
        if(amount >= 0)
        {
            Value += amount;

            Value = Value > _maxHealth ? _maxHealth : Value;

            OnEntityHeal?.Invoke(Value);
        }
    }

    public bool IsAlive => Value > 0;
}
