using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Combat_Lamniat : Combat_Avatar
{
    [Header("Lamniat近戰參數")]
    protected int m_maximumMeleeComboCount = 3;
    protected int m_meleeComboCounter = 0;
    [SerializeField] protected int MeleeComboCounter => this.m_meleeComboCounter;
    public void SetMeleeComboCounter(int value) => this.m_meleeComboCounter = value;

    [Header("Lamniat遠程參數")]
    public bool IsShooting;

    [Header("Lamniat近戰物件")]
    [SerializeField] protected HitBox_Overlap2D m_hitBox_left;
    [SerializeField] protected HitBox_Overlap2D m_hitBox_right;
    [SerializeField] protected List<VisualEffect> m_slashEffects_left;
    [SerializeField] protected List<VisualEffect> m_slashEffects_right;

    [Header("Lamniat遠程物件")]
    [SerializeField] protected Transform m_firePoint;
    [SerializeField] protected GameObject m_bulletPrefab;
    

    protected override void Awake() {
        base.Awake();
        m_attackClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_avatarAnimator, "Lamniat_attack_1_chop_clockwise");
        // Debug.Log("Combat_Lamniat Start SetAnimateClipTime attackClipTime: "+attackClipTime);
    }

    protected override void Start() {
        #region InputSystem事件設定
        m_inputControls = m_avatar.InputCtrl;

        m_inputControls.Lamniat_Land.Melee.performed += content => {
            Debug.Log("Lamniat_Land.Melee.started");
            SetToAttackState();

            m_avatarAnimator.SetTrigger("melee");
        };

        m_inputControls.Lamniat_Land.Shoot.performed += content => {
            Debug.Log("Lamniat_Land.Shoot.started");
            SetToAttackState();

            m_avatarAnimator.SetTrigger("shoot");
            IsShooting = true;
        };

        m_inputControls.Lamniat_Land.Shoot_Hold.performed += content => {
            Debug.Log("Lamniat_Land.Shoot_Hold.started");
            //m_isHoldShoot = true;
            
        };

        m_inputControls.Lamniat_Land.Shoot_Hold.canceled += content => {
            Debug.Log("Lamniat_Land.Shoot_Hold.canceled");
            //m_isHoldShoot = false;
        };

        // m_inputControls.Lamniat_Land.Aim.performed += content => {
        //     Debug.Log("Lamniat_Land.Aim.started");
        //     SetToAttackState();

        //     IsAiming = true;
        //     m_avatarAnimator.SetBool("isTakingAim", IsAiming);
        // };
        

        // m_inputControls.Lamniat_Land.Aim.canceled += content => {
        //     Debug.Log("Lamniat_Land.Aim.end");
        //     m_avatarMovement.CanMove = true;
        //     IsAttacking = false;
        //     IsAiming = false;

        //     if(m_avatarMovement.IsMoving) m_avatar.SetCurrentBaseState(m_avatar.Move);
        //     else m_avatar.SetCurrentBaseState(m_avatar.Idle);

        //     m_avatarAnimator.SetBool("isTakingAim", IsAiming);
        // };

        #endregion
    }

    protected override void Update() {
        base.Update();

        SetAnimateCombatPara();
    }

    protected void SetToAttackState() {
        if(m_avatar.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Move)) {
            m_avatarMovement.SetFacingDir(m_avatarMovement.Movement);
            //m_avatar.SetFacingDir(m_avatar.Movement);
            m_avatarMovement.SetMovementAfterTrigger(m_avatarMovement.Movement);
        }

        m_avatarMovement.CanMove = false;
        if(!IsAttacking) IsAttacking = true;
        if(!m_avatar.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Attack)) m_avatar.SetCurrentBaseState(m_avatar.Attack);
    }

    public void Melee() {
        //TODO Set Melee para
    }

    public void Shoot() {
        IsShooting =  true;
        GameObject bullet = Instantiate(m_bulletPrefab, m_firePoint.position, m_firePoint.rotation);
        bullet.GetComponent<Projectile_Bullet>().SetDirection(m_avatarMovement.FacingDir);
        var angle = Vector3.Angle(m_avatarMovement.FacingDir, bullet.GetComponent<Projectile_Bullet>().referenceAxis);
        var quaternion = m_avatarMovement.FacingDir.x > 0 ? Quaternion.Euler(0, 0, -angle) : Quaternion.Euler(0, 0, angle);
        bullet.transform.rotation = quaternion;
    }

    public void FinishMelee() 
    {
        Debug.Log("FinishMeleeAttack start"); 
        IsAttacking = false;
        IsPreAttacking = false;
        IsPostAttacking = false;
        CancelRecovery = false;
        m_avatarMovement.CanMove = true;

        //m_avatarMovement.SetMovement(m_avatarMovement.MovementAfterTrigger);
        m_avatarMovement.SetMovementAfterTrigger(Vector3.zero);
        
        if(m_avatarMovement.IsMoving) m_avatar.SetCurrentBaseState(m_avatar.Move);
        else m_avatar.SetCurrentBaseState(m_avatar.Idle);

        m_hitBox_left.gameObject.SetActive(false);
        m_hitBox_right.gameObject.SetActive(false);
        m_slashEffects_left.ForEach(h => 
        {
            h.gameObject.SetActive(false);
        });
        m_slashEffects_right.ForEach(h => 
        {
            h.gameObject.SetActive(false);
        });

        // this.m_meleeComboCounter = 0;


        Debug.Log("FinishMeleeAttack end");
    }

    public void FinishShoot() {
        Debug.Log("FinishShoot start"); 
        IsAttacking = false;
        IsShooting =false;
        m_avatarMovement.CanMove = true;

        //m_avatarMovement.SetMovement(m_avatarMovement.MovementAfterTrigger);
        m_avatarMovement.SetMovementAfterTrigger(Vector3.zero);
        
        if(m_avatarMovement.IsMoving) m_avatar.SetCurrentBaseState(m_avatar.Move);
        else m_avatar.SetCurrentBaseState(m_avatar.Idle);

        Debug.Log("FinishShoot end");
    }

    public void SetAnimateCombatPara() 
    {
        m_avatarAnimator.SetBool("isAttacking", IsAttacking);
        m_avatarAnimator.SetBool("isPreAttacking", IsPreAttacking);
        m_avatarAnimator.SetBool("isPostAttacking", IsPostAttacking);
        m_avatarAnimator.SetBool("cancelRecovery", CancelRecovery);
        m_avatarAnimator.SetBool("cancelRecovery", CancelRecovery);
        m_avatarAnimator.SetBool("isFlipX", m_avatar.SprtRenderer.flipX);
        // m_avatarAnimator.SetBool("isHoldShoot", m_isHoldShoot);
        // m_avatarAnimator.SetBool("isTakingAim", IsAiming);
    }

    public void SetMeleeHitboxDir() {
        Debug.Log("SetHitboxDir m_avatarMovement.FacingDir: "+m_avatarMovement.FacingDir);
        Debug.Log("SetHitboxDir m_meleeComboCounter: "+m_meleeComboCounter);
        string objName = string.Empty;
        if(m_avatarMovement.FacingDir.x < 0) {
            // Left
            UpdateMeleeHitboxsEnabled(m_hitBox_left, m_slashEffects_left);
            
        } else if(m_avatarMovement.FacingDir.x > 0) {
            // Right
            UpdateMeleeHitboxsEnabled(m_hitBox_right, m_slashEffects_right);
        } 
    }
    
    private void UpdateMeleeHitboxsEnabled(HitBox_Overlap2D hitbox, List<VisualEffect> m_slashEffects)
    {
        hitbox.gameObject.SetActive(true);
        int index = m_meleeComboCounter - 1 >= 0 ? m_meleeComboCounter - 1 : 0;
        m_slashEffects[index].gameObject.SetActive(true);
    }

    private void UpdateSlashEffect() {

    }
    
}
