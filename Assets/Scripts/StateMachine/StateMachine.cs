using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public abstract class StateMachine
    {
        protected IState currentState;

        /// <summary>
        /// ÇÐ»»×´Ì¬
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(IState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void HandleInput()
        {
            currentState?.HandleInput();
        }

        public void Update()
        {
            currentState?.Update();
        }

        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }

        public void OnAnimationEnterEvent()
        {
            currentState?.OnAnimationEnterEvent();
        }
        public void OnAnimationExitEvent()
        {
            currentState?.OnAnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            currentState?.OnAnimationTransitionEvent();
        }

        public void OnTriggerEnter(Collider collider)
        {
            currentState?.OnTriggerEnter(collider);
        }

        public void OnTriggerExit(Collider collider)
        {
            currentState?.OnTriggerExit(collider);
        }
    }
}
