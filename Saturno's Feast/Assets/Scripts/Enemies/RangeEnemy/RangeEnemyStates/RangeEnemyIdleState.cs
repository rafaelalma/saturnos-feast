using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class RangeEnemyIdleState : RangeEnemyBaseState
    {
        private float time;

        public override void EnterState(RangeEnemyBehaviour rangeEnemy)
        {
            rangeEnemy.currentEnumState = RangeEnemyState.Idle;

            time = 0.0f;
        }

        public override void Update(RangeEnemyBehaviour rangeEnemy)
        {
            time += Time.deltaTime;

            if (time >= 3.0f || Vector3.Distance(rangeEnemy.transform.position, rangeEnemy.Player.position) <= rangeEnemy.InteriorDistance)
            {
                rangeEnemy.TransitionToState(rangeEnemy.HideState);
            }
            else if (Vector3.Distance(rangeEnemy.transform.position, rangeEnemy.Player.position) > rangeEnemy.InteriorDistance
                    && Vector3.Distance(rangeEnemy.transform.position, rangeEnemy.Player.position) <= rangeEnemy.ExteriorDistance)
            {
                rangeEnemy.TransitionToState(rangeEnemy.AttackState);
            }
        }
    }
}