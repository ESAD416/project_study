using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Lamniat : BaseStateMachine_Player
{
    public MoveState_Lamniat(Player avatar)
    {
        this.currentPlayer = avatar;
        this.m_bState = Constant.BaseState.Move;
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
        AnimeUtils.ActivateAnimatorLayer(this.currentPlayer.Animator, "MoveLayer");
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
