using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class DoorSound : MonoBehaviour
    {
        [SerializeField] private AudioClip doorSound;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound()
        {
            audioSource.PlayOneShot(doorSound, 0.2f);
        }
    }
}