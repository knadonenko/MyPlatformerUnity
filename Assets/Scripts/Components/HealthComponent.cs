﻿using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;

        public void ApplyDamage(int damageValue)
        {
            _health -= damageValue;
            _onDamage?.Invoke();
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public void ApplyHeal(int healValue)
        {
            _health += healValue;
            if (_health > _maxHealth) _health = _maxHealth;
        }
    }   
}