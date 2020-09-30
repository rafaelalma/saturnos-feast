using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class NewEnemyChaseState : NewEnemyBaseState
    {
        public override void EnterState(NewEnemyBehaviour newEnemy)
        {
            newEnemy.currentEnumState = NewEnemyState.Chase;

            newEnemy.Animator.SetBool("isRunning", true);

            newEnemy.AILerp.speed = newEnemy.ChaseSpeed;

            newEnemy.SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        }

        public override void Update(NewEnemyBehaviour newEnemy)
        {
            if (Vector3.Distance(newEnemy.transform.position, newEnemy.Player.position) <= newEnemy.InteriorDistance)
            {
                newEnemy.Animator.SetBool("isRunning", false);

                newEnemy.TransitionToState(newEnemy.AttackState);
            }
            else if (Vector3.Distance(newEnemy.transform.position, newEnemy.Player.position) >= newEnemy.ExteriorDistance)
            {
                newEnemy.TransitionToState(newEnemy.WanderState);
            }
            else
            {
                newEnemy.IAstarAI.destination = newEnemy.Player.position;
            }
        }

        public override void OnCollisionEnter2D(NewEnemyBehaviour newEnemy, Collision2D collision)
        {
            return;
        }
    }
}