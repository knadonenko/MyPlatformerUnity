using UnityEngine;
using UnityEngine.Events;

namespace Components.Interactions
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            Debug.Log("Interract");
            _action?.Invoke();
        }
    }
}