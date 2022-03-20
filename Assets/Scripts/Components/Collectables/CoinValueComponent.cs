using Creatures;
using Creatures.Mobs.Hero;
using UnityEngine;

namespace Components.Collectables
{
    public class CoinValueComponent : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        [SerializeField] private int _coinValue;

        public void UpdateCoinsSum()
        {
            _hero.UpdateCoins(_coinValue);
        }
    }    
}