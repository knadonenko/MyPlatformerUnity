using System;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData inventory;
        // public int coin;
        public int health;
        // public bool isArmed;
        // public int  swordAmount;

        public InventoryData Inventory => inventory;
    }
} 