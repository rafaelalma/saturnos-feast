using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace RefactoredProject
{
    public enum RangeEnemyState
    {
        Seek, Idle, Attack, Move, Hide
    }

    public class RangeEnemyBehaviour : MonoBehaviour
    {
        public RangeEnemyState currentEnumState;

        #region SerializedFields

        [SerializeField] private float moveSpeed;
        public float MoveSpeed { get { return moveSpeed; } private set { moveSpeed = value; } }

        [SerializeField] private float exteriorDistance;
        public float ExteriorDistance { get { return exteriorDistance; } private set { exteriorDistance = value; } }

        [SerializeField] private float interiorDistance;
        public float InteriorDistance { get { return interiorDistance; } private set { interiorDistance = value; } }

        [SerializeField] Transform firePoint;
        public Transform FirePoint { get { return firePoint; } private set { firePoint = value; } }

        [SerializeField] LayerMask playerAndObstacle;
        public LayerMask PlayerAndObstacle { get { return playerAndObstacle; } private set { playerAndObstacle = value; } }

        [SerializeField] float timeBetweenProjectiles;
        public float TimeBetweenProjectiles { get { return timeBetweenProjectiles; } private set { timeBetweenProjectiles = value; } }

        [SerializeField] float projectileSpeed;
        public float ProjectileSpeed { get { return projectileSpeed; } private set { projectileSpeed = value; } }

        [SerializeField] int projectileDamage;
        public int ProjectileDamage { get { return projectileDamage; } private set { projectileDamage = value; } }

        [Header("Sounds")]

        [SerializeField] private AudioClip takeDamageSound;
        public AudioClip TakeDamageSound { get { return takeDamageSound; } private set { takeDamageSound = value; } }

        #endregion

        public Transform SpawnPosition { get; set; }

        public float MoveRadius { get; set; }

        public Transform Player { get; private set; }

        public IAstarAI IAstarAI { get; private set; }

        public AILerp AILerp { get; private set; }

        public Animator Animator { get; private set; }

        public SpriteRenderer SpriteRenderer { get; private set; }

        public BoxCollider2D BoxCollider { get; private set; }

        public AudioSource AudioSource { get; private set; }

        #region States

        public readonly RangeEnemySeekState SeekState = new RangeEnemySeekState();
        public readonly RangeEnemyIdleState IdleState = new RangeEnemyIdleState();
        public readonly RangeEnemyAttackState AttackState = new RangeEnemyAttackState();
        public readonly RangeEnemyMoveState MoveState = new RangeEnemyMoveState();
        public readonly RangeEnemyHideState HideState = new RangeEnemyHideState();

        #endregion

        private RangeEnemyBaseState currentState;

        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;

            IAstarAI = GetComponent<IAstarAI>();
            AILerp = GetComponent<AILerp>();

            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            BoxCollider = GetComponent<BoxCollider2D>();

            AudioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            TransitionToState(SeekState);
        }

        public void TransitionToState(RangeEnemyBaseState state)
        {
            currentState = state;
            currentState.EnterState(this);
        }

        private void Update()
        {
            currentState.Update(this);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, exteriorDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interiorDistance);
        }
    }
}