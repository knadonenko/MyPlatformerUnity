using System;
using Model.Data;
using UnityEngine;

namespace DefaultNamespace.Model
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;
        public int Coin;
        public int Health;
        public bool IsArmed;
        public int  SwordAmount;
    }
} 