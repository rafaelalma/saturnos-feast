using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BasicEnemy : Enemy
    {
        private BasicEnemyBehaviour behaviour;

        private void Awake()
        {
            behaviour = GetComponent<BasicEnemyBehaviour>();
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
                behaviour.AudioSource.PlayOneShot(behaviour.TakeDamageSound, 0.6f);

                StartCoroutine(ShowBloodParticles("PurpleBloodParticles"));

                base.TakeDamage(damage);
            }
        }

        protected override IEnumerator Die()
        {
            behaviour.Animator.SetBool("isRunning", false);
            behaviour.Animator.SetTrigger("isDead");
            behaviour.AILerp.enabled = false;
            behaviour.enabled = false;

            GameObject projectilePickup = ObjectPooling.Instance.GetAvailableObject("ProjectilePickup");
            projectilePickup.transform.position = transform.position;
            projectilePickup.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            projectilePickup.SetActive(true);

            GameManager.Instance.CurrentAmountOfEnemies--;
            GameManager.Instance.EnemiesKilled++;

            return base.Die();
        }
    }
}