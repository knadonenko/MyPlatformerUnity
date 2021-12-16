using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    [SerializeField] private float jumpSpeed;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private LayerCheck _groundCheck;

    // [SerializeField] private float _groundCheckRadius;
    // [SerializeField] private Vector3 _groundCheckPositionDelta;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void SaySomething()
    {
        Debug.Log("Something!");   
    }

    private bool IsGrounded()
    {
        // var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, 
        //     _groundCheckRadius, Vector2.down, 0, _groundLayer);
        // return hit.collider != null;
        return _groundCheck.IsTouchingLayer;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * speed,  _rigidbody.velocity.y);
        var isJumping = _direction.y > 0;
        if (isJumping)
        {
            if (IsGrounded())
            {
                _rigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }
        } else if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(_direction.x,  _rigidbody.velocity.y * 0.5f);
        }
    }

    private void OnDrawGizmos()
    {
        // Debug.DrawRay(transform.position, Vector3.down, IsGrounded() ? Color.blue : Color.red);
        Gizmos.color = IsGrounded() ? Color.blue : Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
