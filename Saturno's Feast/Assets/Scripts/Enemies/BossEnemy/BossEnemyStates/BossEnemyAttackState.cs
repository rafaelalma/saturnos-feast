using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BossEnemyAttackState : BossEnemyBaseState
    {
        private float time;
        private float timeToDealDamage;

        public override void EnterState(BossEnemyBehaviour bossEnemy)
        {
            bossEnemy.currentEnumState = BossEnemyState.Attack;

            time = 0.0f;

            timeToDealDamage = 0.25f;

            bossEnemy.Animator.SetBool("isAttacking", true);
            bossEnemy.Animator.SetFloat("angle", CalculateAngle(bossEnemy));
        }

        public override void Update(BossEnemyBehaviour bossEnemy)
        {
            time += Time.deltaTime;

            if (time >= timeToDealDamage)
            {
                time = 0.0f;

                timeToDealDamage = 0.5f;

                Attack(bossEnemy);

                if (Vector3.Distance(bossEnemy.transform.position, bossEnemy.Player.position) >= bossEnemy.InteriorDistance)
                {
                    bossEnemy.Animator.SetBool("isAttacking", false);

                    bossEnemy.TransitionToState(bossEnemy.ChaseState);
                }

                bossEnemy.Animator.SetFloat("angle", CalculateAngle(bossEnemy));
            }
        }

        private void Attack(BossEnemyBehaviour bossEnemy)
        {
            Collider2D victim = Physics2D.OverlapCircle(bossEnemy.transform.position, bossEnemy.InteriorDistance, bossEnemy.PlayerLayer);
            {
                if (victim != null)
                {
                    Vector3 lookDirection = victim.gameObject.transform.position - bossEnemy.transform.position;

                    if (bossEnemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("BossEnemy_Attack_Back") && Vector3.Angle(Vector3.up, lookDirection) <= 45.0f)
                    {
                        bossEnemy.Player.GetComponent<PlayerBehaviour>().TakeDamage(bossEnemy.AttackDamage);
                    }
                    else if (bossEnemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("BossEnemy_Attack_Right") && Vector3.Angle(Vector3.right, lookDirection) <= 45.0f)
                    {
                        bossEnemy.Player.GetComponent<PlayerBehaviour>().TakeDamage(bossEnemy.AttackDamage);
                    }
                    else if (bossEnemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("BossEnemy_Attack_Front") && Vector3.Angle(Vector3.down, lookDirection) <= 45.0f)
                    {
                        bossEnemy.Player.GetComponent<PlayerBehaviour>().TakeDamage(bossEnemy.AttackDamage);
                    }
                    else if (bossEnemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("BossEnemy_Attack_Left") && Vector3.Angle(Vector3.left, lookDirection) <= 45.0f)
                    {
                        bossEnemy.Player.GetComponent<PlayerBehaviour>().TakeDamage(bossEnemy.AttackDamage);
                    }
                }
            }
        }

        public override void OnCollisionEnter2D(BossEnemyBehaviour bossEnemy, Collision2D collision)
        {
            return;
        }

        private float CalculateAngle(BossEnemyBehaviour bossEnemy)
        {
            Vector3 lookDirection = bossEnemy.Player.position - bossEnemy.transform.position;
            lookDirection.Normalize();
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;

            return angle;
        }
    }
}