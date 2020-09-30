using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class Door : MonoBehaviour
    {
        private BoxCollider2D doorCollider;
        private Animator animator;

        private void Awake()
        {
            animator = transform.parent.GetComponent<Animator>();
            doorCollider = transform.parent.GetComponent<BoxCollider2D>();
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == 10)
            {
                OpenDoor();
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == 10)
            {
                CloseDoor();
            }
        }

        protected void OpenDoor()
        {
            if (doorCollider.enabled)
            {
                animator.SetBool("isOpen", true);
                doorCollider.enabled = false;
            }
        }

        protected void CloseDoor()
        {
            if (!doorCollider.enabled)
            {
                animator.SetBool("isOpen", false);
                doorCollider.enabled = true;
            }
        }
    }
}