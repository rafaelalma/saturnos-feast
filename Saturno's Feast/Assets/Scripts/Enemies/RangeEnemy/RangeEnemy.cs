using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class RangeEnemy : Enemy
    {
        private RangeEnemyBehaviour behaviour;

        private void Awake()
        {
            behaviour = GetComponent<RangeEnemyBehaviour>();
        }

        protected override void OnEnable()
        {
            behaviour.enabled = true;

            base.OnEnable();
        }

        public override void TakeDamage(int damage)
        {
            if (CurrentHealth > 0)
            {
                behaviour.AudioSource.PlayOneShot(behaviour.TakeDamageSound, 0.4f);

                StartCoroutine(ShowBloodParticles("BlueBloodParticles"));

                base.TakeDamage(damage);
            }
        }

        protected override IEnumerator Die()
        {
            behaviour.Animator.SetTrigger("isDead");

            behaviour.enabled = false;

            GameManager.Instance.CurrentAmountOfEnemies--;
            GameManager.Instance.EnemiesKilled++;

            return base.Die();
        }
    }
}