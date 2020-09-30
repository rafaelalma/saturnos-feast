using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RefactoredProject
{
    public class ThirdEvent : MonoBehaviour
    {
        private BoxCollider2D boxCollider;

        public delegate void PlayerEnterQueenRoom();
        public static event PlayerEnterQueenRoom OnPlayerEnterQueenRoom;

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
                OnPlayerEnterQueenRoom?.Invoke();

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