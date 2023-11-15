using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Combat_Lamniat : Combat_Avatar
{
    protected int maximumMeleeComboCount = 3;
    protected int meleeComboCounter = 0;

    bool nextAttack2 = false;
    bool nextAttack3 = false;
    

    protected override void Awake() {
        base.Awake();
        m_attackClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_avatarAnimator, "Lamniat_attack_1_chop_clockwise");
        // Debug.Log("Combat_Lamniat Start SetAnimateClipTime attackClipTime: "+attackClipTime);
    }

    protected override void Start() {
        #region InputSystem事件設定
        m_inputControls = m_avatar.InputCtrl;

        m_inputControls.Lamniat_Land.Attack.performed += content => {
            Debug.Log("Lamniat_Land.Attack.started");
            if(m_avatar.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Move)) {
                m_avatarMovement.SetFacingDir(m_avatarMovement.Movement);
                //m_avatar.SetFacingDir(m_avatar.Movement);
                m_avatarMovement.SetMovementAfterTrigger(m_avatarMovement.Movement);
            }

            MeleeAttack();
        };

        #endregion
    }

    protected override void Update() {
        base.Update();

        SetAnimateCombatPara();
    }

    private void MeleeAttack() 
    {
        if(!IsAttacking) IsAttacking = true;
        if(!m_avatar.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Attack)) m_avatar.SetCurrentBaseState(m_avatar.Attack);
        
        SetHitboxDir();
        m_avatarAnimator.SetTrigger("melee");
    }

    public void FinishMeleeAttack() 
    {
        IsAttacking = false;
        IsPreAttacking = false;
        IsPostAttacking = false;
        CancelRecovery = false;

        m_avatarMovement.SetMovement(m_avatarMovement.MovementAfterTrigger);
        m_avatarMovement.SetMovementAfterTrigger(Vector3.zero);
        
        if(m_avatarMovement.IsMoving) m_avatar.SetCurrentBaseState(m_avatar.Move);
        else m_avatar.SetCurrentBaseState(m_avatar.Idle);

        Debug.Log("FinishMeleeAttack");
    }

    public void SetAnimateCombatPara() 
    {
        m_avatarAnimator.SetBool("isAttacking", IsAttacking);
        m_avatarAnimator.SetBool("isPreAttacking", IsPreAttacking);
        m_avatarAnimator.SetBool("isPostAttacking", IsPostAttacking);
        m_avatarAnimator.SetBool("cancelRecovery", CancelRecovery);
    }

    private void SetHitboxDir() {
        if(m_avatarMovement.FacingDir.x == 0 && m_avatarMovement.FacingDir.y > 0) {
            // Up
            UpdateHitboxsEnabled("Melee_Up");
        } else if(m_avatarMovement.FacingDir.x == 0 && m_avatarMovement.FacingDir.y < 0) {
            // Down 
            UpdateHitboxsEnabled("Melee_Down");
        } else if(m_avatarMovement.FacingDir.x < 0 && m_avatarMovement.FacingDir.y == 0) {
            // Left
            UpdateHitboxsEnabled("Melee_Left");
        } else if(m_avatarMovement.FacingDir.x > 0 && m_avatarMovement.FacingDir.y == 0) {
            // Right
            UpdateHitboxsEnabled("Melee_Right");
        } 
    }
    
    private void UpdateHitboxsEnabled(string gameObjectName) {
        m_hitBoxs.ForEach(h => 
        {
            if(h.name.Equals(gameObjectName)) h.gameObject.SetActive(true);
            else h.gameObject.SetActive(false);
        });
    }
    
}
