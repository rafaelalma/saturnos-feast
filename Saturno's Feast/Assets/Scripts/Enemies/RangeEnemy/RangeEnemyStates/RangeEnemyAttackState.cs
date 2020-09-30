using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public class RangeEnemyAttackState : RangeEnemyBaseState
    {
        private bool projectileIsReady = true;

        public override void EnterState(RangeEnemyBehaviour rangeEnemy)
        {
            rangeEnemy.currentEnumState = RangeEnemyState.Attack;
        }

        public override void Update(RangeEnemyBehaviour rangeEnemy)
        {
            if (Vector3.Distance(rangeEnemy.transform.position, rangeEnemy.Player.position) <= rangeEnemy.InteriorDistance)
            {
                rangeEnemy.TransitionToState(rangeEnemy.HideState);
            }
            else if (Vector3.Distance(rangeEnemy.transform.position, rangeEnemy.Player.position) >= rangeEnemy.ExteriorDistance)
            {
                rangeEnemy.TransitionToState(rangeEnemy.IdleState);
            }
            else if (projectileIsReady && CanSeePlayer(rangeEnemy))
            {
                rangeEnemy.StartCoroutine(FireProjectileRoutine(rangeEnemy));
            }
        }

        private bool CanSeePlayer(RangeEnemyBehaviour rangeEnemy)
        {
            RaycastHit2D raycastInfo = Physics2D.Raycast(rangeEnemy.transform.position, rangeEnemy.Player.position - rangeEnemy.transform.position,
                rangeEnemy.ExteriorDistance, rangeEnemy.PlayerAndObstacle);

            if (raycastInfo.collider != null && raycastInfo.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerator FireProjectileRoutine(RangeEnemyBehaviour rangeEnemy)
        {
            projectileIsReady = false;

            FireProjectile(rangeEnemy);

            yield return new WaitForSeconds(rangeEnemy.TimeBetweenProjectiles);
            projectileIsReady = true;
        }

        private void FireProjectile(RangeEnemyBehaviour rangeEnemy)
        {
            rangeEnemy.Animator.SetTrigger("isAttacking");

            GameObject projectile = ObjectPooling.Instance.GetAvailableObject("EnemyProjectile");

            projectile.transform.position = rangeEnemy.FirePoint.transform.position;
            projectile.transform.rotation = rangeEnemy.FirePoint.transform.rotation;

            EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
            enemyProjectile.Speed = rangeEnemy.ProjectileSpeed;
            enemyProjectile.Damage = rangeEnemy.ProjectileDamage;

            projectile.SetActive(true);
        }
    }
}