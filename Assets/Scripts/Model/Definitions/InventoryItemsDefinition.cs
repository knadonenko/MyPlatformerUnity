using System;
using Model.Data;
using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDefinition : ScriptableObject
    {
        [SerializeField] private ItemDef[] items;

        public ItemDef Get(String id)
        {
            foreach (var itemDef in items)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string id;
        public string Id => id;
        public bool IsVoid => string.IsNullOrEmpty(id);
    }
}