using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BasicEnemyWanderState : BasicEnemyBaseState
    {
        public override void EnterState(BasicEnemyBehaviour basicEnemy)
        {
            basicEnemy.currentEnumState = BasicEnemyState.Wander;

            basicEnemy.Animator.SetBool("isRunning", true);

            basicEnemy.AILerp.speed = basicEnemy.WanderSpeed;

            Wander(basicEnemy);
        }

        private void Wander(BasicEnemyBehaviour basicEnemy)
        {
            basicEnemy.IAstarAI.destination = PickRandomPoint(basicEnemy);
            basicEnemy.IAstarAI.SearchPath();
        }

        private Vector3 PickRandomPoint(BasicEnemyBehaviour basicEnemy)
        {
            Vector3 point = Random.insideUnitSphere * basicEnemy.WanderRadius;

            point.z = 0;
            point += basicEnemy.transform.position;
            return point;
        }

        public override void Update(BasicEnemyBehaviour basicEnemy)
        {
            if (Vector3.Distance(basicEnemy.transform.position, basicEnemy.Player.position) <= basicEnemy.ExteriorDistance)
            {
                basicEnemy.TransitionToState(basicEnemy.ChaseState);
            }
            else if (!basicEnemy.IAstarAI.pathPending && (basicEnemy.IAstarAI.reachedEndOfPath || !basicEnemy.IAstarAI.hasPath))
            {
                basicEnemy.TransitionToState(basicEnemy.IdleState);
            }
        }

        public override void OnCollisionEnter2D(BasicEnemyBehaviour basicEnemy, Collision2D collision)
        {
            return;
        }
    }
}