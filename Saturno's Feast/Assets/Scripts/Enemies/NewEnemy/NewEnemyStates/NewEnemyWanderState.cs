using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace RefactoredProject
{
    public class NewEnemyWanderState : NewEnemyBaseState
    {
        private GridGraph grid;
        private GraphNode myNode, randomNode;

        public override void EnterState(NewEnemyBehaviour newEnemy)
        {
            newEnemy.currentEnumState = NewEnemyState.Wander;

            newEnemy.Animator.SetBool("isRunning", true);

            newEnemy.AILerp.speed = newEnemy.WanderSpeed;

            newEnemy.SpriteRenderer.color = Color.white;

            grid = AstarPath.active.data.gridGraph;

            myNode = AstarPath.active.GetNearest(newEnemy.transform.position, NNConstraint.Default).node;

            Wander(newEnemy);
        }

        private void Wander(NewEnemyBehaviour newEnemy)
        {
            randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];

            if (randomNode.Walkable && PathUtilities.IsPathPossible(myNode, randomNode))
            {
                newEnemy.IAstarAI.destination = (Vector3)randomNode.position;
                newEnemy.IAstarAI.SearchPath();
            }
            else
            {
                Wander(newEnemy);
            }
        }

        public override void Update(NewEnemyBehaviour newEnemy)
        {
            if (Vector3.Distance(newEnemy.transform.position, newEnemy.Player.position) <= newEnemy.ExteriorDistance)
            {
                newEnemy.TransitionToState(newEnemy.ChaseState);
            }
            else if (newEnemy.IAstarAI.reachedEndOfPath)
            {
                myNode = AstarPath.active.GetNearest(newEnemy.transform.position, NNConstraint.Default).node;

                Wander(newEnemy);
            }
        }

        public override void OnCollisionEnter2D(NewEnemyBehaviour newEnemy, Collision2D collision)
        {
            return;
        }
    }
}