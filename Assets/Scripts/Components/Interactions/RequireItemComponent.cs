using DefaultNamespace.Model;
using Model.Data;
using Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Interactions
{
    public class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _reqired;
        // [InventoryId] [SerializeField] private string _id;
        // [SerializeField] private int _count;
        [SerializeField] private bool _removeAfterUse;

        [SerializeField] private UnityEvent onSuccess;
        [SerializeField] private UnityEvent onFail;

        public void Check()
        {
            var session = FindObjectOfType<GameSession>();
            var areAllRequirementsMet = true;
            foreach (var item in _reqired)
            {
                var numItems = session.Data.Inventory.Count(item.Id);
                if (numItems < item.Value)
                {
                    areAllRequirementsMet = false;
                }
                
            }

            if (areAllRequirementsMet)
            {
                foreach (var item in _reqired)
                {
                    if (_removeAfterUse)
                        session.Data.Inventory.Remove(item.Id, item.Value);
                    onSuccess.Invoke();   
                }
            }
            else
            {
                onFail?.Invoke();
            }
        }
    }
}