using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class RangeEnemyRotation : MonoBehaviour
    {
        private RangeEnemyBehaviour behaviour;
        private Transform player;
        private float angle;

        private void Awake()
        {
            behaviour = transform.parent.GetComponent<RangeEnemyBehaviour>();
        }

        private void Start()
        {
            player = behaviour.Player;
        }

        private void Update()
        {
            Rotate();

            behaviour.Animator.SetFloat("angle", angle);
        }

        private void Rotate()
        {
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.Normalize();
            angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}