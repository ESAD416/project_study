using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Lamniat : BaseStateMachine_Avatar
{
    public IdleState_Lamniat(Avatar avatar) 
    {
        this.currentAvatar = avatar;
        this.m_bState = BaseState.Idle;
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
        AnimeUtils.ActivateAnimatorLayer(currentAvatar.Animator, "IdleLayer");
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
