using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [RequireComponent(typeof(PlayerInput))]//�����Զ��������ű�
    public class Player : MonoBehaviour
    {
        //�ű�
        public PlayerInput input {  get; private set; }

        private PlayerMovementStateMachine movementStateMachine;

        //���
        public Rigidbody rb {  get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();

            movementStateMachine = new PlayerMovementStateMachine(this);//�������Ĺ��캯��
        }

        private void Start()
        {
            //��ʼ����ֵState(һ��ı���CurrentState(��������֡�߼�
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
