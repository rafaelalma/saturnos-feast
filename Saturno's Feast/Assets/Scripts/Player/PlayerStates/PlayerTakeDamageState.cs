using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerTakeDamageState : PlayerBaseState
    {
        private float time;

        public override void EnterState(PlayerBehaviour player)
        {
            player.currentEnumState = PlayerState.TakeDamage;

            time = 0.0f;

            player.Animator.SetBool("isTakingDamage", true);

            MustTakeDamage(player);
        }

        public override void ShouldTransitionToTakeDamageState(PlayerBehaviour player)
        {
            return;
        }

        public override void Update(PlayerBehaviour player)
        {
            time += Time.deltaTime;

            if (time >= 0.167f)
            {
                if (player.CurrentHealth <= 0)
                {
                    player.CurrentHealth = 0;

                    TransitionToDeadState(player, "isTakingDamage");
                }
                else
                {
                    TransitionToIdleState(player, "isTakingDamage");
                }
            }
        }

        public void MustTakeDamage(PlayerBehaviour player)
        {
            player.StartCoroutine(ShowBloodParticles(player));

            player.CurrentHealth -= player.DamageTaken;
            player.DamageTaken = 0;

            player.AudioSource.PlayOneShot(player.TakeDamageSound, 0.5f);

            GameManager.Instance.ResetCombo();

            CanvasManager.Instance.UpdateLifebar(player.CurrentHealth, player.MaxHealth);
        }

        private IEnumerator ShowBloodParticles(PlayerBehaviour player)
        {
            GameObject bloodParticles = ObjectPooling.Instance.GetAvailableObject("RedBloodParticles");
            bloodParticles.transform.position = player.transform.position;
            bloodParticles.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            bloodParticles.SetActive(true);

            yield return new WaitForSeconds(0.167f);
            bloodParticles.SetActive(false);
        }
    }
}