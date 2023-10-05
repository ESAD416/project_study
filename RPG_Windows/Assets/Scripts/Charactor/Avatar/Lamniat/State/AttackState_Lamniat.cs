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
    }
    public override void OnEnter(Avatar avatar)
    {
        this.currentAvatar = avatar;
    }

    public override void OnUpdate()
    {
        AnimeUtils.ActivateAnimatorLayer(currentAvatar.Animator, "AttackLayer");
    }

    public override void OnFixedUpdate()
    {
       currentAvatar.AvatarMovement.SetMovement(Vector3.zero);
    }

    public override void OnExit()
    {
        // OnExit
    }
}
