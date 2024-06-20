using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Lamniat : BaseStateMachine_Player
{
    public IdleState_Lamniat(Player avatar) 
    {
        this.currentPlayer = avatar;
        this.m_bState = Constant.BaseState.Idle;
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
        AnimeUtils.ActivateAnimatorLayer(this.currentPlayer.Animator, "IdleLayer");
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
