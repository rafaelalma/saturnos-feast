using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerDeadState : PlayerBaseState
    {
        private float time;

        public delegate void PlayerDeath();
        public static event PlayerDeath OnPlayerDeath;

        public override void EnterState(PlayerBehaviour player)
        {
            player.currentEnumState = PlayerState.Dead;

            player.BoxCollider.enabled = false;

            OnPlayerDeath?.Invoke();

            GameManager.Instance.PlayerDeaths++;

            time = 0.0f;

            player.Animator.SetBool("isDead", true);
        }

        public override void ShouldTransitionToTakeDamageState(PlayerBehaviour player)
        {
            return;
        }

        public override void Update(PlayerBehaviour player)
        {
            time += Time.deltaTime;

            if (time >= 1.5f)
            {
                TransitionToRespawnState(player, "isDead");
            }
        }
    }
}