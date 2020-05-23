﻿using System;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private LayerMask _damageableLayer = default;
    [SerializeField] private int _damageAmount = default;
    public event Action _hitTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.gameObject.layer) & _damageableLayer) != 0)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                _hitTarget?.Invoke();
                damageable.TakeDamage(_damageAmount, gameObject);
            }
        }
    }
}
