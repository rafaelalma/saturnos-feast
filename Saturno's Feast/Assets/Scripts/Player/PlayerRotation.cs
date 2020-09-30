using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private PlayerBehaviour playerBehaviour;
        private bool pausedGame;

        private void Awake()
        {
            playerBehaviour = player.GetComponent<PlayerBehaviour>();
        }

        private void OnEnable()
        {
            GameManager.OnPause += GameIsPaused;
            GameManager.OnUnpause += GameIsUnpaused;
        }

        private void Update()
        {
            if (!pausedGame)
            {
                Rotate();
            }
        }

        private void OnDisable()
        {
            GameManager.OnPause -= GameIsPaused;
            GameManager.OnUnpause -= GameIsUnpaused;
        }

        private void GameIsPaused()
        {
            pausedGame = true;
        }

        private void GameIsUnpaused()
        {
            pausedGame = false;
        }

        private void Rotate()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lookDirection = mousePosition - transform.position;
            lookDirection.Normalize();
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            playerBehaviour.Animator.SetFloat("angle", angle);
        }
    }
}