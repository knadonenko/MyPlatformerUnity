using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinValueComponent : MonoBehaviour
{
    [SerializeField] private Hero _hero;
    [SerializeField] private int _coinValue;

    public void UpdateCoinsSum()
    {
        _hero.UpdateCoins(_coinValue);
    }
}
