using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public abstract class BasicEnemyBaseState
    {
        public abstract void EnterState(BasicEnemyBehaviour basicEnemy);

        public abstract void Update(BasicEnemyBehaviour basicEnemy);

        public abstract void OnCollisionEnter2D(BasicEnemyBehaviour basicEnemy, Collision2D collision);
    }
}