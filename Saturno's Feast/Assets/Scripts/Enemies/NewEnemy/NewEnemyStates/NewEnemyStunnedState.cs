using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class NewEnemyStunnedState : NewEnemyBaseState
    {
        private float time;

        public override void EnterState(NewEnemyBehaviour newEnemy)
        {
            newEnemy.currentEnumState = NewEnemyState.Stunned;

            newEnemy.Animator.SetBool("isStunned", true);

            time = 0.0f;
        }

        public override void Update(NewEnemyBehaviour newEnemy)
        {
            time += Time.deltaTime;

            if (time >= 1.0f)
            {
                newEnemy.AILerp.enabled = true;

                newEnemy.Animator.SetBool("isStunned", false);

                newEnemy.TransitionToState(newEnemy.FleeState);

            }
        }

        public override void OnCollisionEnter2D(NewEnemyBehaviour newEnemy, Collision2D collision)
        {
            return;
        }
    }
}