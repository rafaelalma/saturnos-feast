using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public enum PlayerState
    {
        Respawn, Idle, Run, MeleeAttack, Block, Dash, StrongRangeAttack, TakeDamage, Dead
    }

    public class PlayerBehaviour : MonoBehaviour
    {
        public PlayerState currentEnumState;

        #region SerializedFields

        [SerializeField] private Transform respawnPosition;
        public Transform RespawnPosition { get { return respawnPosition; } private set { respawnPosition = value; } }

        [SerializeField] private float speed;
        public float Speed { get { return speed; } private set { speed = value; } }

        [SerializeField] private float dashSpeed;
        public float DashSpeed { get { return dashSpeed; } private set { dashSpeed = value; } }

        [SerializeField] private float dashCooldown;
        public float DashCooldown { get { return dashCooldown; } private set { dashCooldown = value; } }

        [SerializeField] private LayerMask obstacleLayer;
        public LayerMask ObstacleLayer { get { return obstacleLayer; } private set { obstacleLayer = value; } }

        [SerializeField] private int maxHealth;
        public int MaxHealth { get { return maxHealth; } private set { maxHealth = value; } }

        [SerializeField] private LayerMask enemyLayer;
        public LayerMask EnemyLayer { get { return enemyLayer; } private set { enemyLayer = value; } }

        [SerializeField] private int meleeAttackDamage;
        public int MeleeAttackDamage { get { return meleeAttackDamage; } private set { meleeAttackDamage = value; } }

        [SerializeField] private Transform firePoint;
        public Transform FirePoint { get { return firePoint; } private set { firePoint = value; } }

        [SerializeField] private int maxProjectiles;
        public int MaxProjectiles { get { return maxProjectiles; } private set { maxProjectiles = value; } }

        [SerializeField] private float projectileSpeed;
        public float ProjectileSpeed { get { return projectileSpeed; } private set { projectileSpeed = value; } }

        [SerializeField] private int projectileDamage;
        public int ProjectileDamage { get { return projectileDamage; } private set { projectileDamage = value; } }

        [SerializeField] private int lifesteal;
        public int Lifesteal { get { return lifesteal; } private set { lifesteal = value; } }

        [Header("Sounds")]

        [SerializeField] AudioClip runSound;
        [SerializeField] AudioClip meleeAttackSound;
        [SerializeField] AudioClip rangeAttackSound;

        [SerializeField] private AudioClip blockSound;
        public AudioClip BlockSound { get { return blockSound; } private set { blockSound = value; } }

        [SerializeField] private AudioClip takeDamageSound;
        public AudioClip TakeDamageSound { get { return takeDamageSound; } private set { takeDamageSound = value; } }

        #endregion

        public int DamageTaken { get; set; }

        public int CurrentHealth { get; set; }

        public int CurrentProjectiles { get; set; }

        public float TimeSinceLastDash { get; set; }

        public Vector3 MoveDirection { get; private set; }

        public BoxCollider2D BoxCollider { get; private set; }

        public Animator Animator { get; private set; }

        public AudioSource AudioSource { get; private set; }

        public bool HasBlueKey { get; private set; }
        public bool HasWhiteKey { get; private set; }
        public bool HasBlackKey { get; private set; }

        public bool HasMitigateCorruptionPower { get; private set; }

        public bool HasMeleeAttackImprovement { get; private set; }
        public bool HasRangeAttackImprovement { get; private set; }
        public bool HasProjectilePickupImprovement { get; private set; }

        public bool PausedGame { get; private set; }

        #region States

        public readonly PlayerRespawnState RespawnState = new PlayerRespawnState();
        public readonly PlayerIdleState IdleState = new PlayerIdleState();
        public readonly PlayerRunState RunState = new PlayerRunState();
        public readonly PlayerMeleeAttackState MeleeAttackState = new PlayerMeleeAttackState();
        public readonly PlayerBlockState BlockState = new PlayerBlockState();
        public readonly PlayerDashState DashState = new PlayerDashState();
        public readonly PlayerStrongRangeAttackState StrongRangeAttackState = new PlayerStrongRangeAttackState();
        public readonly PlayerTakeDamageState TakeDamageState = new PlayerTakeDamageState();
        public readonly PlayerDeadState DeadState = new PlayerDeadState();

        #endregion

        private PlayerBaseState currentState;

        private void Awake()
        {
            Animator = GetComponent<Animator>();

            BoxCollider = GetComponent<BoxCollider2D>();

            AudioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            GameManager.OnPause += GameIsPaused;
            GameManager.OnUnpause += GameIsUnpaused;

            BossEnemy.OnBossEnemyDeath += UnlockMitigateCorruptionPower;

            GameManager.OnSharpndClawsEvent += SharpenedClaws;
            GameManager.OnHollwdClawsEvent += HollowedClaws;
            GameManager.OnSplintrdBonesEvent += SplinteredBones;
            GameManager.OnBoneAdaptatnEvent += BoneAdaptation;
            GameManager.OnChambrdBonesEvent += ChamberedBones;
            GameManager.OnBloodAbsorptnEvent += BloodAbsorption;
            GameManager.OnCellGrwthIEvent += CellGrowthI;
            GameManager.OnCellGrwthIIEvent += CellGrowthII;
            GameManager.OnFlexdMusclesIEvent += FlexedMusclesI;
            GameManager.OnFlexdMusclesIIEvent += FlexedMusclesII;
            GameManager.OnAerodynmcBodyEvent += AerodynamicBody;
        }

        private void Start()
        {
            CurrentHealth = MaxHealth;
            CurrentProjectiles = MaxProjectiles;

            CanvasManager.Instance.UpdateLifebar(CurrentHealth, MaxHealth);
            CanvasManager.Instance.UpdateProjectiles(CurrentProjectiles);

            TransitionToState(IdleState);
        }

        public void TransitionToState(PlayerBaseState state)
        {
            currentState = state;
            currentState.EnterState(this);
        }

        private void Update()
        {
            currentState.Update(this);

            if (!PausedGame)
            {
                ManageMovementParameters();
            }

            TimeSinceLastDash += Time.deltaTime;
            if (TimeSinceLastDash >= dashCooldown)
            {
                CanvasManager.Instance.UpdateDash(true);
            }
        }

        private void OnDisable()
        {
            GameManager.OnPause -= GameIsPaused;
            GameManager.OnUnpause -= GameIsUnpaused;

            BossEnemy.OnBossEnemyDeath -= UnlockMitigateCorruptionPower;

            GameManager.OnSharpndClawsEvent -= SharpenedClaws;
            GameManager.OnHollwdClawsEvent -= HollowedClaws;
            GameManager.OnSplintrdBonesEvent -= SplinteredBones;
            GameManager.OnBoneAdaptatnEvent -= BoneAdaptation;
            GameManager.OnChambrdBonesEvent -= ChamberedBones;
            GameManager.OnBloodAbsorptnEvent -= BloodAbsorption;
            GameManager.OnCellGrwthIEvent -= CellGrowthI;
            GameManager.OnCellGrwthIIEvent -= CellGrowthII;
            GameManager.OnFlexdMusclesIEvent -= FlexedMusclesI;
            GameManager.OnFlexdMusclesIIEvent -= FlexedMusclesII;
            GameManager.OnAerodynmcBodyEvent -= AerodynamicBody;
        }

        private void ManageMovementParameters()
        {
            int moveX = (int)Input.GetAxisRaw("Horizontal");
            int moveY = (int)Input.GetAxisRaw("Vertical");
            MoveDirection = new Vector3(moveX, moveY, 0).normalized;

            Animator.SetInteger("moveX", moveX);
            Animator.SetInteger("moveY", moveY);
        }

        public void TakeDamage(int damage)
        {
            if (damage > 0)
            {
                DamageTaken = damage;

                ShouldTransitionToTakeDamageState();
            }
        }

        private void ShouldTransitionToTakeDamageState()
        {
            currentState.ShouldTransitionToTakeDamageState(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("ProjectilePickup"))
            {
                if (CurrentProjectiles < MaxProjectiles)
                {
                    collision.gameObject.SetActive(false);

                    if (!HasProjectilePickupImprovement)
                    {
                        CurrentProjectiles++;
                    }
                    else
                    {
                        CurrentProjectiles += 2;
                    }

                    if (CurrentProjectiles > maxProjectiles)
                    {
                        CurrentProjectiles = maxProjectiles;
                    }

                    CanvasManager.Instance.UpdateProjectiles(CurrentProjectiles);
                }
            }
            else if (collision.gameObject.CompareTag("BlueKey"))
            {
                collision.gameObject.SetActive(false);

                HasBlueKey = true;

                CanvasManager.Instance.ShowBlueKey();
            }
            else if (collision.gameObject.CompareTag("WhiteKey"))
            {
                collision.gameObject.SetActive(false);

                HasWhiteKey = true;

                CanvasManager.Instance.ShowWhiteKey();
            }
            else if (collision.gameObject.CompareTag("BlackKey"))
            {
                collision.gameObject.SetActive(false);

                HasBlackKey = true;

                CanvasManager.Instance.ShowBlackKey();
            }
        }

        #region Powers

        private void UnlockMitigateCorruptionPower()
        {
            HasMitigateCorruptionPower = true;
        }

        #endregion

        #region Mutations

        private void SharpenedClaws()
        {
            meleeAttackDamage += 2;
        }

        private void HollowedClaws()
        {
            HasMeleeAttackImprovement = true;
            meleeAttackDamage -= 2;
        }

        private void SplinteredBones()
        {
            MaxProjectiles += 5;
            CurrentProjectiles += 5;

            projectileDamage += 2;

            CanvasManager.Instance.UpdateProjectiles(CurrentProjectiles);
        }

        private void BoneAdaptation()
        {
            HasProjectilePickupImprovement = true;
        }

        private void ChamberedBones()
        {
            HasRangeAttackImprovement = true;
        }

        private void BloodAbsorption()
        {
            lifesteal++;
        }

        private void CellGrowthI()
        {
            maxHealth += 50;
            CurrentHealth += 50;

            CanvasManager.Instance.UpdateLifebar(CurrentHealth, maxHealth);
        }

        private void CellGrowthII()
        {
            maxHealth += 100;
            CurrentHealth += 100;

            CanvasManager.Instance.UpdateLifebar(CurrentHealth, maxHealth);
        }

        private void FlexedMusclesI()
        {
            speed++;
        }

        private void FlexedMusclesII()
        {
            speed += 2;
        }

        private void AerodynamicBody()
        {
            dashCooldown -= 0.5f;
        }

        #endregion

        #region Sounds

        public void PlayRunSound()
        {
            AudioSource.PlayOneShot(runSound, 0.6f);
        }

        public void PlayMeleeAttackSound()
        {
            AudioSource.PlayOneShot(meleeAttackSound, 0.2f);
        }

        public void PlayRangeAttackSound()
        {
            AudioSource.PlayOneShot(rangeAttackSound, 0.1f);
        }

        #endregion

        private void GameIsPaused()
        {
            PausedGame = true;
        }

        private void GameIsUnpaused()
        {
            PausedGame = false;
        }

        private void OnDrawGizmosSelected()
        {
            // GIZMO
            Gizmos.DrawWireSphere(transform.position, 2);
        }
    }
}