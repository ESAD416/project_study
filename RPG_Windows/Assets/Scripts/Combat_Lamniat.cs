using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Lamniat : Combat_Avatar
{
    private AvatarInputActionsControls inputControls;

    protected void Awake() {
        attackClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_targetAnimator, "Lamniat_attack_1_chop_clockwise");
        Debug.Log("Combat_Lamniat Start SetAnimateClipTime attackClipTime: "+attackClipTime);
    }

    protected override void Start() {
        #region InputSystem事件設定
        inputControls = m_avatar.InputCtrl;

        inputControls.Lamniat_Land.Attack.started += content => {
            Debug.Log("Lamniat_Land.Attack.started");
            if(m_avatar.isMoving) {
                m_avatar.SetFacingDir(m_avatar.Movement);
                movementAfterAttack = m_avatar.Movement;
            }

            isAttacking = true;
            m_avatar.SetStatus(Charactor.CharactorStatus.Attack);
            attackRoutine = StartCoroutine(Attack());
        };
        

        #endregion
    }

    protected override void Update() {
        if(isAttacking) AnimeUtils.ActivateAnimatorLayer(m_targetAnimator, "AttackLayer");
    }

    
}
