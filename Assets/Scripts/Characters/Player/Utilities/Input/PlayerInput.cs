using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        /// <summary>
        /// ��ʼDash��̵�Э��
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        public void DishingActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }

        /// <summary>
        /// Dash��̰����Ľ�����ʱ
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();

            yield return new WaitForSeconds(seconds);

            action.Enable();
        }
    }
}
