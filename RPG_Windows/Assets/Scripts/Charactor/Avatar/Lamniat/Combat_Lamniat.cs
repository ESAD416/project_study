using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Combat_Lamniat : Combat_Avatar
{
    [Header("Lamniat戰鬥參數")]
    protected int m_maximumMeleeComboCount = 3;
    protected int m_meleeComboCounter = 0;
    public int MeleeComboCounter => this.m_meleeComboCounter;
    public void SetMeleeComboCounter(int value) => this.m_meleeComboCounter = value;

    [Header("Lamniat戰鬥物件")]
    [SerializeField] protected HitBox_Overlap2D m_hitBox_left;
    [SerializeField] protected HitBox_Overlap2D m_hitBox_right;
    [SerializeField] protected List<VisualEffect> m_slashEffects;
    

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
            // if(m_meleeComboCounter >= m_maximumMeleeComboCount) m_meleeComboCounter = 0;
            // m_meleeComboCounter++;
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
        
        //SetHitboxDir();
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

        m_hitBox_left.gameObject.SetActive(false);
        m_hitBox_right.gameObject.SetActive(false);
        m_slashEffects.ForEach(h => 
        {
            h.gameObject.SetActive(false);
        });

        this.m_meleeComboCounter = 0;


        Debug.Log("FinishMeleeAttack");
    }

    public void SetAnimateCombatPara() 
    {
        m_avatarAnimator.SetBool("isAttacking", IsAttacking);
        m_avatarAnimator.SetBool("isPreAttacking", IsPreAttacking);
        m_avatarAnimator.SetBool("isPostAttacking", IsPostAttacking);
        m_avatarAnimator.SetBool("cancelRecovery", CancelRecovery);
    }

    public void SetHitboxDir() {
        Debug.Log("SetHitboxDir m_avatarMovement.FacingDir: "+m_avatarMovement.FacingDir);
        Debug.Log("SetHitboxDir m_meleeComboCounter: "+m_meleeComboCounter);
        string objName = string.Empty;
        if(m_avatarMovement.FacingDir.x < 0) {
            // Left
            objName = "VFXGraph_slash_combo"+ m_meleeComboCounter +"_left";
            UpdateHitboxsEnabled(m_hitBox_left, objName);
            
        } else if(m_avatarMovement.FacingDir.x > 0) {
            // Right
            objName = "VFXGraph_slash_combo"+ m_meleeComboCounter +"_right";
            UpdateHitboxsEnabled(m_hitBox_right, objName);
        } 
    }
    
    private void UpdateHitboxsEnabled(HitBox_Overlap2D hitbox, string slashEffectObjName)
    {
        hitbox.gameObject.SetActive(true);
        m_slashEffects.ForEach(h => 
        {
            if(h.name.Equals(slashEffectObjName)) h.gameObject.SetActive(true);
            else h.gameObject.SetActive(false);
        });
    }

    private void UpdateSlashEffect() {

    }
    
}
