using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class EnemyProjectile : MonoBehaviour
    {
        public float Speed { get; set; }
        public int Damage { get; set; }

        private void Update()
        {
            transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Collision"))
            {
                gameObject.SetActive(false);
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerBehaviour>().TakeDamage(Damage);

                gameObject.SetActive(false);
            }
        }
    }
}