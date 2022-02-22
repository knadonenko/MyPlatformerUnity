using Components;
using DefaultNamespace;
using DefaultNamespace.Creatures;
using DefaultNamespace.Model;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Utils;

namespace Creatures
{
    public class Hero : Creature
    {

        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private float _interactionRadius;

        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [Space] [Header("Particles")] [SerializeField]
        private ParticleSystem _hitParticles;

        [Space] [Header("Animators")] [SerializeField]
        private AnimatorController _armed;

        [SerializeField] private AnimatorController _disArmed;

        // [SerializeField] private float _groundCheckRadius;
        // [SerializeField] private Vector3 _groundCheckPositionDelta;

        private static readonly int ThrowKey = Animator.StringToHash("throw");

        private SpriteRenderer _spriteRenderer;
        private bool _allowDoubleJump;
        private bool _isDashing = false;
        private float _dashCoolDown = 2.0f;
        private float _timeStamp;

        private GameSession _session;
        private float _defaultGravityScale;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = _rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            health.SetHealth(_session.Data.Health);
            UpdateHeroWeapon();
        }

        public void SaySomething()
        {
            Debug.Log("Something!");
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Health = currentHealth;
        }

        protected override void Update()
        {
            base.Update();
        }

        public void UpdateCoins(int coinValue)
        {
            _session.Data.Coin += coinValue;
        }

        protected override float CalculateYVelocity()
        {
            var isJumpPressing = Direction.y > 0;
            
            if (_isGrounded)
            {
                _allowDoubleJump = true;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!_isGrounded && _allowDoubleJump)
            {
                _allowDoubleJump = false;
                _particles.Spawn("jump");
                return jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }
        
        protected override float CalculateXVelocity()
        {
            if (_isDashing)
            {
                _isDashing = false;
                return Direction.x * speed * 15f;
            } 
        
            return base.CalculateXVelocity();
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.Data.Coin > 0) SpawnCoins();
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coin, 5);
            _session.Data.Coin -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);
        
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public void DoDash()
        {
            if (Time.time > _timeStamp + _dashCoolDown && !_isDashing)
            {
                _timeStamp = Time.time;
                Debug.Log("DASHING");
                _isDashing = true;
            }
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.IsInLayer(_groundLayer))
            {
                var contact = col.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity) _particles.Spawn("fall");
            }
        }

        public override void Attack()
        {
            if(!_session.Data.IsArmed) return;
            base.Attack();
            _particles.Spawn("swish");
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
            Animator.runtimeAnimatorController = _armed;
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disArmed;
        }

        public void OnDoThrow()
        {
            _particles.Spawn("throw");
        }
        
        public void Throw()
        {
            if (_armed)
            { 
                Animator.SetTrigger(ThrowKey);    
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Debug.DrawRay(transform.position, Vector3.down, IsGrounded() ? Color.blue : Color.red);
            // Gizmos.color = IsGrounded() ? Color.blue : Color.red;
            // Gizmos.DrawSphere(transform.position, 0.3f);
            Handles.color = _isGrounded ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, 0.3f);
        }
#endif
    }
} 