using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions inputActions {  get; private set; }
        public PlayerInputActions.PlayerActions playerActions { get; private set; }//��Unity�Զ�����(Player/Actions���

        private void Awake()
        {
            inputActions = new PlayerInputActions();

            playerActions = inputActions.Player;//��ֵ�������ƣ�Ѱ�����Player������ActionMap
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }
    }
}
