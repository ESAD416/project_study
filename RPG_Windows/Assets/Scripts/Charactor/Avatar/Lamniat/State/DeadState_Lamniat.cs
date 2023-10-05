using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Lamniat : BaseStateMachine_Avatar
{
    public DeadState_Lamniat(Avatar avatar)
    { 
        this.currentAvatar = avatar;
        this.m_bState = BaseState.Dead;
    }

    public override void OnEnter()
    {
        // OnEnter
    }
    public override void OnEnter(Avatar avatar)
    {
        this.currentAvatar = avatar;
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
