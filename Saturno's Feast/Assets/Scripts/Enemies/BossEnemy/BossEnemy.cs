using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BossEnemy : Enemy
    {
        private BossEnemyBehaviour behaviour;

        public delegate void BossEnemyDeath();
        public static event BossEnemyDeath OnBossEnemyDeath;

        private void Awake()
        {
            behaviour = GetComponent<BossEnemyBehaviour>();
        }

        protected override void OnEnable()
        {
            behaviour.enabled = true;

            PlayerDeadState.OnPlayerDeath += ResetBossEnemy;

            base.OnEnable();
        }

        private void OnDisable()
        {
            PlayerDeadState.OnPlayerDeath -= ResetBossEnemy;
        }

        public override void TakeDamage(int damage)
        {
            if (CurrentHealth > 0 && behaviour.currentEnumState != BossEnemyState.Charge)
            {
                behaviour.AudioSource.PlayOneShot(behaviour.TakeDamageSound, 0.4f);

                StartCoroutine(ShowBloodParticles("GreenBloodParticles"));

                base.TakeDamage(damage);
            }
        }

        protected override IEnumerator Die()
        {
            OnBossEnemyDeath?.Invoke();

            behaviour.Animator.SetTrigger("isDead");

            behaviour.enabled = false;
            behaviour.AILerp.enabled = false;

            return base.Die();
        }

        private void ResetBossEnemy()
        {
            if(gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }
    }
}