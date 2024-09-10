using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public interface IState
    {
        public void Enter();
        public void Exit();

        public void HandleInput();//�κ������߼���ص�

        public void Update();

        public void PhysicsUpdate();

        public void OnAnimationEnterEvent();

        public void OnAnimationExitEvent();

        public void OnAnimationTransitionEvent();

        public void OnTriggerEnter(Collider collider);
        public void OnTriggerExit(Collider collider);
    }
}
