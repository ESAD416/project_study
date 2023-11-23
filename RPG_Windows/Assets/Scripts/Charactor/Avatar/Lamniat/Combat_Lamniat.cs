using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Combat_Lamniat : Combat_Avatar
{
    protected int m_maximumMeleeComboCount = 3;
    protected int m_meleeComboCounter = 0;
    public int MeleeComboCounter => this.m_meleeComboCounter;
    public void SetMeleeComboCounter(int value) => this.m_meleeComboCounter = value;
    [SerializeField] protected GameObject slashEffectPrefeb;
    

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
            if(m_meleeComboCounter >= m_maximumMeleeComboCount) m_meleeComboCounter = 0;
            m_meleeComboCounter++;
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
        Quaternion quaternion = Quaternion.identity;
        if(m_avatarMovement.FacingDir.x == 0 && m_avatarMovement.FacingDir.y > 0) {
            // Up
            if(MeleeComboCounter == 2) quaternion = Quaternion.Euler(0f , 0f , 90f);
            else quaternion = Quaternion.Euler(0f , 180f , 90f);
            UpdateHitboxsEnabled("Melee_Up", quaternion);
        } else if(m_avatarMovement.FacingDir.x == 0 && m_avatarMovement.FacingDir.y < 0) {
            // Down 
            if(m_meleeComboCounter == 2) quaternion = Quaternion.Euler(0f , 0f , -90f);
            else quaternion = Quaternion.Euler(0f , 180f , -90f);
            UpdateHitboxsEnabled("Melee_Down", quaternion);
        } else if(m_avatarMovement.FacingDir.x < 0 && m_avatarMovement.FacingDir.y == 0) {
            // Left
            if(m_meleeComboCounter == 2) quaternion = Quaternion.Euler(0f , 180f , 0f);
            else quaternion = Quaternion.Euler(180f , 180f , 0f);
            UpdateHitboxsEnabled("Melee_Left", quaternion);
        } else if(m_avatarMovement.FacingDir.x > 0 && m_avatarMovement.FacingDir.y == 0) {
            // Right
            if(m_meleeComboCounter == 2) quaternion = Quaternion.Euler(0f , 0f , 0f);
            else quaternion = Quaternion.Euler(180f , 0f , 0f);
            UpdateHitboxsEnabled("Melee_Right", quaternion);
        } 
    }


    
    private void UpdateHitboxsEnabled(string gameObjectName, Quaternion quaternion) 
    {
        m_hitBoxs.ForEach(h => 
        {
            if(h.name.Equals(gameObjectName)) {
                h.gameObject.SetActive(true);

                var existingSlashEffect = h.gameObject.transform.Find("VFXGraph_slash");
                if(existingSlashEffect != null) {
                    existingSlashEffect.transform.rotation = quaternion;
                }
            }
            else h.gameObject.SetActive(false);
        });
    }

    private void UpdateSlashEffect() {

    }
    
}
