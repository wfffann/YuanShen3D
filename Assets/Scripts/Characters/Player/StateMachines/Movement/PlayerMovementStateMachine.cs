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

        /// <summary>
        /// ���캯��(���������ʵ��ʱ�ᴴ�������Stateʵ��
        /// </summary>
        public PlayerMovementStateMachine(Player _player)
        {
            playerStateReusableData = new PlayerStateReusableData();

            player = _player;

            idlingState = new PlayerIdlingState(this);
            dashState = new PlayerDashingState(this);
            walkingState = new PlayerWalkingState(this);
            runningState = new PlayerRunningState(this);
            sprintingState = new PlayerSprintingState(this);
        }
    }
}
