using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Lamniat : BaseStateMachine_Player
{
    public DeadState_Lamniat(Player avatar)
    { 
        this.currentPlayer = avatar;
        this.m_bState = Constant.BaseState.Dead;
    }

    public override void OnEnter()
    {
        // OnEnter
    }
    public override void OnEnter(Player avatar)
    {
        this.currentPlayer = avatar;
        OnEnter();
    }

    public override void OnUpdate()
    {
        // OnUpdate
    }

    public override void OnFixedUpdate()
    {
        // OnFixedUpdate
    }

    public override void OnExit()
    {
        // OnExit
    }
}
