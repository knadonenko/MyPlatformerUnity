using Components.Health;
using UnityEngine;

namespace Components.Collectables
{
    public class PotionComponent : MonoBehaviour
    {
        [SerializeField] private int _healAmmount;

        public void Heal(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            if (healthComponent != null)
            {
                healthComponent.ApplyHeal(_healAmmount);
            }
        }
    }
}