using System;
using UnityEngine;

namespace Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        protected override void Start()
        {
            base.Start();
            
            var force = new Vector2(Direction * flySpeed, 0);
            Rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        // private void FixedUpdate()
        // {
        //     var position = _rigidbody.position;
        //     position.x += _direction * _flySpeed;
        //     _rigidbody.MovePosition(position);
        // }
    }
}