using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [RequireComponent(typeof(PlayerInput))]//他会自动添加这个脚本
    public class Player : MonoBehaviour
    {
        //Data
        [field: Header("References")]
        [field: SerializeField] public PlayerSO playerData {  get; private set; }

        [field: Header("Collisions")]
        [field: SerializeField] public CapsulColliderUtility colliderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData playerLayerData { get; private set; }

        //脚本
        public PlayerInput input {  get; private set; }

        private PlayerMovementStateMachine movementStateMachine;

        //组件
        public Rigidbody rb {  get; private set; }
        public Transform mainCameraTransform { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();

            //碰撞体初始化数据
            colliderUtility.Initialize(gameObject);

            //获取当前碰撞体的数据
            colliderUtility.CalculateCapsuleColliderDimensions();

            mainCameraTransform = Camera.main.transform;

            movementStateMachine = new PlayerMovementStateMachine(this);//伴随他的构造函数
        }

        private void OnValidate()
        {
            //碰撞体初始化数据
            colliderUtility.Initialize(gameObject);

            //获取当前碰撞体的数据
            colliderUtility.CalculateCapsuleColliderDimensions();
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

        public void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }

        private void Update()
        {
            movementStateMachine.HandleInput();

            movementStateMachine.Update();
        }
    }
}
