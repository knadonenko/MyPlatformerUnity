using System;
using Components.ColliderBased;
using Components.GOBased;
using UnityEngine;
using Utils;

namespace Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [Header("Melee")]
        [SerializeField] private Cooldown meleeCoolDown;
        [SerializeField] private CheckCircleOverlap meleeAttack;
        [SerializeField] private LayerCheck meleeCanAttack;
        [SerializeField] private bool canPerformMelee = true;
        
        [Header("Range")]
        [SerializeField] private Cooldown rangeCooldown;
        [SerializeField] private SpawnComponent rangeAttack;

        [SerializeField] private LayerCheck vision;
        private Animator animator;
        
        protected static readonly int MeleeKey = Animator.StringToHash("melee");
        protected static readonly int RangeKey = Animator.StringToHash("range");

        

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (vision.IsTouchingLayer)
            {
                if (canPerformMelee)
                {
                    if (meleeCanAttack.IsTouchingLayer)
                    {
                        if (meleeCoolDown.IsReady)
                            MeleeAttack();
                        return;
                    }    
                }

                if (rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void RangeAttack()
        {
            rangeCooldown.Reset();
            animator.SetTrigger(RangeKey);
        }

        private void MeleeAttack()
        {
            meleeCoolDown.Reset(); 
            animator.SetTrigger(MeleeKey);
        }

        public void OnMeleeAttack()
        {
            meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            rangeAttack.Spawn();
        }
    }
}