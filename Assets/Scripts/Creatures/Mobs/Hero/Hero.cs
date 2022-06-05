using Components.ColliderBased;
using Components.Health;
using DefaultNamespace.Creatures;
using DefaultNamespace.Model;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Utils;

namespace Creatures.Mobs.Hero
{
    public class Hero : Creature
    {

        [SerializeField] private float _slamDownVelocity;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerCheck _wallCheck;

        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [Space] [Header("Particles")] [SerializeField]
        private ParticleSystem _hitParticles;

        [Space] [Header("Animators")] 
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disArmed;

        [SerializeField] private Cooldown _throwCooldown;

        // [SerializeField] private float _groundCheckRadius;
        // [SerializeField] private Vector3 _groundCheckPositionDelta;

        private int SwordCount => _session.Data.Inventory.Count("sword");
        private int CoinCount => _session.Data.Inventory.Count("coin");

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWallKey = Animator.StringToHash("is-on-wall");

        private SpriteRenderer _spriteRenderer;
        private bool _allowDoubleJump;
        private bool _isDashing = false;
        private float _dashCoolDown = 2.0f;
        private float _timeStamp;
        private int _swordsAmount;
        private bool _isOnWall;

        private GameSession _session;
        private float _defaultGravityScale;

        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Inventory.inventoryChanged += OnInventoryChanged;
            var health = GetComponent<HealthComponent>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            health.SetHealth(_session.Data.health);
            _swordsAmount = SwordCount;
            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.inventoryChanged -= OnInventoryChanged;
        }

        public void OnInventoryChanged(string id, int value)
        {
            if (id == "sword")
                UpdateHeroWeapon();

        }

        public void SaySomething()
        {
            Debug.Log("Something!");
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.health = currentHealth;
        }

        protected override void Update()
        {
            base.Update();

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            
            Animator.SetBool(IsOnWallKey, _isOnWall);
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
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
            if (!_isGrounded && _allowDoubleJump && !_isOnWall)
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
            var coinsCount = CoinCount;
            if (coinsCount > 0) SpawnCoins();
        }

        private void SpawnCoins()
        {
            var coinsCount = CoinCount;
            var numCoinsToDispose = Mathf.Min(coinsCount, 5);
            _session.Data.Inventory.Remove("coin", numCoinsToDispose);

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
            var swordCount = _session.Data.Inventory.Count("sword");
            if(SwordCount <= 0) return;
            base.Attack();
            _particles.Spawn("swish");
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disArmed;
        }

        public void OnDoThrow()
        {
            _particles.Spawn("throw");
        }
        
        public void Throw()
        {
            if (_armed && _throwCooldown.IsReady && SwordCount > 1)
            { 
                Animator.SetTrigger(ThrowKey); 
                _throwCooldown.Reset();
                _swordsAmount--; 
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