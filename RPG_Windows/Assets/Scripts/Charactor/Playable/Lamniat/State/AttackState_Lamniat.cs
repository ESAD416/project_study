using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Lamniat : BaseStateMachine_Player
{
    public AttackState_Lamniat(Player avatar) 
    {
        this.currentPlayer = avatar;
        this.m_bState = Constant.BaseState.Attack;
    }


    public override void OnEnter()
    {
        // OnEnter
        //this.currentAvatar.Animator?.SetBool("isAttack", true);
    }
    public override void OnEnter(Player avatar)
    {
        this.currentPlayer = avatar;
        OnEnter();
    }

    public override void OnUpdate()
    {
        // var velocity = this.currentAvatar.AvatarMovement.MoveVelocity;
        // var moveSpeed = this.currentAvatar.AvatarMovement.MoveSpeed;
        // this.currentAvatar.AvatarMovement.SetMoveVelocity(Vector2.ClampMagnitude(velocity*0f, moveSpeed*0f));
        
        AnimeUtils.ActivateAnimatorLayer(this.currentPlayer.Animator, "AttackLayer");
    }

    public override void OnFixedUpdate()
    {
       //this.currentAvatar.AvatarMovement.SetMovement(Vector3.zero);
    }

    public override void OnExit()
    {
        // OnExit
    }
}
