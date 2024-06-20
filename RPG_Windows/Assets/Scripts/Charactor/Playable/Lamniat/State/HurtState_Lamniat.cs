using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_Lamniat : BaseStateMachine_Player
{
    public HurtState_Lamniat(Player avatar) {
        this.currentPlayer = avatar;
        this.m_bState = Constant.BaseState.Hurt;
    }

    public override void OnEnter()
    {
        // OnEnter
        this.currentPlayer.Animator?.SetTrigger("hurt");
    }
    public override void OnEnter(Player avatar)
    {
        this.currentPlayer = avatar;
        OnEnter();
    }

    public override void OnUpdate()
    {
        //AnimeUtils.ActivateAnimatorLayer(this.currentAvatar.Animator, "TriggerLayer");
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
