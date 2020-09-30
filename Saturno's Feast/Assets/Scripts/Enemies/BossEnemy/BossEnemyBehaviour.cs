using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace RefactoredProject
{
    public enum BossEnemyState
    {
        Intro, Chase, Attack, Charge, Stunned
    }

    public class BossEnemyBehaviour : MonoBehaviour
    {
        public BossEnemyState currentEnumState;

        #region SerializedFields

        [SerializeField] private LayerMask playerLayer;
        public LayerMask PlayerLayer { get { return playerLayer; } private set { playerLayer = value; } }

        [SerializeField] private float chaseSpeed;
        public float ChaseSpeed { get { return chaseSpeed; } private set { chaseSpeed = value; } }

        [SerializeField] private int attackDamage;
        public int AttackDamage { get { return attackDamage; } private set { attackDamage = value; } }

        [SerializeField] private float minChargeCooldown;
        public float MinChargeCooldown { get { return minChargeCooldown; } private set { minChargeCooldown = value; } }

        [SerializeField] private float maxChargeCooldown;
        public float MaxChargeCooldown { get { return maxChargeCooldown; } private set { maxChargeCooldown = value; } }

        [SerializeField] private float chargeSpeed;
        public float ChargeSpeed { get { return chargeSpeed; } private set { chargeSpeed = value; } }

        [SerializeField] private int chargeDamage;
        public int ChargeDamage { get { return chargeDamage; } private set { chargeDamage = value; } }

        [SerializeField] private float interiorDistance;
        public float InteriorDistance { get { return interiorDistance; } private set { interiorDistance = value; } }

        [SerializeField] private float exteriorDistance;
        public float ExteriorDistance { get { return exteriorDistance; } private set { exteriorDistance = value; } }

        [Header("Sounds")]

        [SerializeField] private AudioClip takeDamageSound;
        [SerializeField] private AudioClip introSound;

        public AudioClip TakeDamageSound { get { return takeDamageSound; } private set { takeDamageSound = value; } }

        #endregion

        public float TimeSinceLastCharge { get; set; }

        public float ChargeCooldown { get; set; }

        public Transform Player { get; private set; }

        public IAstarAI IAstarAI { get; private set; }

        public AILerp AILerp { get; private set; }

        public Animator Animator { get; private set; }

        public AudioSource AudioSource { get; private set; }

        #region States

        public readonly BossEnemyIntroState IntroState = new BossEnemyIntroState();
        public readonly BossEnemyChaseState ChaseState = new BossEnemyChaseState();
        public readonly BossEnemyAttackState AttackState = new BossEnemyAttackState();
        public readonly BossEnemyChargeState ChargeState = new BossEnemyChargeState();
        public readonly BossEnemyStunnedState StunnedState = new BossEnemyStunnedState();

        #endregion

        private BossEnemyBaseState currentState;

        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;

            IAstarAI = GetComponent<IAstarAI>();
            AILerp = GetComponent<AILerp>();

            Animator = GetComponent<Animator>();

            AudioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            TransitionToState(IntroState);
        }

        public void TransitionToState(BossEnemyBaseState state)
        {
            currentState = state;
            currentState.EnterState(this);
        }

        private void Update()
        {
            currentState.Update(this);

            TimeSinceLastCharge += Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            currentState.OnCollisionEnter2D(this, collision);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interiorDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, exteriorDistance);
        }

        public void PlayIntroSound()
        {
            AudioSource.PlayOneShot(introSound, 0.5f);
        }
    }
}