using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class PlayerProjectile : MonoBehaviour
    {
        public float Speed { get; set; }
        public int Damage { get; set; }

        private Vector3 direction;

        private void OnEnable()
        {
            direction = Vector3.up;
        }

        private void Update()
        {
            transform.Translate(direction * Speed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Collision"))
            {
                gameObject.SetActive(false);
            }
            else if (collision.gameObject.layer == 10)
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                if (enemy.CurrentHealth > 0)
                {
                    gameObject.SetActive(false);
                }

                enemy.TakeDamage(Damage);
            }
            else if (collision.gameObject.layer == 12)
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                if (enemy.CurrentHealth > 0)
                {
                    gameObject.SetActive(false);
                }

                enemy.TakeDamage(Damage);
            }
            else if (collision.gameObject.CompareTag("EnemyProjectile"))
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}