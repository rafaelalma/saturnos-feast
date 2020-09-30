using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class NewEnemyAttackState : NewEnemyBaseState
    {
        private Vector3 myLastPosition;
        private Vector3 lastPlayerPosition;
        private float time;

        public override void EnterState(NewEnemyBehaviour newEnemy)
        {
            newEnemy.currentEnumState = NewEnemyState.Attack;

            newEnemy.Animator.SetBool("isAttacking", true);

            newEnemy.SpriteRenderer.color = Color.white;

            newEnemy.AILerp.enabled = false;

            time = 0.0f;

            myLastPosition = newEnemy.transform.position;
            lastPlayerPosition = Vector3.zero;
        }

        public override void Update(NewEnemyBehaviour newEnemy)
        {
            time += Time.deltaTime;

            if (Vector3.Distance(newEnemy.transform.position, newEnemy.Player.position) <= newEnemy.CloseDistance && time <= 0.83f)
            {
                newEnemy.Animator.SetBool("isAttacking", false);

                newEnemy.AILerp.enabled = true;

                newEnemy.TransitionToState(newEnemy.FleeState);
            }
            else if (time > 0.83f && time <= 1.167f)
            {
                if (lastPlayerPosition == Vector3.zero)
                {
                    lastPlayerPosition = newEnemy.Player.position;
                }

                newEnemy.transform.Translate((lastPlayerPosition - myLastPosition).normalized * newEnemy.AttackSpeed * Time.deltaTime);
            }
            else if (time > 1.167f && Vector3.Distance(newEnemy.transform.position, newEnemy.Player.position) <= newEnemy.InteriorDistance)
            {
                newEnemy.Animator.SetBool("isAttacking", false);

                newEnemy.AILerp.enabled = true;

                newEnemy.TransitionToState(newEnemy.FleeState);
            }
            else if (time > 1.167f && Vector3.Distance(newEnemy.transform.position, newEnemy.Player.position) > newEnemy.InteriorDistance)
            {
                newEnemy.Animator.SetBool("isAttacking", false);

                newEnemy.AILerp.enabled = true;

                newEnemy.TransitionToState(newEnemy.ChaseState);
            }
        }

        public override void OnCollisionEnter2D(NewEnemyBehaviour newEnemy, Collision2D collision)
        {
            if (newEnemy.enabled)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();

                    if (player.currentEnumState == PlayerState.Block)
                    {
                        newEnemy.Animator.SetBool("isAttacking", false);

                        // Player show sparks
                        collision.gameObject.GetComponent<PlayerBehaviour>().TakeDamage(newEnemy.AttackDamage);

                        newEnemy.TransitionToState(newEnemy.StunnedState);
                    }
                    else
                    {
                        newEnemy.Animator.SetBool("isAttacking", false);

                        collision.gameObject.GetComponent<PlayerBehaviour>().TakeDamage(newEnemy.AttackDamage);

                        newEnemy.AILerp.enabled = true;

                        newEnemy.TransitionToState(newEnemy.FleeState);
                    }
                }
                else if (collision.gameObject.CompareTag("Collision"))
                {
                    newEnemy.Animator.SetBool("isAttacking", false);

                    newEnemy.TransitionToState(newEnemy.StunnedState);
                }
            }
        }
    }
}