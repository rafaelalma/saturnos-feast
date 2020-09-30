using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerIdleState : PlayerBaseState
    {
        public override void EnterState(PlayerBehaviour player)
        {
            player.currentEnumState = PlayerState.Idle;
        }

        public override void ShouldTransitionToTakeDamageState(PlayerBehaviour player)
        {
            player.TransitionToState(player.TakeDamageState);
        }

        public override void Update(PlayerBehaviour player)
        {
            if (!player.PausedGame)
            {
                if (player.MoveDirection != Vector3.zero)
                {
                    player.TransitionToState(player.RunState);
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    player.TransitionToState(player.MeleeAttackState);
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    player.TransitionToState(player.BlockState);
                }
                else if (Input.GetMouseButtonDown(1) && player.CurrentProjectiles > 0)
                {
                    player.TransitionToState(player.StrongRangeAttackState);
                }
            }
        }
    }
}