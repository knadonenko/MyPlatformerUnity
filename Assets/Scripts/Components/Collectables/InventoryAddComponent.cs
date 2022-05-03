using Creatures.Mobs.Hero;
using UnityEngine;

namespace Components.Collectables 
{
    
    public class InventoryAddComponent : MonoBehaviour 
    {
        [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject gameObject) 
        {
            var hero = gameObject.GetComponent<Hero>();

            if (hero != null)
                hero.AddInInventory(_id, _count); 
        }
    }

}