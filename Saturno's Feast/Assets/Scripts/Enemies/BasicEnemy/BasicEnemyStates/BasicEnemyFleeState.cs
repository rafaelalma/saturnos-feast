using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BasicEnemyFleeState : BasicEnemyBaseState
    {
        private Vector3 fleeVector;
        public override void EnterState(BasicEnemyBehaviour basicEnemy)
        {
            basicEnemy.currentEnumState = BasicEnemyState.Flee;

            basicEnemy.AILerp.speed = basicEnemy.ChaseSpeed;

            RandomizeFleeVector();
        }

        public override void Update(BasicEnemyBehaviour basicEnemy)
        {
            if (Vector3.Distance(basicEnemy.transform.position, basicEnemy.Player.position) > basicEnemy.InteriorDistance)
            {
                basicEnemy.TransitionToState(basicEnemy.ChaseState);
            }
            else
            {
                Flee(basicEnemy);
            }
        }

        private void Flee(BasicEnemyBehaviour basicEnemy)
        {
            basicEnemy.IAstarAI.destination = basicEnemy.transform.position - fleeVector * 2.0f;
        }

        public override void OnCollisionEnter2D(BasicEnemyBehaviour basicEnemy, Collision2D collision)
        {
            RandomizeFleeVector();
        }

        private void RandomizeFleeVector()
        {
            fleeVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            fleeVector.Normalize();
        }
    }
}