using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class CorruptionDoor : Door
    {
        private void OnEnable()
        {
            SecondEvent.OnPlayerEnterCentralRoom += CloseDoor;
            PlayerDeadState.OnPlayerDeath += OpenDoor;
            BossEnemy.OnBossEnemyDeath += UnsuscribeFromOnPlayerRespawn;
        }

        private void Start()
        {
            OpenDoor();
        }

        private void OnDisable()
        {
            SecondEvent.OnPlayerEnterCentralRoom -= CloseDoor;
            PlayerDeadState.OnPlayerDeath -= OpenDoor;
            BossEnemy.OnBossEnemyDeath -= UnsuscribeFromOnPlayerRespawn;
        }

        protected override void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerBehaviour>().HasMitigateCorruptionPower)
            {
                OpenDoor();
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CloseDoor();
            }
        }

        private void UnsuscribeFromOnPlayerRespawn()
        {
            PlayerDeadState.OnPlayerDeath -= OpenDoor;
        }
    }
}