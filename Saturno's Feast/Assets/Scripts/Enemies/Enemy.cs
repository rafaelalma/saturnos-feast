using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace RefactoredProject
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int dnaValue;

        public int CurrentHealth { get; private set; }

        protected virtual void OnEnable()
        {
            CurrentHealth = maxHealth;
        }

        public virtual void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;

                StartCoroutine(Die());
            }
        }

        protected IEnumerator ShowBloodParticles(string bloodParticlesTag)
        {
            GameObject bloodParticles = ObjectPooling.Instance.GetAvailableObject(bloodParticlesTag);
            bloodParticles.transform.position = transform.position;
            bloodParticles.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
            bloodParticles.SetActive(true);

            yield return new WaitForSeconds(0.167f);
            bloodParticles.SetActive(false);
        }

        protected virtual IEnumerator Die()
        {
            GameManager.Instance.UpdateCombo();
            GameManager.Instance.AddDNA(dnaValue);

            yield return new WaitForSeconds(10.0f);
            gameObject.SetActive(false);
        }
    }
}