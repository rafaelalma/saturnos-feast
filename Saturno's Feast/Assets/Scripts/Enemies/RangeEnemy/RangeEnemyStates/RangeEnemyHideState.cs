using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class RangeEnemyHideState : RangeEnemyBaseState
    {
        private float time;

        public override void EnterState(RangeEnemyBehaviour rangeEnemy)
        {
            rangeEnemy.currentEnumState = RangeEnemyState.Hide;

            time = 0.0f;

            rangeEnemy.Animator.SetTrigger("isHiding");
        }

        public override void Update(RangeEnemyBehaviour rangeEnemy)
        {
            time += Time.deltaTime;

            if(time >= 0.5f)
            {
                rangeEnemy.TransitionToState(rangeEnemy.MoveState);
            }
        }
    }
}