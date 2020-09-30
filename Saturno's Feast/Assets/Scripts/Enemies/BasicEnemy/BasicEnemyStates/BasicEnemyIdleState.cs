using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BasicEnemyIdleState : BasicEnemyBaseState
    {
        private float time;
        private float randomTime;

        public override void EnterState(BasicEnemyBehaviour basicEnemy)
        {
            basicEnemy.currentEnumState = BasicEnemyState.Idle;

            basicEnemy.Animator.SetBool("isRunning", false);

            basicEnemy.AILerp.enabled = false;

            time = 0.0f;
            randomTime = Random.Range(0.0f, 5.0f);
        }

        public override void Update(BasicEnemyBehaviour basicEnemy)
        {
            time += Time.deltaTime;

            if (Vector3.Distance(basicEnemy.transform.position, basicEnemy.Player.position) <= basicEnemy.ExteriorDistance)
            {
                basicEnemy.AILerp.enabled = true;

                basicEnemy.TransitionToState(basicEnemy.ChaseState);
            }
            else if (time >= randomTime)
            {
                basicEnemy.AILerp.enabled = true;

                basicEnemy.TransitionToState(basicEnemy.WanderState);
            }
        }

        public override void OnCollisionEnter2D(BasicEnemyBehaviour basicEnemy, Collision2D collision)
        {
            return;
        }
    }
}