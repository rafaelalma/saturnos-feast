using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public abstract class NewEnemyBaseState
    {
        public abstract void EnterState(NewEnemyBehaviour newEnemy);

        public abstract void Update(NewEnemyBehaviour newEnemy);

        public abstract void OnCollisionEnter2D(NewEnemyBehaviour newEnemy, Collision2D collision);
    }
}