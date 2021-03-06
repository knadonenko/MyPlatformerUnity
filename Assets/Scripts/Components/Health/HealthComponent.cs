using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _onChange;

        public void ApplyDamage(int damageValue)
        {
            if(_health <= 0) return;

            _health -= damageValue;
            _onChange?.Invoke(_health);
            
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

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int> {}

        public void SetHealth(int health)
        {
            _health = health;
        }
    }   
}