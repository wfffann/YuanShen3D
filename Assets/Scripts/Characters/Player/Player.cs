using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [RequireComponent(typeof(PlayerInput))]//他会自动添加这个脚本
    public class Player : MonoBehaviour
    {
        //脚本
        public PlayerInput input {  get; private set; }

        private PlayerMovementStateMachine movementStateMachine;

        //组件
        public Rigidbody rb {  get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();

            movementStateMachine = new PlayerMovementStateMachine(this);//伴随他的构造函数
        }

        private void Start()
        {
            //初始化赋值State(一齐改变了CurrentState(方便后面的帧逻辑
            movementStateMachine.ChangeState(movementStateMachine.idlingState);
        }

        private void FixedUpdate()
        {
            movementStateMachine.PhysicsUpdate();
        }

        private void Update()
        {
            movementStateMachine.HandleInput();

            movementStateMachine.Update();
        }
    }
}
