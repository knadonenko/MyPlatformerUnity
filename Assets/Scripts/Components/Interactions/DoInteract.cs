using UnityEngine;

namespace Components.Interactions
{
    public class DoInteract : MonoBehaviour
    {
        public void DoInteraction(GameObject gameObject)
        {
            var interactableComponent = gameObject.GetComponent<InteractableComponent>();
            if (interactableComponent != null)
            {
                interactableComponent.Interact();
            }
        }
    }
}