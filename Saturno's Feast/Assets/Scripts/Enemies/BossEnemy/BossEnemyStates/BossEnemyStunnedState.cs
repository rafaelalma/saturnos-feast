using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BossEnemyStunnedState : BossEnemyBaseState
    {
        private float time;

        public override void EnterState(BossEnemyBehaviour bossEnemy)
        {
            bossEnemy.currentEnumState = BossEnemyState.Stunned;

            time = 0.0f;
        }

        public override void Update(BossEnemyBehaviour bossEnemy)
        {
            time += Time.deltaTime;

            if (time >= 2.0f)
            {
                if (Vector3.Distance(bossEnemy.transform.position, bossEnemy.Player.transform.position) <= bossEnemy.InteriorDistance)
                {
                    bossEnemy.TransitionToState(bossEnemy.AttackState);
                }
                else
                {
                    bossEnemy.TransitionToState(bossEnemy.ChaseState);
                }
            }
        }

        public override void OnCollisionEnter2D(BossEnemyBehaviour bossEnemy, Collision2D collision)
        {
            return;
        }
    }
}