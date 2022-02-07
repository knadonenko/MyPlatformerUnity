using Creatures;
using UnityEngine;

public class CoinValueComponent : MonoBehaviour
{
    [SerializeField] private Hero _hero;
    [SerializeField] private int _coinValue;

    public void UpdateCoinsSum()
    {
        _hero.UpdateCoins(_coinValue);
    }
}
