using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Lamniat : BaseStateMachine_Avatar
{
    public MoveState_Lamniat(Avatar avatar)
    {
        this.currentAvatar = avatar;
        this.m_bState = BaseState.Move;
    }

    public override void OnEnter()
    {
        // OnEnter
    }
    public override void OnEnter(Avatar avatar)
    {
        this.currentAvatar = avatar;
        OnEnter();
    }

    public override void OnUpdate()
    {
        AnimeUtils.ActivateAnimatorLayer(this.currentAvatar.Animator, "MoveLayer");
    }

    public override void OnFixedUpdate()
    {
        // OnFixedUpdate
        //this.currentAvatar.Rigidbody.velocity = this.currentAvatar.AvatarMovement.Movement.normalized * this.currentAvatar.AvatarMovement.MoveSpeed;
    }

    public override void OnExit()
    {
        // OnExit
    }
}