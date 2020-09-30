using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BossEnemySpawn : MonoBehaviour
    {
        [SerializeField] private Transform bossEnemySpawnPosition;

        private void OnEnable()
        {
            SecondEvent.OnPlayerEnterCentralRoom += SpawnBossEnemy;
        }

        private void OnDisable()
        {
            SecondEvent.OnPlayerEnterCentralRoom -= SpawnBossEnemy;
        }

        private void SpawnBossEnemy()
        {
            GameObject bossEnemy = ObjectPooling.Instance.GetAvailableObject("BossEnemy");

            bossEnemy.transform.position = bossEnemySpawnPosition.position;
            bossEnemy.transform.rotation = Quaternion.identity;

            bossEnemy.SetActive(true);
        }
    }
}