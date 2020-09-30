using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class FourthEvent : MonoBehaviour
    {
        private BoxCollider2D boxCollider;

        public delegate void PlayerEnterQueenLair();
        public static event PlayerEnterQueenLair OnPlayerEnterQueenLair;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            PlayerDeadState.OnPlayerDeath += EnableTrigger;
        }

        private void OnDisable()
        {
            PlayerDeadState.OnPlayerDeath -= EnableTrigger;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                OnPlayerEnterQueenLair?.Invoke();

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
    }
}