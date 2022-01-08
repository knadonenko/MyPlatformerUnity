using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableLayer;
    private Collider2D _collider;
    
    public bool IsTouchingLayer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_interactableLayer);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IsTouchingLayer = _collider.IsTouchingLayers(_interactableLayer);
    }
}
