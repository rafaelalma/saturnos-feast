using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RefactoredProject
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("The GameManager is NULL.");
                }

                return instance;
            }
        }

        public int CurrentAmountOfEnemies { get; set; }
        public int EnemiesKilled { get; set; }
        public int PlayerDeaths { get; set; }

        private int dna, combo;
        private readonly float timeToResetCombo = 10.0f;
        private float timeSinceLastKill;

        private int highestCombo;
        private float runTime;
        private int score;

        private GameObject panelGameMenu;
        private GameObject panelHelp;
        private GameObject panelEnd;

        [SerializeField] private int enemiesKilledWeight, highestComboWeight, runTimeWeight, playerDeathsWeight;
        [SerializeField] private Text textScore, textEnemiesKilled, textHighestCombo, textRunTime, textPlayerDeaths;

        [Header("Progression")]
        [SerializeField] private Text textAmountDNA;
        [SerializeField] private Button SharpndClawsI, SharpndClawsII, SharpndClawsIII, HollwdClaws;
        [SerializeField] private Button SplintrdBonesI, SplintrdBonesII, BoneAdaptatn, ChambrdBones;
        [SerializeField] private Button BloodAbsorptnI, BloodAbsorptnII, CellGrwthI, CellGrwthII;
        [SerializeField] private Button FlexdMusclesI, FlexdMusclesII, AerodynmcBodyI, AerodynmcBodyII;

        #region Events

        public delegate void Pause();
        public static event Pause OnPause;

        public delegate void Unpause();
        public static event Unpause OnUnpause;

        public delegate void SharpndClawsEvent();
        public static event SharpndClawsEvent OnSharpndClawsEvent;

        public delegate void HollwdClawsEvent();
        public static event HollwdClawsEvent OnHollwdClawsEvent;

        public delegate void SplintrdBonesEvent();
        public static event SplintrdBonesEvent OnSplintrdBonesEvent;

        public delegate void BoneAdaptatnEvent();
        public static event BoneAdaptatnEvent OnBoneAdaptatnEvent;

        public delegate void ChambrdBonesEvent();
        public static event ChambrdBonesEvent OnChambrdBonesEvent;

        public delegate void BloodAbsorptnEvent();
        public static event BloodAbsorptnEvent OnBloodAbsorptnEvent;

        public delegate void CellGrwthIEvent();
        public static event CellGrwthIEvent OnCellGrwthIEvent;

        public delegate void CellGrwthIIEvent();
        public static event CellGrwthIIEvent OnCellGrwthIIEvent;

        public delegate void FlexdMusclesIEvent();
        public static event FlexdMusclesIEvent OnFlexdMusclesIEvent;

        public delegate void FlexdMusclesIIEvent();
        public static event FlexdMusclesIIEvent OnFlexdMusclesIIEvent;

        public delegate void AerodynmcBodyEvent();
        public static event AerodynmcBodyEvent OnAerodynmcBodyEvent;

        #endregion

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            panelGameMenu = GameObject.Find("PanelGameMenu");
            panelHelp = GameObject.Find("PanelHelp");
            panelEnd = GameObject.Find("PanelEnd");

            panelEnd.SetActive(false);

            ContinueGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !panelEnd.activeInHierarchy)
            {
                if (!panelGameMenu.activeInHierarchy && !panelHelp.activeInHierarchy)
                {
                    PauseGame();
                }
                else
                {
                    ContinueGame();
                }
            }

            timeSinceLastKill += Time.deltaTime;

            if (timeSinceLastKill >= timeToResetCombo)
            {
                ResetCombo();
            }

            if (combo > highestCombo)
            {
                highestCombo = combo;
            }

            runTime += Time.deltaTime;
        }

        #region GameMenu

        private void PauseGame()
        {
            Time.timeScale = 0;
            panelGameMenu.SetActive(true);

            OnPause?.Invoke();

            textAmountDNA.text = "DNA: " + dna.ToString();
        }

        public void ContinueGame()
        {
            Time.timeScale = 1;

            if (panelGameMenu.activeInHierarchy)
            {
                panelGameMenu.SetActive(false);
            }

            if (panelHelp.activeInHierarchy)
            {
                panelHelp.SetActive(false);
            }

            OnUnpause?.Invoke();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ShowHelpPanel()
        {
            panelGameMenu.SetActive(false);
            panelHelp.SetActive(true);
        }

        #endregion

        #region DNA

        public void AddDNA(int dna)
        {
            this.dna += dna;

            CanvasManager.Instance.UpdateDNA(this.dna);
        }

        public void SubstractDNA(int dna)
        {
            this.dna -= dna;

            textAmountDNA.text = "DNA: " + this.dna.ToString();

            CanvasManager.Instance.UpdateDNA(this.dna);
        }

        #endregion

        #region Combo

        public void UpdateCombo()
        {
            combo++;

            timeSinceLastKill = 0;

            CanvasManager.Instance.UpdateCombo(combo);
        }

        public void ResetCombo()
        {
            combo = 0;

            CanvasManager.Instance.UpdateCombo(combo);
        }

        #endregion

        public void EndGame()
        {
            panelEnd.SetActive(true);

            Time.timeScale = 0;
            OnPause?.Invoke();

            score = EnemiesKilled * enemiesKilledWeight + highestCombo * highestComboWeight - Mathf.RoundToInt(runTime) * runTimeWeight - PlayerDeaths * playerDeathsWeight;

            textScore.text = "SCORE: " + score.ToString();
            textEnemiesKilled.text = "Enemies Killed: " + EnemiesKilled.ToString();
            textHighestCombo.text = "Highest Combo: " + highestCombo.ToString();
            textRunTime.text = "Run Time: " + Mathf.RoundToInt(runTime / 3600).ToString() + ":" + Mathf.RoundToInt((runTime % 3600) / 60).ToString() + ":"
                + Mathf.RoundToInt((runTime % 3600) % 60).ToString();
            textPlayerDeaths.text = "Player Deaths: " + PlayerDeaths.ToString();
        }

        #region Mutations

        public void SharpenedClawsI()
        {
            if (dna >= 100)
            {
                SubstractDNA(100);

                SharpndClawsI.interactable = false;

                SharpndClawsII.interactable = true;

                OnSharpndClawsEvent?.Invoke();
            }
        }

        public void SharpenedClawsII()
        {
            if (dna >= 200)
            {
                SubstractDNA(200);

                SharpndClawsII.interactable = false;

                SharpndClawsIII.interactable = true;

                OnSharpndClawsEvent?.Invoke();
            }
        }

        public void SharpenedClawsIII()
        {
            if (dna >= 300)
            {
                SubstractDNA(300);

                SharpndClawsIII.interactable = false;

                OnSharpndClawsEvent?.Invoke();
            }
        }

        public void HollowedClaws()
        {
            if (dna >= 800)
            {
                SubstractDNA(800);

                HollwdClaws.interactable = false;

                OnHollwdClawsEvent?.Invoke();
            }
        }

        public void SplinteredBonesI()
        {
            if (dna >= 200)
            {
                SubstractDNA(200);

                SplintrdBonesI.interactable = false;

                SplintrdBonesII.interactable = true;

                OnSplintrdBonesEvent?.Invoke();
            }
        }

        public void SplinteredBonesII()
        {
            if (dna >= 200)
            {
                SubstractDNA(200);

                SplintrdBonesII.interactable = false;

                OnSplintrdBonesEvent?.Invoke();
            }
        }

        public void BoneAdaptation()
        {
            if (dna >= 400)
            {
                SubstractDNA(400);

                BoneAdaptatn.interactable = false;

                OnBoneAdaptatnEvent?.Invoke();
            }
        }

        public void ChamberedBones()
        {
            if (dna >= 600)
            {
                SubstractDNA(600);

                ChambrdBones.interactable = false;

                OnChambrdBonesEvent?.Invoke();
            }
        }

        public void BloodAbsorptionI()
        {
            if (dna >= 400)
            {
                SubstractDNA(400);

                BloodAbsorptnI.interactable = false;

                BloodAbsorptnII.interactable = true;

                OnBloodAbsorptnEvent?.Invoke();
            }
        }

        public void BloodAbsorptionII()
        {
            if (dna >= 600)
            {
                SubstractDNA(600);

                BloodAbsorptnII.interactable = false;

                OnBloodAbsorptnEvent?.Invoke();
            }
        }

        public void CellGrowthI()
        {
            if (dna >= 250)
            {
                SubstractDNA(250);

                CellGrwthI.interactable = false;

                CellGrwthII.interactable = true;

                OnCellGrwthIEvent?.Invoke();
            }
        }

        public void CellGrowthII()
        {
            if (dna >= 500)
            {
                SubstractDNA(500);

                CellGrwthII.interactable = false;

                OnCellGrwthIIEvent?.Invoke();
            }
        }

        public void FlexedMusclesI()
        {
            if (dna >= 150)
            {
                SubstractDNA(150);

                FlexdMusclesI.interactable = false;

                FlexdMusclesII.interactable = true;

                OnFlexdMusclesIEvent?.Invoke();
            }
        }

        public void FlexedMusclesII()
        {
            if (dna >= 300)
            {
                SubstractDNA(300);

                FlexdMusclesII.interactable = false;

                OnFlexdMusclesIIEvent?.Invoke();
            }
        }

        public void AerodynamicBodyI()
        {
            if (dna >= 250)
            {
                SubstractDNA(250);

                AerodynmcBodyI.interactable = false;

                AerodynmcBodyII.interactable = true;

                OnAerodynmcBodyEvent?.Invoke();
            }
        }

        public void AerodynamicBodyII()
        {
            if (dna >= 400)
            {
                SubstractDNA(400);

                AerodynmcBodyII.interactable = false;

                OnAerodynmcBodyEvent?.Invoke();
            }
        }

        #endregion
    }
}