using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RefactoredProject
{
    public class CanvasManager : MonoBehaviour
    {
        private static CanvasManager instance;

        public static CanvasManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("The CanvasManager is NULL.");
                }

                return instance;
            }
        }

        [SerializeField] private Image lifebarImage, dashImage, projectilesImage;
        [SerializeField] private Image blueKey, whiteKey, blackKey;
        [SerializeField] private Text projectilesText, dnaText, comboText;
        [SerializeField] private Sprite dashIsActive, dashIsNotActive;
        [SerializeField] private Sprite projectilesAreActive, projectilesAreNotActive;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            blueKey.enabled = false;
            whiteKey.enabled = false;
            blackKey.enabled = false;
        }

        public void UpdateLifebar(float currentHealth, float maxHealth)
        {
            lifebarImage.fillAmount = currentHealth / maxHealth;
        }

        public void UpdateDash(bool dashIsActive)
        {
            if (dashIsActive)
            {
                dashImage.sprite = this.dashIsActive;
            }
            else
            {
                dashImage.sprite = dashIsNotActive;
            }
        }

        public void UpdateProjectiles(int currentProjectiles)
        {
            projectilesText.text = currentProjectiles.ToString();

            if (currentProjectiles > 0)
            {
                projectilesImage.sprite = projectilesAreActive;
            }
            else
            {
                projectilesImage.sprite = projectilesAreNotActive;
            }
        }

        public void UpdateDNA(int dna)
        {
            dnaText.text = dna.ToString();
        }

        public void UpdateCombo(int combo)
        {
            if (combo > 0)
            {
                comboText.text = combo.ToString();
            }
            else
            {
                comboText.text = "";
            }
        }

        public void ShowBlueKey()
        {
            blueKey.enabled = true;
        }

        public void ShowWhiteKey()
        {
            whiteKey.enabled = true;
        }

        public void ShowBlackKey()
        {
            blackKey.enabled = true;
        }
    }
}