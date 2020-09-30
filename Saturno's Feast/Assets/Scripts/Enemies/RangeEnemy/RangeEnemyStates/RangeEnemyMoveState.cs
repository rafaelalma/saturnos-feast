using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class RangeEnemyMoveState : RangeEnemyBaseState
    {
        public override void EnterState(RangeEnemyBehaviour rangeEnemy)
        {
            rangeEnemy.currentEnumState = RangeEnemyState.Move;

            rangeEnemy.SpriteRenderer.enabled = false;
            rangeEnemy.BoxCollider.enabled = false;

            rangeEnemy.AILerp.enabled = true;
            rangeEnemy.AILerp.speed = rangeEnemy.MoveSpeed;

            Move(rangeEnemy);
        }

        private void Move(RangeEnemyBehaviour rangeEnemy)
        {
            rangeEnemy.IAstarAI.destination = PickRandomPoint(rangeEnemy);
            rangeEnemy.IAstarAI.SearchPath();
        }

        private Vector3 PickRandomPoint(RangeEnemyBehaviour rangeEnemy)
        {
            Vector3 point = Random.insideUnitSphere * rangeEnemy.MoveRadius;

            point.z = 0;
            point += rangeEnemy.SpawnPosition.position;
            return point;
        }

        public override void Update(RangeEnemyBehaviour rangeEnemy)
        {
            if(!rangeEnemy.IAstarAI.pathPending && (rangeEnemy.IAstarAI.reachedEndOfPath || !rangeEnemy.IAstarAI.hasPath))
            {
                rangeEnemy.AILerp.enabled = false;

                rangeEnemy.TransitionToState(rangeEnemy.SeekState);
            }
        }
    }
}