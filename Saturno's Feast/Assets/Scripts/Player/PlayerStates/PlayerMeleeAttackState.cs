using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerMeleeAttackState : PlayerBaseState
    {
        private float time;
        private bool hasAttacked;

        public override void EnterState(PlayerBehaviour player)
        {
            player.currentEnumState = PlayerState.MeleeAttack;

            time = 0.0f;

            hasAttacked = false;

            player.Animator.SetBool("isMeleeAttacking", true);
        }

        public override void ShouldTransitionToTakeDamageState(PlayerBehaviour player)
        {
            TransitionToTakeDamageState(player, "isMeleeAttacking");
        }

        public override void Update(PlayerBehaviour player)
        {
            time += Time.deltaTime;

            if (time >= 0.25f && !hasAttacked && !player.HasMeleeAttackImprovement)
            {
                MeleeAttack(player);
            }
            else if (time >= 0.25f && player.HasMeleeAttackImprovement)
            {
                MeleeAttack(player);

                TransitionToIdleState(player, "isMeleeAttacking");
            }
            else if (time >= 0.5f)
            {
                TransitionToIdleState(player, "isMeleeAttacking");
            }
            else if (Input.GetKey(KeyCode.Space) && !player.PausedGame)
            {
                TransitionToBlockState(player, "isMeleeAttacking");
            }
        }

        public void MeleeAttack(PlayerBehaviour player)
        {
            hasAttacked = true;

            Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(player.transform.position, 2, player.EnemyLayer);

            foreach (var enemyToHit in enemiesToHit)
            {
                Vector3 vector = enemyToHit.gameObject.transform.position - player.transform.position;

                if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Melee_Back"))
                {
                    float angle = Vector3.Angle(Vector3.up, vector);

                    if (enemyToHit.gameObject.layer == 10 && angle <= 50.0f)
                    {
                        DealDamage(player, enemyToHit);
                    }
                    else if (enemyToHit.gameObject.layer == 12 && angle <= 90.0f)
                    {
                        DealDamage(player, enemyToHit);
                    }
                }
                else if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Melee_Right"))
                {
                    float angle = Vector3.Angle(Vector3.right, vector);

                    if (enemyToHit.gameObject.layer == 10 && angle <= 50.0f)
                    {
                        DealDamage(player, enemyToHit);
                    }
                    else if (enemyToHit.gameObject.layer == 12 && angle <= 90.0f)
                    {
                        DealDamage(player, enemyToHit);
                    }
                }
                else if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Melee_Front"))
                {
                    float angle = Vector3.Angle(Vector3.down, vector);

                    if (enemyToHit.gameObject.layer == 10 && angle <= 50.0f)
                    {
                        DealDamage(player, enemyToHit);
                    }
                    else if (enemyToHit.gameObject.layer == 12 && angle <= 90.0f)
                    {
                        DealDamage(player, enemyToHit);
                    }
                }
                else if (player.Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Melee_Left"))
                {
                    float angle = Vector3.Angle(Vector3.left, vector);

                    if (enemyToHit.gameObject.layer == 10 && angle <= 50.0f)
                    {
                        DealDamage(player, enemyToHit);
                    }
                    else if (enemyToHit.gameObject.layer == 12 && angle <= 90.0f)
                    {
                        DealDamage(player, enemyToHit);
                    }
                }
            }
        }

        private void DealDamage(PlayerBehaviour player, Collider2D enemyToHit)
        {
            enemyToHit.GetComponent<Enemy>().TakeDamage(player.MeleeAttackDamage);

            player.CurrentHealth += player.Lifesteal;
            if (player.CurrentHealth > player.MaxHealth)
            {
                player.CurrentHealth = player.MaxHealth;
            }

            CanvasManager.Instance.UpdateLifebar(player.CurrentHealth, player.MaxHealth);
        }
    }
}