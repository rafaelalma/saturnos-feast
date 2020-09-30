using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class RangeEnemySeekState : RangeEnemyBaseState
    {
        private float time;

        public override void EnterState(RangeEnemyBehaviour rangeEnemy)
        {
            rangeEnemy.currentEnumState = RangeEnemyState.Seek;

            time = 0.0f;

            rangeEnemy.SpriteRenderer.enabled = true;
            rangeEnemy.BoxCollider.enabled = true;

            rangeEnemy.Animator.SetTrigger("isSeeking");
        }

        public override void Update(RangeEnemyBehaviour rangeEnemy)
        {
            time += Time.deltaTime;

            if(time >= 0.5f)
            {
                rangeEnemy.TransitionToState(rangeEnemy.IdleState);
            }
        }
    }
}