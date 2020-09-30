using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class QueenTail : Enemy
    {
        [SerializeField] private Events eventToSuscribe;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private float timeBetweenSpawns;
        [SerializeField] private int amountToSpawn;
        [SerializeField] private int maxAmountToSpawn;

        [Header("Sound")]
        [SerializeField] private AudioClip takeDamageSound;

        private int currentAmountSpawned;
        private IEnumerator spawnBasicEnemy;
        private Animator animator;
        private BoxCollider2D boxCollider;
        private AudioSource audioSource;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            boxCollider = GetComponent<BoxCollider2D>();

            audioSource = GetComponent<AudioSource>();
        }

        protected override void OnEnable()
        {
            SuscribeToEvents();

            base.OnEnable();
        }

        private void OnDisable()
        {
            UnsuscribeFromEvents();
        }

        public override void TakeDamage(int damage)
        {
            if (CurrentHealth > 0)
            {
                audioSource.PlayOneShot(takeDamageSound, 0.4f);

                StartCoroutine(ShowBloodParticles("BigGreenBloodParticles"));

                base.TakeDamage(damage);
            }
        }

        private void CallSpawnBasicEnemy()
        {
            if (currentAmountSpawned < maxAmountToSpawn && GameManager.Instance.CurrentAmountOfEnemies < 500)
            {
                spawnBasicEnemy = SpawnBasicEnemy();
                StartCoroutine(spawnBasicEnemy);
            }
        }

        private IEnumerator SpawnBasicEnemy()
        {
            animator.SetTrigger("spawn");

            yield return new WaitForSeconds(1.25f);

            for (int i = 0; i < amountToSpawn; i++)
            {
                GameObject basicEnemy = ObjectPooling.Instance.GetAvailableObject("BasicEnemy");

                basicEnemy.transform.position = spawnPosition.position;
                basicEnemy.transform.rotation = Quaternion.identity;

                basicEnemy.SetActive(true);

                currentAmountSpawned++;

                GameManager.Instance.CurrentAmountOfEnemies++;
            }

            if (timeBetweenSpawns < 1.25f)
            {
                timeBetweenSpawns = 1.25f;
            }
            float realTimeBetweenSpawns = timeBetweenSpawns - 1.25f;

            yield return new WaitForSeconds(realTimeBetweenSpawns);

            if (currentAmountSpawned < maxAmountToSpawn && GameManager.Instance.CurrentAmountOfEnemies < 500)
            {
                spawnBasicEnemy = SpawnBasicEnemy();
                StartCoroutine(spawnBasicEnemy);
            }
        }

        private void StopSpawnBasicEnemy()
        {
            if (spawnBasicEnemy != null)
            {
                StopCoroutine(spawnBasicEnemy);
            }
        }

        protected override IEnumerator Die()
        {
            boxCollider.enabled = false;

            if (spawnBasicEnemy != null)
            {
                StopCoroutine(spawnBasicEnemy);
            }

            UnsuscribeFromEvents();

            animator.SetTrigger("die");

            GameManager.Instance.UpdateCombo();
            GameManager.Instance.AddDNA(dnaValue);

            yield return null;
        }

        private void SuscribeToEvents()
        {
            switch (eventToSuscribe)
            {
                case Events.FirstEvent:
                    FirstEvent.OnPlayerEnterBedroom += CallSpawnBasicEnemy;
                    break;
                case Events.SecondEvent:
                    SecondEvent.OnPlayerEnterCentralRoom += CallSpawnBasicEnemy;
                    break;
                case Events.ThirdEvent:
                    ThirdEvent.OnPlayerEnterQueenRoom += CallSpawnBasicEnemy;
                    break;
                case Events.FourthEvent:
                    FourthEvent.OnPlayerEnterQueenLair += CallSpawnBasicEnemy;
                    break;
            }

            PlayerDeadState.OnPlayerDeath += StopSpawnBasicEnemy;
        }

        private void UnsuscribeFromEvents()
        {
            switch (eventToSuscribe)
            {
                case Events.FirstEvent:
                    FirstEvent.OnPlayerEnterBedroom -= CallSpawnBasicEnemy;
                    break;
                case Events.SecondEvent:
                    SecondEvent.OnPlayerEnterCentralRoom -= CallSpawnBasicEnemy;
                    break;
                case Events.ThirdEvent:
                    ThirdEvent.OnPlayerEnterQueenRoom -= CallSpawnBasicEnemy;
                    break;
                case Events.FourthEvent:
                    FourthEvent.OnPlayerEnterQueenLair -= CallSpawnBasicEnemy;
                    break;
            }

            PlayerDeadState.OnPlayerDeath -= StopSpawnBasicEnemy;
        }
    }
}