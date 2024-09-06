using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions inputActions {  get; private set; }
        public PlayerInputActions.PlayerActions playerActions { get; private set; }//由Unity自动生成(Player/Actions结合

        private void Awake()
        {
            inputActions = new PlayerInputActions();

            playerActions = inputActions.Player;//赋值变量名称（寻找这个Player命名的ActionMap
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
        /// 开始Dash冲刺的协程
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        public void DishingActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }

        /// <summary>
        /// Dash冲刺按键的禁用延时
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
