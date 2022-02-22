using System;
using Components;
using UnityEngine;

namespace DefaultNamespace.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Parameters")] 
        [SerializeField] private bool _invertScale;
        [SerializeField] protected float jumpSpeed;
        [SerializeField] private float jumpDamageSpeed;
        [SerializeField] protected float speed;
        [SerializeField] protected int _damage;
        [SerializeField] protected LayerMask _groundLayer;
        
        [SerializeField] protected CheckCircleOverlap _attackRange;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] protected SpawnListComponent _particles;
        
        protected Rigidbody2D _rigidbody;
        protected Vector2 Direction;
        protected Animator Animator;
        protected bool _isGrounded;
        private bool _isJumping;
        
        private static readonly int IsGroundedKey = Animator.StringToHash("is-grounded");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int VertVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int HitKey = Animator.StringToHash("hit");
        protected static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            _isGrounded = _groundCheck.IsTouchingLayer;
        }
        
        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }
        
        private void FixedUpdate()
        {
            var xVelocity = CalculateXVelocity();
            var yVelocity = CalculateYVelocity();

            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(IsGroundedKey, _isGrounded);
            Animator.SetBool(IsRunningKey, Direction.x != 0);
            Animator.SetFloat(VertVelocityKey, _rigidbody.velocity.y);
 
            UpdateSpriteDirection(Direction);
        }
        
        protected virtual float CalculateXVelocity()
        {
            return Direction.x * speed;
        }
        
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (_isGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;
                var isFalling = _rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping) yVelocity *= 0.5f;

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (_isGrounded)
            {
                yVelocity = jumpSpeed;
                _particles.Spawn("jump");
            }

            return yVelocity;
        }

        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(HitKey);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpDamageSpeed);
        }
        
        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
                transform.localScale = new Vector3(multiplier, 1, 1);
            else if (direction.x < 0) transform.localScale = new Vector3(-1 * multiplier, 1, 1);
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
        }
        
        public void PerformAttack()
        {
            _attackRange.Check();
        }
        
    }
}