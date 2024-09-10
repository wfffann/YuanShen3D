using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public interface IState
    {
        public void Enter();
        public void Exit();

        public void HandleInput();//任何输入逻辑相关的

        public void Update();

        public void PhysicsUpdate();

        public void OnAnimationEnterEvent();

        public void OnAnimationExitEvent();

        public void OnAnimationTransitionEvent();

        public void OnTriggerEnter(Collider collider);
        public void OnTriggerExit(Collider collider);
    }
}
