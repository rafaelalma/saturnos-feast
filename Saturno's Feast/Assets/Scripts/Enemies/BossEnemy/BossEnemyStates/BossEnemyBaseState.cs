using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public abstract class BossEnemyBaseState
    {
        public abstract void EnterState(BossEnemyBehaviour bossEnemy);

        public abstract void Update(BossEnemyBehaviour bossEnemy);

        public abstract void OnCollisionEnter2D(BossEnemyBehaviour bossEnemy, Collision2D collision);
    }
}