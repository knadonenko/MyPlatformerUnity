using Creatures;
using Creatures.Mobs.Hero;
using UnityEngine;

namespace Components.Collectables
{
    public class ArmHeroComponent : MonoBehaviour
    {
        private Hero _hero;
        
        public void ArmHero(GameObject gameObject)
        {
            var hero = gameObject.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}