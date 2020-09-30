using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float smoothing = 5.0f;

        private Vector3 offset;
        private Vector3 cameraPosition;

        void Start()
        {
            offset = transform.position - player.position;
        }

        private void LateUpdate()
        {
            cameraPosition = player.position + offset;

            transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothing * Time.deltaTime);
        }
    }
}