using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    [RequireComponent(typeof(PlayerInput))]//�����Զ��������ű�
    public class Player : MonoBehaviour
    {
        //Data
        [field: Header("References")]
        [field: SerializeField] public PlayerSO playerData {  get; private set; }

        [field: Header("Collisions")]
        [field: SerializeField] public CapsulColliderUtility colliderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData playerLayerData { get; private set; }

        //�ű�
        public PlayerInput input {  get; private set; }

        private PlayerMovementStateMachine movementStateMachine;

        //���
        public Rigidbody rb {  get; private set; }
        public Transform mainCameraTransform { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<PlayerInput>();

            //��ײ���ʼ������
            colliderUtility.Initialize(gameObject);

            //��ȡ��ǰ��ײ�������
            colliderUtility.CalculateCapsuleColliderDimensions();

            mainCameraTransform = Camera.main.transform;

            movementStateMachine = new PlayerMovementStateMachine(this);//�������Ĺ��캯��
        }

        private void OnValidate()
        {
            //��ײ���ʼ������
            colliderUtility.Initialize(gameObject);

            //��ȡ��ǰ��ײ�������
            colliderUtility.CalculateCapsuleColliderDimensions();
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
