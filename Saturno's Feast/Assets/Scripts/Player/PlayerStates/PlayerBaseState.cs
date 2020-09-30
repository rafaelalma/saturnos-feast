using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefactoredProject
{
    public abstract class PlayerBaseState
    {
        public abstract void EnterState(PlayerBehaviour player);

        public abstract void Update(PlayerBehaviour player);

        public abstract void ShouldTransitionToTakeDamageState(PlayerBehaviour player);

        protected virtual void TransitionToIdleState(PlayerBehaviour player, string animatorParameter, bool isActive = false)
        {
            player.Animator.SetBool(animatorParameter, isActive);

            player.TransitionToState(player.IdleState);
        }

        protected virtual void TransitionToRunState(PlayerBehaviour player, string animatorParameter, bool isActive = false)
        {
            player.Animator.SetBool(animatorParameter, isActive);

            player.TransitionToState(player.RunState);
        }

        protected virtual void TransitionToMeleeAttackState(PlayerBehaviour player, string animatorParameter, bool isActive = false)
        {
            player.Animator.SetBool(animatorParameter, isActive);

            player.TransitionToState(player.MeleeAttackState);
        }

        protected virtual void TransitionToBlockState(PlayerBehaviour player, string animatorParameter, bool isActive = false)
        {
            player.Animator.SetBool(animatorParameter, isActive);

            player.TransitionToState(player.BlockState);
        }

        protected virtual void TransitionToDashState(PlayerBehaviour player, string animatorParameter, bool isActive = false)
        {
            player.Animator.SetBool(animatorParameter, isActive);

            player.TransitionToState(player.DashState);
        }

        protected virtual void TransitionToStrongRangeAttackState(PlayerBehaviour player, string animatorParameter, bool isActive = false)
        {
            player.Animator.SetBool(animatorParameter, isActive);

            player.TransitionToState(player.StrongRangeAttackState);
        }

        protected virtual void TransitionToTakeDamageState(PlayerBehaviour player, string animatorParameter, bool isActive = false)
        {
            player.Animator.SetBool(animatorParameter, isActive);

            player.TransitionToState(player.TakeDamageState);
        }

        protected virtual void TransitionToDeadState(PlayerBehaviour player, string animatorParameter, bool isActive = false)
        {
            player.Animator.SetBool(animatorParameter, isActive);

            player.TransitionToState(player.DeadState);
        }

        protected virtual void TransitionToRespawnState(PlayerBehaviour player, string animatorParameter, bool isActive = false)
        {
            player.Animator.SetBool(animatorParameter, isActive);

            player.TransitionToState(player.RespawnState);
        }
    }
}