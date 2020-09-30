using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BossEnemyChargeState : BossEnemyBaseState
    {
        private Vector3 myLastPosition;
        private Vector3 lastPlayerPosition;

        public override void EnterState(BossEnemyBehaviour bossEnemy)
        {
            bossEnemy.currentEnumState = BossEnemyState.Charge;

            lastPlayerPosition = bossEnemy.Player.transform.position;
            myLastPosition = bossEnemy.transform.position;

            bossEnemy.Animator.SetBool("isCharging", true);
            bossEnemy.Animator.SetFloat("angle", CalculateAngle());
        }

        public override void Update(BossEnemyBehaviour bossEnemy)
        {
            bossEnemy.transform.Translate((lastPlayerPosition - myLastPosition).normalized * bossEnemy.ChargeSpeed * Time.deltaTime);
        }

        public override void OnCollisionEnter2D(BossEnemyBehaviour bossEnemy, Collision2D collision)
        {
            if(bossEnemy.enabled)
            {
                if (collision.gameObject.CompareTag("Collision"))
                {
                    bossEnemy.Animator.SetBool("isCharging", false);

                    bossEnemy.TimeSinceLastCharge = 0.0f;
                    bossEnemy.ChargeCooldown = Random.Range(bossEnemy.MinChargeCooldown, bossEnemy.MaxChargeCooldown);

                    bossEnemy.TransitionToState(bossEnemy.StunnedState);
                }
                else if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<PlayerBehaviour>().TakeDamage(bossEnemy.ChargeDamage);
                }
            }
        }

        private float CalculateAngle()
        {
            Vector3 lookDirection = lastPlayerPosition - myLastPosition;
            lookDirection.Normalize();
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;

            return angle;
        }
    }
}