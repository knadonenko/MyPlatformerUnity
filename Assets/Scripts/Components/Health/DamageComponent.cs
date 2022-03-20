using UnityEngine;

namespace Components.Health
{
    public class DamageComponent : MonoBehaviour
    {
        [SerializeField] private int _damage;

        public void ModifyHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            if (healthComponent != null)
            {
                healthComponent.ApplyDamage(_damage);
            }
        }
    }    
}