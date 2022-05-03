using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDefinition items;
        public InventoryItemsDefinition Items => items;

        private static DefsFacade _instance;
        public static DefsFacade instDef => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
        
    }
}