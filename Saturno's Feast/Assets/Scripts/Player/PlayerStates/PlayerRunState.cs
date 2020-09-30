using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerRunState : PlayerBaseState
    {
        public override void EnterState(PlayerBehaviour player)
        {
            player.currentEnumState = PlayerState.Run;

            player.Animator.SetBool("isRunning", true);
        }

        public override void ShouldTransitionToTakeDamageState(PlayerBehaviour player)
        {
            TransitionToTakeDamageState(player, "isRunning");
        }

        public override void Update(PlayerBehaviour player)
        {
            if (!player.PausedGame)
            {
                if (player.MoveDirection == Vector3.zero)
                {
                    TransitionToIdleState(player, "isRunning");
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    TransitionToMeleeAttackState(player, "isRunning");
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    TransitionToBlockState(player, "isRunning");
                }
                else if (Input.GetKeyDown(KeyCode.LeftShift) && player.TimeSinceLastDash >= player.DashCooldown)
                {
                    TransitionToDashState(player, "isRunning");
                }
                else if (Input.GetMouseButtonDown(1) && player.CurrentProjectiles > 0)
                {
                    TransitionToStrongRangeAttackState(player, "isRunning");
                }
                else
                {
                    TryToMove(player, player.MoveDirection, player.Speed * Time.deltaTime);
                }
            }
        }

        private void TryToMove(PlayerBehaviour player, Vector3 baseMoveDirection, float distance)
        {
            Vector3 moveDirection = baseMoveDirection;
            bool canMove = CanMove(player, moveDirection, distance);

            if (!canMove)
            {
                player.Animator.SetBool("isRunning", false);

                moveDirection = new Vector3(baseMoveDirection.x, 0, 0).normalized;
                canMove = (moveDirection.x != 0 && CanMove(player, moveDirection, distance));

                if (!canMove)
                {
                    player.Animator.SetBool("isRunning", false);

                    moveDirection = new Vector3(0, baseMoveDirection.y, 0).normalized;
                    canMove = (moveDirection.y != 0 && CanMove(player, moveDirection, distance));

                    if (canMove)
                    {
                        Move(player, moveDirection, distance);
                    }
                }
                else
                {
                    Move(player, moveDirection, distance);
                }
            }
            else
            {
                Move(player, moveDirection, distance);
            }
        }

        private bool CanMove(PlayerBehaviour player, Vector3 direction, float distance)
        {
            return Physics2D.Raycast(player.transform.position, direction, distance, player.ObstacleLayer).collider == null;
        }

        private void Move(PlayerBehaviour player, Vector3 moveDirection, float distance)
        {
            player.Animator.SetBool("isRunning", true);

            player.transform.Translate(moveDirection * distance);
        }
    }
}