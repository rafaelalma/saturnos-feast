using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class NewEnemy : Enemy
    {
        private NewEnemyBehaviour behaviour;

        private void Awake()
        {
            behaviour = GetComponent<NewEnemyBehaviour>();
        }

        protected override void OnEnable()
        {
            behaviour.enabled = true;
            behaviour.AILerp.enabled = true;

            base.OnEnable();
        }

        public override void TakeDamage(int damage)
        {
            if (CurrentHealth > 0)
            {
                behaviour.AudioSource.PlayOneShot(behaviour.TakeDamageSound, 0.4f);

                StartCoroutine(ShowBloodParticles("YellowBloodParticles"));

                base.TakeDamage(damage);
            }
        }

        protected override IEnumerator Die()
        {
            behaviour.Animator.SetTrigger("isDead");

            behaviour.SpriteRenderer.color = Color.white;
            behaviour.AILerp.enabled = false;
            behaviour.enabled = false;

            GameManager.Instance.CurrentAmountOfEnemies--;
            GameManager.Instance.EnemiesKilled++;

            return base.Die();
        }
    }
}