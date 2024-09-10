using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public PlayerStateReusableData playerStateReusableData;
        public Player player { get;}

        //Ground
        public PlayerIdlingState idlingState { get; }
        public PlayerDashingState dashState { get; }
        public PlayerWalkingState walkingState { get; }
        public PlayerRunningState runningState { get; }
        public PlayerSprintingState sprintingState { get; }


        public PlayerLightLandingState playerLightLandingState { get; }
        public PlayerHardLandingState playerHardLandingState { get; }
        public PlayerRollingState playerRollingState { get; }

        //Stop
        public PlayerLightStoppingState playerLightStoppingState { get; }
        public PlayerMiddleStoppingState playerMediumStoppingState { get; }
        public PlayerHardStoppingState playerHardStoppingState { get; }

        //Airborne
        public PlayerJumpState playerJumpState { get; }
        public PlayerFallingState playerFallingState { get; }

        /// <summary>
        /// 构造函数(创建这个类实例时会创建里面的State实例
        /// </summary>
        public PlayerMovementStateMachine(Player _player)
        {
            playerStateReusableData = new PlayerStateReusableData();

            player = _player;

            idlingState = new PlayerIdlingState(this);

            dashState = new PlayerDashingState(this);

            //MovingState
            walkingState = new PlayerWalkingState(this);
            runningState = new PlayerRunningState(this);
            sprintingState = new PlayerSprintingState(this);

            //StoppingState
            playerLightStoppingState = new PlayerLightStoppingState(this);
            playerMediumStoppingState = new PlayerMiddleStoppingState(this);
            playerHardStoppingState = new PlayerHardStoppingState(this);

            //AirborneState
            playerJumpState = new PlayerJumpState(this);
            playerFallingState = new PlayerFallingState(this);

            //LandingState
            playerLightLandingState = new PlayerLightLandingState(this);
            playerHardLandingState = new PlayerHardLandingState(this);
            playerRollingState = new PlayerRollingState(this);
        }
    }
}
