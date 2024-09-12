using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YuanShenImpactMovementSystem
{
    public class PlayerLandingState : PlayerGroundedState
    {
        public PlayerLandingState(PlayerMovementStateMachine _playerMovementStateMachine) : base(_playerMovementStateMachine)
        {

        }

        #region IState Methods

        #endregion

        #region Input Methods
        //protected override void OnMovementCanceled(InputAction.CallbackContext context)
        //{
            
        //}
        #endregion
    }
}
