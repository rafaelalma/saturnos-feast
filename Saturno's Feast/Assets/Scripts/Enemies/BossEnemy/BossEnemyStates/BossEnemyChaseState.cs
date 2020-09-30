using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BossEnemyChaseState : BossEnemyBaseState
    {
        public override void EnterState(BossEnemyBehaviour bossEnemy)
        {
            bossEnemy.currentEnumState = BossEnemyState.Chase;

            bossEnemy.Animator.SetBool("isChasing", true);

            bossEnemy.IAstarAI.destination = bossEnemy.Player.position;
            bossEnemy.IAstarAI.SearchPath();

            bossEnemy.AILerp.enabled = true;
            bossEnemy.AILerp.speed = bossEnemy.ChaseSpeed;
        }

        public override void Update(BossEnemyBehaviour bossEnemy)
        {
            if (Vector3.Distance(bossEnemy.transform.position, bossEnemy.Player.position) <= bossEnemy.InteriorDistance)
            {

                bossEnemy.Animator.SetBool("isChasing", false);

                bossEnemy.AILerp.enabled = false;

                bossEnemy.TransitionToState(bossEnemy.AttackState);
            }
            else if (bossEnemy.TimeSinceLastCharge >= bossEnemy.ChargeCooldown && Vector3.Distance(bossEnemy.transform.position, bossEnemy.Player.position) > bossEnemy.InteriorDistance)
            {
                bossEnemy.Animator.SetBool("isChasing", false);

                bossEnemy.AILerp.enabled = false;

                bossEnemy.TransitionToState(bossEnemy.ChargeState);
            }
            else
            {
                bossEnemy.IAstarAI.destination = bossEnemy.Player.position;

                bossEnemy.Animator.SetFloat("angle", CalculateAngle(bossEnemy));
            }
        }

        public override void OnCollisionEnter2D(BossEnemyBehaviour bossEnemy, Collision2D collision)
        {
            return;
        }

        private float CalculateAngle(BossEnemyBehaviour bossEnemy)
        {
            Vector3 lookDirection = bossEnemy.IAstarAI.destination - bossEnemy.transform.position;
            lookDirection.Normalize();
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;

            return angle;
        }
    }
}