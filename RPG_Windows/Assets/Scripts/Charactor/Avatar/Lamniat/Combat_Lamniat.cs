using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Lamniat : Combat_Avatar
{
    private AvatarInputActionsControls inputControls;

    protected override void Awake() {
        base.Awake();
        attackClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_avatarAnimator, "Lamniat_attack_1_chop_clockwise");
        Debug.Log("Combat_Lamniat Start SetAnimateClipTime attackClipTime: "+attackClipTime);
    }

    protected override void Start() {
        #region InputSystem事件設定
        inputControls = m_avatar.InputCtrl;

        inputControls.Lamniat_Land.Attack.started += content => {
            Debug.Log("Lamniat_Land.Attack.started");
            if(m_avatar.CharStatus.Equals(Charactor.CharactorStatus.Move)) {
                m_avatarMovement.SetFacingDir(m_avatarMovement.Movement);
                //m_avatar.SetFacingDir(m_avatar.Movement);
                movementAfterAttack = m_avatarMovement.Movement;
            }

            isAttacking = true;
            attackRoutine = StartCoroutine(Attack());
        };
        

        #endregion
    }

    protected override void Update() {
        base.Update();
        // if(m_avatar.Status.Equals(Charactor.CharactorStatus.Attack)) AnimeUtils.ActivateAnimatorLayer(m_targetAnimator, "AttackLayer");
        // else AnimeUtils.ActivateAnimatorLayer(m_targetAnimator, "IdleLayer");
    }

    
}
