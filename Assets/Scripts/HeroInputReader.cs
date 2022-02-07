using Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }

    public void OnLeftMouseButtonClick(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.SaySomething();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Interact();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.DoDash();
        }
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Attack();
        }
    }
}