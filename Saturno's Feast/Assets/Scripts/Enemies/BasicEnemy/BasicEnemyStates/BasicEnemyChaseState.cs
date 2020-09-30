using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BasicEnemyChaseState : BasicEnemyBaseState
    {
        public override void EnterState(BasicEnemyBehaviour basicEnemy)
        {
            basicEnemy.currentEnumState = BasicEnemyState.Chase;

            basicEnemy.Animator.SetBool("isRunning", true);

            basicEnemy.AILerp.speed = basicEnemy.ChaseSpeed;
        }

        public override void Update(BasicEnemyBehaviour basicEnemy)
        {
            if (Vector3.Distance(basicEnemy.transform.position, basicEnemy.Player.position) >= basicEnemy.ExteriorDistance)
            {
                basicEnemy.TransitionToState(basicEnemy.WanderState);
            }
            else
            {
                basicEnemy.IAstarAI.destination = basicEnemy.Player.position;
            }
        }

        public override void OnCollisionEnter2D(BasicEnemyBehaviour basicEnemy, Collision2D collision)
        {

            if (basicEnemy.enabled)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<PlayerBehaviour>().TakeDamage(basicEnemy.AttackDamage);

                    basicEnemy.TransitionToState(basicEnemy.FleeState);
                }
            }
        }
    }
}