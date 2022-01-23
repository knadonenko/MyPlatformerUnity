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
    [SerializeField] private SpawnComponent _footStepParticles;
    [SerializeField] private SpawnComponent _jumpParticles;
    [SerializeField] private SpawnComponent _fallParticles;
    [SerializeField] private ParticleSystem _hitParticles;

    // [SerializeField] private float _groundCheckRadius;
    // [SerializeField] private Vector3 _groundCheckPositionDelta;

    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _allowDoubleJump;
    private bool _isGrounded;
    private bool _isFalling = true;
    private bool _isDashing = false;
    private float _dashCoolDown = 3.0f;
    private float _timeStamp;
    
    private static readonly int IsGroundedKey = Animator.StringToHash("is-grounded");
    private static readonly int IsRunningKey = Animator.StringToHash("is-running");
    private static readonly int VertVelocityKey = Animator.StringToHash("vertical-velocity");
    private static readonly int HitKey = Animator.StringToHash("hit");

    private int _coinsSum = 0;

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

    private void Update()
    {
        _isGrounded = IsGrounded();
        _isFalling = CheckIsFalling();
        CheckFallen();
    }

    private void CheckFallen()
    {
        if (_isFalling && _isGrounded && !_allowDoubleJump)
        {
            _fallParticles.Spawn();
            _isFalling = false;
        }
    }

    private void FixedUpdate()
    {
        var xVelocity = CalculateXVelocity();//_direction.x * speed;
        var yVelocity = CalculateYVelocity();

        _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        
        _animator.SetBool(IsGroundedKey, _isGrounded);
        _animator.SetBool(IsRunningKey, _direction.x != 0);
        _animator.SetFloat(VertVelocityKey, _rigidbody.velocity.y);

        UpdateSpriteDirection();
    }

    public void UpdateCoins(int coinValue)
    {
        _coinsSum += coinValue;
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

    private float CalculateXVelocity()
    {
        var xVelocity = _direction.x * speed;

        if (_isDashing)
        {
            xVelocity *= 15f;
            _isDashing = false;
        } 
        
        return xVelocity;
    }

    private float CalculateJumpVelocity(float yVelocity)
    {
        var isFalling = CheckIsFalling();
        if (!isFalling) return yVelocity;

        if (IsGrounded())
        {
            yVelocity = jumpSpeed;
            _jumpParticles.Spawn();
        }
        else if(_allowDoubleJump)
        {
            yVelocity += jumpSpeed;
            _jumpParticles.Spawn();
            _allowDoubleJump = false;
        }

        return yVelocity;
    }

    private bool CheckIsFalling()
    {
        return _rigidbody.velocity.y <= 0.001f;
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
            transform.localScale = Vector3.one;
        }
        else if (_direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void TakeDamage()
    {
        _animator.SetTrigger(HitKey);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpDamageSpeed);

        if (_coinsSum > 0)
        {
            SpawnCoins();
        }
    }

    private void SpawnCoins()
    {
        var numCoinsToDispose = Mathf.Min(_coinsSum, 5);
        _coinsSum -= numCoinsToDispose;

        var burst = _hitParticles.emission.GetBurst(0);
        burst.count = numCoinsToDispose;
        _hitParticles.emission.SetBurst(0, burst);
        
        _hitParticles.gameObject.SetActive(true);
        _hitParticles.Play();
    }

    public void SpawnFootDust()
    {
        _footStepParticles.Spawn();
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

    public void DoDash()
    {
        Debug.Log(_timeStamp + "  " + Time.time);
        if (Time.time > _timeStamp + _dashCoolDown && !_isDashing)
        {
            _timeStamp = Time.time;
            Debug.Log("DASHING");
            _isDashing = true;
        }
    }
}