using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace RefactoredProject
{
    public enum NewEnemyState
    {
        Wander, Chase, Attack, Flee, Stunned
    }

    public class NewEnemyBehaviour : MonoBehaviour
    {
        public NewEnemyState currentEnumState;

        #region SerializedFields

        [SerializeField] private float wanderSpeed;
        public float WanderSpeed { get { return wanderSpeed; } private set { wanderSpeed = value; } }

        [SerializeField] private float chaseSpeed;
        public float ChaseSpeed { get { return chaseSpeed; } private set { chaseSpeed = value; } }

        [SerializeField] private float attackSpeed;
        public float AttackSpeed { get { return attackSpeed; } private set { attackSpeed = value; } }

        [SerializeField] private float exteriorDistance;
        public float ExteriorDistance { get { return exteriorDistance; } private set { exteriorDistance = value; } }

        [SerializeField] private float interiorDistance;
        public float InteriorDistance { get { return interiorDistance; } private set { interiorDistance = value; } }

        [SerializeField] private float closeDistance;
        public float CloseDistance { get { return closeDistance; } private set { closeDistance = value; } }

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

        public SpriteRenderer SpriteRenderer { get; private set; }

        public AudioSource AudioSource { get; private set; }

        #region States

        public readonly NewEnemyWanderState WanderState = new NewEnemyWanderState();
        public readonly NewEnemyChaseState ChaseState = new NewEnemyChaseState();
        public readonly NewEnemyAttackState AttackState = new NewEnemyAttackState();
        public readonly NewEnemyFleeState FleeState = new NewEnemyFleeState();
        public readonly NewEnemyStunnedState StunnedState = new NewEnemyStunnedState();

        #endregion

        private NewEnemyBaseState currentState;

        private float angle;

        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;

            IAstarAI = GetComponent<IAstarAI>();
            AILerp = GetComponent<AILerp>();

            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();

            AudioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            TransitionToState(WanderState);
        }

        public void TransitionToState(NewEnemyBaseState state)
        {
            currentState = state;
            currentState.EnterState(this);
        }

        private void Update()
        {
            currentState.Update(this);

            CalculateAngle();

            Animator.SetFloat("angle", angle);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            currentState.OnCollisionEnter2D(this, collision);
        }

        private void CalculateAngle()
        {
            Vector3 lookDirection = IAstarAI.destination - transform.position;
            lookDirection.Normalize();
            angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90.0f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, exteriorDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, interiorDistance);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, closeDistance);
        }
    }
}