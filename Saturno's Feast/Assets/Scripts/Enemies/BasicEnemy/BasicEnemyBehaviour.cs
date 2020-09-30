using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace RefactoredProject
{
    public enum BasicEnemyState
    {
        Wander, Idle, Chase, Flee
    }

    public class BasicEnemyBehaviour : MonoBehaviour
    {
        public BasicEnemyState currentEnumState;

        #region SerializedFields

        [SerializeField] private float wanderRadius;
        public float WanderRadius { get { return wanderRadius; } private set { wanderRadius = value; } }

        [SerializeField] private float wanderSpeed;
        public float WanderSpeed { get { return wanderSpeed; } private set { wanderSpeed = value; } }

        [SerializeField] private float chaseSpeed;
        public float ChaseSpeed { get { return chaseSpeed; } private set { chaseSpeed = value; } }

        [SerializeField] private float exteriorDistance;
        public float ExteriorDistance { get { return exteriorDistance; } set { exteriorDistance = value; } }

        [SerializeField] private float interiorDistance;
        public float InteriorDistance { get { return interiorDistance; } private set { interiorDistance = value; } }

        [SerializeField] private int attackDamage;
        public int AttackDamage { get { return attackDamage; } private set { attackDamage = value; } }

        [Header("Sounds")]

        [SerializeField] private AudioClip takeDamageSound;
        public AudioClip TakeDamageSound { get { return takeDamageSound; } private set { takeDamageSound = value; } }

        #endregion

        public Transform Player { get; private set; }

        public IAstarAI IAstarAI { get; private set; }

        public AILerp AILerp { get; private set; }

        public Animator Animator { get; private set; }

        public AudioSource AudioSource { get; private set; }

        #region States

        public readonly BasicEnemyWanderState WanderState = new BasicEnemyWanderState();
        public readonly BasicEnemyIdleState IdleState = new BasicEnemyIdleState();
        public readonly BasicEnemyChaseState ChaseState = new BasicEnemyChaseState();
        public readonly BasicEnemyFleeState FleeState = new BasicEnemyFleeState();

        #endregion

        private BasicEnemyBaseState currentState;

        private float angle;

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
            TransitionToState(WanderState);
        }

        public void TransitionToState(BasicEnemyBaseState state)
        {
            currentState = state;
            currentState.EnterState(this);
        }

        private void Update()
        {
            currentState.Update(this);

            Animator.SetFloat("angle", CalculateAngle());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            currentState.OnCollisionEnter2D(this, collision);
        }

        private float CalculateAngle()
        {
            Vector3 lookDirection = IAstarAI.destination - transform.position;
            lookDirection.Normalize();

            angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
            return angle;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, exteriorDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interiorDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, wanderRadius);
        }
    }
}