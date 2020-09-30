using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class FirstEvent : MonoBehaviour
    {
        private BoxCollider2D boxCollider;

        public delegate void PlayerEnterBedroom();
        public static event PlayerEnterBedroom OnPlayerEnterBedroom;

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
                OnPlayerEnterBedroom?.Invoke();

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