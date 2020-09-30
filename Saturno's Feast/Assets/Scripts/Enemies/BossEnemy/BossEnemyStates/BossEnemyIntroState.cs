using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BossEnemyIntroState : BossEnemyBaseState
    {
        private float time;

        public override void EnterState(BossEnemyBehaviour bossEnemy)
        {
            bossEnemy.currentEnumState = BossEnemyState.Intro;

            time = 0.0f;
        }

        public override void Update(BossEnemyBehaviour bossEnemy)
        {
            time += Time.deltaTime;

            if (time >= 2.167f && Vector3.Distance(bossEnemy.transform.position, bossEnemy.Player.position) <= bossEnemy.ExteriorDistance)
            {
                bossEnemy.TransitionToState(bossEnemy.ChargeState);
            }
        }

        public override void OnCollisionEnter2D(BossEnemyBehaviour bossEnemy, Collision2D collision)
        {
            return;
        }
    }
}