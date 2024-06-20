using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState_Lamniat : BaseStateMachine_Player
{

    public DodgeState_Lamniat(Player avatar) {
        this.currentPlayer = avatar;
        this.m_bState = Constant.BaseState.Dodge;
    }

    public override void OnEnter()
    {
        // OnEnter
        this.currentPlayer.Animator?.SetTrigger("dodge");
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
        this.currentPlayer.AvatarMovement.SetMovement(Vector3.zero);
    }

    public override void OnExit()
    {
        // OnExit
    }
}
