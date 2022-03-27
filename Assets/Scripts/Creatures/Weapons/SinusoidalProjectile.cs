using UnityEngine;

namespace Creatures.Weapons
{
    public class SinusoidalProjectile : BaseProjectile
    {

        [SerializeField] private float frequency = 1f;
        [SerializeField] private float amplitude = 1f;
        private float _originalY;
        private float _time;

        protected override void Start()
        {
            base.Start();

            _originalY = Rigidbody.position.y;
        }

        private void FixedUpdate()
        {
            var position = Rigidbody.position;
            position.x += Direction * flySpeed;
            position.y = _originalY + Mathf.Sin(_time * frequency) * amplitude;
            Rigidbody.MovePosition(position);
            _time += Time.fixedDeltaTime;
        }
    }
}