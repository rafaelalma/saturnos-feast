using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class InitialSpawn : MonoBehaviour
    {
        [SerializeField] private EnemyToSpawnInitially[] initialEnemiesToSpawn;

        private void Start()
        {
            InitialSpawnOfEnemies();
        }

        private void InitialSpawnOfEnemies()
        {
            foreach (var initialEnemyToSpawn in initialEnemiesToSpawn)
            {
                for (int i = 0; i < initialEnemyToSpawn.initialAmount; i++)
                {
                    GameObject enemy = ObjectPooling.Instance.GetAvailableObject(initialEnemyToSpawn.tag);

                    enemy.transform.position = initialEnemyToSpawn.spawnPosition.position;
                    enemy.transform.rotation = Quaternion.identity;

                    enemy.SetActive(true);

                    GameManager.Instance.CurrentAmountOfEnemies++;
                }
            }
        }
    }
}