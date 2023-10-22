using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Lamniat : Dodge_Avatar
{
    protected override void Awake() {
        base.Awake();
        dodgeClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_avatarAnimator, "Lamniat_dodge");
        // Debug.Log("Combat_Lamniat Start SetAnimateClipTime attackClipTime: "+attackClipTime);
    }

    protected override void Start() {
        inputControls = m_avatar.InputCtrl;

        inputControls.Lamniat_Land.Dodge.started += content => {
            Debug.Log("Lamniat_Land.Dodge.started");
            // if(m_avatar.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Move)) {
            //     m_avatarMovement.SetFacingDir(m_avatarMovement.Movement);
            //     movementAfterDodge = m_avatarMovement.Movement;
            // }

            m_avatarAnimator?.SetTrigger("dodge");
            m_avatar.SetCurrentBaseState(m_avatar.Dodge);
        };
    }

}
