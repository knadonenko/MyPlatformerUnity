using System;
using Model.Data;
using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemDefinition : ScriptableObject
    {
        [SerializeField] private ItemDef[] items;
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string id;
        public string Id => id;
    }
}