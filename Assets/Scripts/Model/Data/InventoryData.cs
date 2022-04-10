using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();
        
        private InventoryItemData GetItem(string id)
        {
            return _inventory.FirstOrDefault(itemData => itemData.Id == id);
        }
        
        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var item = GetItem(id);

            if (item == null)
            {
                item = new InventoryItemData(id);
                _inventory.Add(item); 
            }
            item.Value += value;
        }

        public void Remove(string id, int value)
        {
            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}