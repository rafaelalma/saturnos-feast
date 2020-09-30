using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class RangeEnemySpawn : MonoBehaviour
    {
        [SerializeField] private Events eventToSuscribe;
        [SerializeField] private float spawnRadius;
        [SerializeField] private float minTime, maxTime;
        [SerializeField] private int maxAmountToSpawn;

        private int currentAmountSpawned;
        private IEnumerator spawnRangeEnemy;

        private void OnEnable()
        {
            SuscribeToEvents();
        }

        private void OnDisable()
        {
            UnsuscribeFromEvents();
        }

        private void CallSpawnRangeEnemy()
        {
            if (currentAmountSpawned < maxAmountToSpawn && GameManager.Instance.CurrentAmountOfEnemies < 500)
            {
                spawnRangeEnemy = SpawnRangeEnemy();
                StartCoroutine(spawnRangeEnemy);
            }
        }

        private IEnumerator SpawnRangeEnemy()
        {
            Vector3 spawnPosition = Random.insideUnitCircle * spawnRadius;

            GameObject rangeEnemy = ObjectPooling.Instance.GetAvailableObject("RangeEnemy");

            RangeEnemyBehaviour rangeEnemyBehaviour = rangeEnemy.GetComponent<RangeEnemyBehaviour>();

            rangeEnemyBehaviour.SpawnPosition = transform;
            rangeEnemyBehaviour.MoveRadius = spawnRadius;

            rangeEnemy.transform.position = transform.position + spawnPosition;
            rangeEnemy.transform.rotation = Quaternion.identity;

            rangeEnemy.SetActive(true);

            currentAmountSpawned++;

            GameManager.Instance.CurrentAmountOfEnemies++;

            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            if (currentAmountSpawned < maxAmountToSpawn && GameManager.Instance.CurrentAmountOfEnemies < 500)
            {
                spawnRangeEnemy = SpawnRangeEnemy();
                StartCoroutine(spawnRangeEnemy);
            }
        }

        private void StopSpawnRangeEnemy()
        {
            if (spawnRangeEnemy != null)
            {
                StopCoroutine(spawnRangeEnemy);
            }
        }

        private void SuscribeToEvents()
        {
            switch (eventToSuscribe)
            {
                case Events.FirstEvent:
                    FirstEvent.OnPlayerEnterBedroom += CallSpawnRangeEnemy;
                    break;
                case Events.SecondEvent:
                    SecondEvent.OnPlayerEnterCentralRoom += CallSpawnRangeEnemy;
                    break;
                case Events.ThirdEvent:
                    ThirdEvent.OnPlayerEnterQueenRoom += CallSpawnRangeEnemy;
                    break;
                case Events.FourthEvent:
                    FourthEvent.OnPlayerEnterQueenLair += CallSpawnRangeEnemy;
                    break;
            }

            PlayerDeadState.OnPlayerDeath += StopSpawnRangeEnemy;
        }

        private void UnsuscribeFromEvents()
        {
            switch (eventToSuscribe)
            {
                case Events.FirstEvent:
                    FirstEvent.OnPlayerEnterBedroom -= CallSpawnRangeEnemy;
                    break;
                case Events.SecondEvent:
                    SecondEvent.OnPlayerEnterCentralRoom -= CallSpawnRangeEnemy;
                    break;
                case Events.ThirdEvent:
                    ThirdEvent.OnPlayerEnterQueenRoom -= CallSpawnRangeEnemy;
                    break;
                case Events.FourthEvent:
                    FourthEvent.OnPlayerEnterQueenLair -= CallSpawnRangeEnemy;
                    break;
            }

            PlayerDeadState.OnPlayerDeath -= StopSpawnRangeEnemy;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}