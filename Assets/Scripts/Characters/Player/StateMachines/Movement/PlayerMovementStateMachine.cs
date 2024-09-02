using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YuanShenImpactMovementSystem
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public Player player { get;}
        public PlayerIdlingState idlingState { get; }
        public PlayerWalkingState walkingState { get; }
        public PlayerRunningState runningState { get; }
        public PlayerSprintingState sprintingState { get; }

        /// <summary>
        /// 构造函数(创建这个类实例时会创建里面的State实例
        /// </summary>
        public PlayerMovementStateMachine(Player _player)
        {
            player = _player;

            idlingState = new PlayerIdlingState(this);

            walkingState = new PlayerWalkingState(this);
            runningState = new PlayerRunningState(this);
            sprintingState = new PlayerSprintingState(this);
        }
    }
}
