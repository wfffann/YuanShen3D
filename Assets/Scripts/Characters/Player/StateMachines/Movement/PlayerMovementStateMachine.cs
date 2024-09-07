using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public PlayerStateReusableData playerStateReusableData;
        public Player player { get;}
        public PlayerIdlingState idlingState { get; }
        public PlayerDashingState dashState { get; }
        public PlayerWalkingState walkingState { get; }
        public PlayerRunningState runningState { get; }
        public PlayerSprintingState sprintingState { get; }
        public PlayerLightStoppingState playerLightStoppingState { get; }
        public PlayerMiddleStoppingState playerMiddleStoppingState { get; }
        public PlayerHardStoppingState playerHardStoppingState { get; }

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
            playerMiddleStoppingState = new PlayerMiddleStoppingState(this);
            playerHardStoppingState = new PlayerHardStoppingState(this);
        }
    }
}
