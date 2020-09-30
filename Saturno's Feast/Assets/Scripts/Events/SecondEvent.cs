using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class SecondEvent : MonoBehaviour
    {
        private BoxCollider2D boxCollider;

        public delegate void PlayerEnterCentralRoom();
        public static event PlayerEnterCentralRoom OnPlayerEnterCentralRoom;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            PlayerDeadState.OnPlayerDeath += EnableTrigger;
            BossEnemy.OnBossEnemyDeath += UnsuscribeFromOnPlayerRespawn;
        }

        private void OnDisable()
        {
            PlayerDeadState.OnPlayerDeath -= EnableTrigger;
            BossEnemy.OnBossEnemyDeath -= UnsuscribeFromOnPlayerRespawn;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnPlayerEnterCentralRoom?.Invoke();

                boxCollider.enabled = false;
            }
        }

        private void EnableTrigger()
        {
            if (!boxCollider.enabled)
            {
                boxCollider.enabled = true;
            }
        }

        private void UnsuscribeFromOnPlayerRespawn()
        {
            PlayerDeadState.OnPlayerDeath -= EnableTrigger;
        }
    }
}