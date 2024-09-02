using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
