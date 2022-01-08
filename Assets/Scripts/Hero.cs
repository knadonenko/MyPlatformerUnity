using Components;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpDamageSpeed;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask _groundLayer; 
    [SerializeField] private LayerCheck _groundCheck;
    [SerializeField] private LayerCheck _interactableCheck;
    [SerializeField] private float _interactionRadius;
    [SerializeField] private Collider2D[] _interactionResults = new Collider2D[1];
    [SerializeField] private LayerMask _interactableLayer;

    // [SerializeField] private float _groundCheckRadius;
    // [SerializeField] private Vector3 _groundCheckPositionDelta;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _allowDoubleJump;
    private bool _isGrounded;
    
    private static readonly int IsGroundedKey = Animator.StringToHash("is-grounded");
    private static readonly int IsRunningKey = Animator.StringToHash("is-running");
    private static readonly int VertVelocityKey = Animator.StringToHash("vertical-velocity");
    private static readonly int HitKey = Animator.StringToHash("hit");
    private static readonly int InteractKey = Animator.StringToHash("");

    private int coinsSum = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    private bool IsInteractable()
    {
        return _interactableCheck.IsTouchingLayer;
    }

    private void Update()
    {
        _isGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        var xVelocity = _direction.x * speed;
        var yVelocity = CalculateYVelocity();

        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        
        _animator.SetBool(IsGroundedKey, _isGrounded);
        _animator.SetBool(IsRunningKey, _direction.x != 0);
        _animator.SetFloat(VertVelocityKey, _rigidbody.velocity.y);

        UpdateSpriteDirection();
    }

    public void UpdateCoins(int coinValue)
    {
        coinsSum += coinValue;
        Debug.Log(coinsSum);
    }

    private float CalculateYVelocity()
    {
        var yVelocity = _rigidbody.velocity.y;
        var isJumpPressing = _direction.y > 0;

        if (IsGrounded()) _allowDoubleJump = true;

        if (isJumpPressing)
        {
            yVelocity = CalculateJumpVelocity(yVelocity);
        }
        else if (_rigidbody.velocity.y > 0)
        {
            yVelocity *= 0.5f;
        }

        return yVelocity;
    }

    private float CalculateJumpVelocity(float yVelocity)
    {
        var isFalling = _rigidbody.velocity.y <= 0.001f;
        if (!isFalling) return yVelocity;

        if (IsGrounded())
        {
            yVelocity = jumpSpeed;
        }
        else if(_allowDoubleJump)
        {
            yVelocity += jumpSpeed;
            _allowDoubleJump = false;
        }

        return yVelocity;
    }

    private void OnDrawGizmos()
    {
        // Debug.DrawRay(transform.position, Vector3.down, IsGrounded() ? Color.blue : Color.red);
        Gizmos.color = IsGrounded() ? Color.blue : Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }

    private void UpdateSpriteDirection()
    {
        if (_direction.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_direction.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(HitKey);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpDamageSpeed);
    }

    public void Interact()
    {
        var size = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            _interactionRadius,
            _interactionResults,
            _interactableLayer);

        for (int i = 0; i < size; i++)
        {
            Debug.Log("Interacting123456");
            var interactable = _interactionResults[i].GetComponent<InteractableComponent>();
            if (interactable != null) interactable.Interact();
        }
    }
}