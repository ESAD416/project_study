using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Lamniat : BaseStateMachine_Avatar
{
    public AttackState_Lamniat(Avatar avatar) 
    {
        this.currentAvatar = avatar;
        this.m_bState = BaseState.Attack;
    }


    public override void OnEnter()
    {
        // OnEnter
        //this.currentAvatar.Animator?.SetBool("isAttack", true);
    }
    public override void OnEnter(Avatar avatar)
    {
        this.currentAvatar = avatar;
        OnEnter();
    }

    public override void OnUpdate()
    {
        // var velocity = this.currentAvatar.AvatarMovement.MoveVelocity;
        // var moveSpeed = this.currentAvatar.AvatarMovement.MoveSpeed;
        // this.currentAvatar.AvatarMovement.SetMoveVelocity(Vector2.ClampMagnitude(velocity*0f, moveSpeed*0f));
        
        AnimeUtils.ActivateAnimatorLayer(this.currentAvatar.Animator, "AttackLayer");
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
