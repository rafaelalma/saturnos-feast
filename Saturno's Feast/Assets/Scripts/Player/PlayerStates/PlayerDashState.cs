using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerDashState : PlayerBaseState
    {
        private float time;
        private Vector3 lastMoveDirection;

        public override void EnterState(PlayerBehaviour player)
        {
            player.currentEnumState = PlayerState.Dash;

            time = 0.0f;

            player.Animator.SetBool("isDashing", true);

            lastMoveDirection = player.MoveDirection;

            player.TimeSinceLastDash = 0.0f;

            CanvasManager.Instance.UpdateDash(false);
        }

        public override void ShouldTransitionToTakeDamageState(PlayerBehaviour player)
        {
            return;
        }

        public override void Update(PlayerBehaviour player)
        {
            time += Time.deltaTime;

            if (!player.PausedGame)
            {
                if (time >= 0.667f)
                {
                    TransitionToRunState(player, "isDashing");
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    TransitionToMeleeAttackState(player, "isDashing");
                }
                else if (Input.GetMouseButtonDown(1) && player.CurrentProjectiles > 0)
                {
                    TransitionToStrongRangeAttackState(player, "isDashing");
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    TransitionToBlockState(player, "isDashing");
                }
                else
                {
                    TryToMove(player, lastMoveDirection, player.DashSpeed * Time.deltaTime);
                }
            }
        }

        private void TryToMove(PlayerBehaviour player, Vector3 moveDirection, float distance)
        {
            bool canMove = CanMove(player, moveDirection, distance);

            if (!canMove)
            {
                TransitionToIdleState(player, "isDashing");
            }
            else
            {
                player.transform.Translate(moveDirection * distance);
            }
        }

        private bool CanMove(PlayerBehaviour player, Vector3 direction, float distance)
        {
            return Physics2D.Raycast(player.transform.position, direction, distance, player.ObstacleLayer).collider == null;
        }
    }
}