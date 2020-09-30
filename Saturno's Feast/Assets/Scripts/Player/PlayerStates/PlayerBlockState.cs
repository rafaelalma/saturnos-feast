using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerBlockState : PlayerBaseState
    {
        public override void EnterState(PlayerBehaviour player)
        {
            player.currentEnumState = PlayerState.Block;

            player.Animator.SetBool("isBlocking", true);
        }

        public override void ShouldTransitionToTakeDamageState(PlayerBehaviour player)
        {
            player.StartCoroutine(ShowSparks(player));
        }

        public override void Update(PlayerBehaviour player)
        {
            if (Input.GetKeyUp(KeyCode.Space) && !player.PausedGame)
            {
                TransitionToIdleState(player, "isBlocking");
            }
        }

        private IEnumerator ShowSparks(PlayerBehaviour player)
        {
            GameObject sparks = ObjectPooling.Instance.GetAvailableObject("Sparks");
            sparks.transform.position = player.transform.position;
            sparks.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));

            sparks.SetActive(true);

            player.AudioSource.PlayOneShot(player.BlockSound,0.2f);

            yield return new WaitForSeconds(0.167f);
            sparks.SetActive(false);
        }
    }
}