using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class BlueDoor : Door
    {
        protected override void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerBehaviour>().HasBlueKey)
            {
                OpenDoor();
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CloseDoor();
            }
        }
    }
}