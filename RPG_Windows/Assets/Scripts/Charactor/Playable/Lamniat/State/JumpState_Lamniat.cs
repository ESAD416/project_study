using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState_Lamniat : BaseStateMachine_Player
{
    public JumpState_Lamniat(Player avatar) {
        this.currentPlayer = avatar;
        this.m_bState = Constant.BaseState.Jump;
    }

    public override void OnEnter()
    {
        // OnEnter
        this.currentPlayer.Animator?.SetTrigger("jump");
    }
    public override void OnEnter(Player avatar)
    {
        this.currentPlayer = avatar;
        OnEnter();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        AnimeUtils.ActivateAnimatorLayer(this.currentPlayer.Animator, "TriggerLayer");
    }

    public override void OnFixedUpdate()
    {
    }

}
