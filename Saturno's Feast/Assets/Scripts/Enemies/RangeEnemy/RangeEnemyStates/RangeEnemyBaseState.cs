using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public abstract class RangeEnemyBaseState
    {
        public abstract void EnterState(RangeEnemyBehaviour rangeEnemy);

        public abstract void Update(RangeEnemyBehaviour rangeEnemy);
    }
}