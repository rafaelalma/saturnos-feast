using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RefactoredProject
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject creature;

        public void Start()
        {
            Time.timeScale = 1;

            creature = GameObject.Find("Creature");
        }

        IEnumerator WaitForAnimToEnd()
        {
            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene(1);
        }
        public void StartGame()
        {
            StartCoroutine(WaitForAnimToEnd());
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}