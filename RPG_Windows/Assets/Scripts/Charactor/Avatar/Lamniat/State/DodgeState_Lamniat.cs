using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState_Lamniat : BaseStateMachine_Avatar
{

    public DodgeState_Lamniat(Avatar avatar) {
        this.currentAvatar = avatar;
        this.m_bState = BaseState.Dodge;
    }

    public override void OnEnter()
    {
        // OnEnter
        this.currentAvatar.Animator?.SetTrigger("dodge");
    }

    public override void OnEnter(Avatar avatar)
    {
        this.currentAvatar = avatar;
        OnEnter();
    }

    public override void OnUpdate()
    {
        AnimeUtils.ActivateAnimatorLayer(this.currentAvatar.Animator, "TriggerLayer");
    }

    public override void OnFixedUpdate()
    {
        this.currentAvatar.AvatarMovement.SetMovement(Vector3.zero);
    }

    public override void OnExit()
    {
        // OnExit
    }
}