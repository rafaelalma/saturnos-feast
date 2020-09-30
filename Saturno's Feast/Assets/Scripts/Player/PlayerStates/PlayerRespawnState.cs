using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerRespawnState : PlayerBaseState
    {
        private float time;

        public override void EnterState(PlayerBehaviour player)
        {
            player.currentEnumState = PlayerState.Respawn;

            player.transform.position = player.RespawnPosition.position;

            player.BoxCollider.enabled = true;

            time = 0.0f;

            ResetHealthAndProjectiles(player);
        }

        public override void ShouldTransitionToTakeDamageState(PlayerBehaviour player)
        {
            return;
        }

        public override void Update(PlayerBehaviour player)
        {
            time += Time.deltaTime;

            if (time >= 2.0f)
            {
                player.TransitionToState(player.IdleState);
            }
        }

        private void ResetHealthAndProjectiles(PlayerBehaviour player)
        {
            player.CurrentHealth = player.MaxHealth;
            player.CurrentProjectiles = player.MaxProjectiles;

            CanvasManager.Instance.UpdateLifebar(player.CurrentHealth, player.MaxHealth);
            CanvasManager.Instance.UpdateProjectiles(player.CurrentProjectiles);
        }
    }
}