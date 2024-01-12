using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MedKit : MonoBehaviour
{
    [SerializeField, Min(0)] private float _healAmount;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Health health) == true)
        {
            if (health.Value >= health.MaxHealth)
            {
                return;
            }
            
            health.Heal(_healAmount);
            
            Destroy(gameObject);
        }
    }
}
