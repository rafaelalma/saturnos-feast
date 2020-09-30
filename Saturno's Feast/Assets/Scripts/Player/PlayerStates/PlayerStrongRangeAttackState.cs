using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerStrongRangeAttackState : PlayerBaseState
    {
        private float time;
        private bool hasAttacked;

        public override void EnterState(PlayerBehaviour player)
        {
            player.currentEnumState = PlayerState.StrongRangeAttack;

            time = 0.0f;

            hasAttacked = false;

            player.Animator.SetBool("isStrongRangeAttacking", true);
        }

        public override void ShouldTransitionToTakeDamageState(PlayerBehaviour player)
        {
            TransitionToTakeDamageState(player, "isStrongRangeAttacking");
        }

        public override void Update(PlayerBehaviour player)
        {
            time += Time.deltaTime;

            if (time >= 0.25f && !hasAttacked && !player.HasRangeAttackImprovement)
            {
                FireProjectile(player);
            }
            else if (time >= 0.25f && player.HasRangeAttackImprovement)
            {
                FireProjectile(player);

                TransitionToIdleState(player, "isStrongRangeAttacking");
            }
            else if (time >= 0.5f)
            {
                TransitionToIdleState(player, "isStrongRangeAttacking");
            }
            else if (Input.GetKey(KeyCode.Space) && !player.PausedGame)
            {
                TransitionToBlockState(player, "isStrongRangeAttacking");
            }
        }

        public void FireProjectile(PlayerBehaviour player)
        {
            hasAttacked = true;

            GameObject projectile = ObjectPooling.Instance.GetAvailableObject("PlayerProjectile");

            projectile.transform.position = player.FirePoint.position;
            projectile.transform.rotation = player.FirePoint.rotation;

            PlayerProjectile playerProjectile = projectile.GetComponent<PlayerProjectile>();
            playerProjectile.Speed = player.ProjectileSpeed;
            playerProjectile.Damage = player.ProjectileDamage;

            projectile.SetActive(true);

            player.CurrentProjectiles--;

            CanvasManager.Instance.UpdateProjectiles(player.CurrentProjectiles);
        }
    }
}