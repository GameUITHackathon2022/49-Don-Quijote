using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LittleFoxLite
{
    public enum PlayerStateType
    {
        Event = 0, Normal = 1, Shoot = 2
    }
    public class StateManager
    {
        PlayerStateType currentType;
        public PlayerStateType CurrentType { get => currentType; private set => currentType = value; }
        BaseState currentState;
        NormalState normalState;
        EventState eventState;
        ShootState shootState;
        PlayerController player;


        public StateManager(PlayerController pplayer, PlayerStateType type = PlayerStateType.Normal)
        {
            List<BaseState> states = new List<BaseState>();
            eventState = new EventState();
            normalState = new NormalState();
            shootState = new ShootState();
            currentType = type;
            player = pplayer;
            currentState = chooseState(type);
            currentState.EnterState(player);
            
        }
        private BaseState chooseState(PlayerStateType type)
        {
            switch (type)
            {
                case PlayerStateType.Normal:
                    return normalState;
                case PlayerStateType.Event:
                    return eventState;
                case PlayerStateType.Shoot:
                    return shootState;
                default:
                    return normalState;
            }

        }
        public void Update()
        {
            currentState.UpdateState(player);
        }
        public void SwitchState(PlayerStateType type)
        {
            if (type == currentType) return;
            currentState.ExitState(player);
            currentType = type;
            currentState = chooseState(type);
            currentState.EnterState(player);
        }
    }
}
