using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class QueenEgg : MonoBehaviour
    {
        [SerializeField] private Events eventToSuscribe;
        [SerializeField] private float minTime, maxTime;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            SuscribeToEvents();
        }

        private void OnDisable()
        {
            UnsuscribeFromEvents();
        }

        private void InvokeSpawnNewEnemy()
        {
            Invoke("SpawnNewEnemy", Random.Range(minTime, maxTime));
        }

        private void SpawnNewEnemy()
        {
            if(GameManager.Instance.CurrentAmountOfEnemies < 500)
            {
                animator.SetTrigger("spawn");

                StartCoroutine(SpawnNewEnemyRoutine());
            }
        }

        private IEnumerator SpawnNewEnemyRoutine()
        {
            yield return new WaitForSeconds(1.5f);

            GameObject newEnemy = ObjectPooling.Instance.GetAvailableObject("NewEnemy");

            newEnemy.transform.position = transform.position;
            newEnemy.transform.rotation = Quaternion.identity;

            newEnemy.SetActive(true);

            GameManager.Instance.CurrentAmountOfEnemies++;

            gameObject.SetActive(false);
        }

        private void CancelInvokeSpawnNewEnemy()
        {
            CancelInvoke();
        }

        private void SuscribeToEvents()
        {
            switch (eventToSuscribe)
            {
                case Events.FirstEvent:
                    FirstEvent.OnPlayerEnterBedroom += InvokeSpawnNewEnemy;
                    break;
                case Events.SecondEvent:
                    SecondEvent.OnPlayerEnterCentralRoom += InvokeSpawnNewEnemy;
                    break;
                case Events.ThirdEvent:
                    ThirdEvent.OnPlayerEnterQueenRoom += InvokeSpawnNewEnemy;
                    break;
                case Events.FourthEvent:
                    FourthEvent.OnPlayerEnterQueenLair += InvokeSpawnNewEnemy;
                    break;
            }

            PlayerDeadState.OnPlayerDeath += CancelInvokeSpawnNewEnemy;
        }

        private void UnsuscribeFromEvents()
        {
            switch (eventToSuscribe)
            {
                case Events.FirstEvent:
                    FirstEvent.OnPlayerEnterBedroom -= InvokeSpawnNewEnemy;
                    break;
                case Events.SecondEvent:
                    SecondEvent.OnPlayerEnterCentralRoom -= InvokeSpawnNewEnemy;
                    break;
                case Events.ThirdEvent:
                    ThirdEvent.OnPlayerEnterQueenRoom -= InvokeSpawnNewEnemy;
                    break;
                case Events.FourthEvent:
                    FourthEvent.OnPlayerEnterQueenLair -= InvokeSpawnNewEnemy;
                    break;
            }

            PlayerDeadState.OnPlayerDeath -= CancelInvokeSpawnNewEnemy;
        }
    }
}