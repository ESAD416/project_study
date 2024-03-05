using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState_Lamniat : BaseStateMachine_Avatar
{
    public JumpState_Lamniat(Avatar avatar) {
        this.currentAvatar = avatar;
        this.m_bState = BaseState.Jump;
    }

    public override void OnEnter()
    {
        // OnEnter
        this.currentAvatar.Animator?.SetTrigger("jump");
    }
    public override void OnEnter(Avatar avatar)
    {
        this.currentAvatar = avatar;
        OnEnter();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        AnimeUtils.ActivateAnimatorLayer(this.currentAvatar.Animator, "TriggerLayer");
    }

    public override void OnFixedUpdate()
    {
    }

}
