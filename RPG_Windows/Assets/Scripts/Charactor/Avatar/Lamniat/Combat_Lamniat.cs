using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Lamniat : Combat_Avatar
{
    protected override void Awake() {
        base.Awake();
        m_attackClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_avatarAnimator, "Lamniat_attack_1_chop_clockwise");
        // Debug.Log("Combat_Lamniat Start SetAnimateClipTime attackClipTime: "+attackClipTime);
    }

    protected override void Start() {
        #region InputSystem事件設定
        m_inputControls = m_avatar.InputCtrl;

        m_inputControls.Lamniat_Land.Attack.started += content => {
            // Debug.Log("Lamniat_Land.Attack.started");
            if(m_avatar.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Move)) {
                m_avatarMovement.SetFacingDir(m_avatarMovement.Movement);
                //m_avatar.SetFacingDir(m_avatar.Movement);
                m_avatarMovement.SetMovementAfterTrigger(m_avatarMovement.Movement);
            }

            IsAttacking = true;
            m_attackRoutine = StartCoroutine(Attack());
        };
        

        #endregion
    }
}
