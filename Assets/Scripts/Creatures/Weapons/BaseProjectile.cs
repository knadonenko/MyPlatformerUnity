using UnityEngine;

namespace Creatures.Weapons
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float flySpeed;
        [SerializeField] private bool invertX;

        protected Rigidbody2D Rigidbody;
        protected int Direction;
        
        protected virtual void Start()
        {
            var mod = invertX ? -1 : 1;
            Direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
            Rigidbody = GetComponent<Rigidbody2D>();
        } 
    }
}