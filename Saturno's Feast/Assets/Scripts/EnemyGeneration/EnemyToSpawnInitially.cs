using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    [System.Serializable]
    public class EnemyToSpawnInitially
    {
        public string tag;
        public Transform spawnPosition;
        public int initialAmount;

        public EnemyToSpawnInitially(string tag, int initialAmount, Transform spawnPosition)
        {
            this.tag = tag;
            this.initialAmount = initialAmount;
            this.spawnPosition = spawnPosition;
        }
    }
}