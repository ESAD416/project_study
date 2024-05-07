using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public bool IsAiming;
    [SerializeField] protected Vector2 m_AimDir = Vector2.zero;
    [SerializeField] protected Vector2 m_ShootDir = Vector2.zero;

    [Header("Lamniat近戰物件")]
    [SerializeField] protected HitBox_Overlap2D m_hitBox_left;
    [SerializeField] protected HitBox_Overlap2D m_hitBox_right;
    [SerializeField] protected List<VisualEffect> m_slashEffects_left;
    [SerializeField] protected List<VisualEffect> m_slashEffects_right;

    [Header("Lamniat遠程物件")]
    [SerializeField] protected Transform m_firePoint_left;
    [SerializeField] protected Transform m_firePoint_right;
    private Transform enabledFirePoint;
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
            
            if(controlDevice == Constant.ControlDevice.KeyboardMouse) {
                if(m_ShootDir.x < 0) {
                    // Left
                    m_avatar.SprtRenderer.flipX = true;
                } else if(m_ShootDir.x > 0) {
                    // Right
                    m_avatar.SprtRenderer.flipX = false;
                } 
            }

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

        m_inputControls.Lamniat_Land.AimAt_GamePad.performed += content => {
            Debug.Log("Lamniat_Land.AimAt_GamePad.started");
            var inputVecter2 = content.ReadValue<Vector2>();

            m_AimDir = inputVecter2;
        };

        m_inputControls.Lamniat_Land.AimAt_Mouse.performed += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            Debug.Log("Lamniat_Land.AimAt_Mouse.inputVecter2: "+inputVecter2);

            m_AimDir = inputVecter2;
        };

        #endregion
    }

    protected override void Update() {
        base.Update();

        SetAnimateCombatPara();
        SetShootDir();

        switch(controlDevice) {
            case Constant.ControlDevice.KeyboardMouse:
                if(m_ShootDir.x < 0) {
                    // Left
                    enabledFirePoint = m_firePoint_left;
                } else if(m_ShootDir.x > 0) {
                    // Right
                    enabledFirePoint = m_firePoint_right;
                } 
                break;
            default:
                if(m_avatarMovement.FacingDir.x < 0) {
                    // Left
                    enabledFirePoint = m_firePoint_left;
                    
                } else if(m_avatarMovement.FacingDir.x > 0) {
                    // Right
                    enabledFirePoint = m_firePoint_right;
                } 
                break;

        }
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

    public void OnDeviceChange(PlayerInput input) {
        Debug.Log("OnDeviceChange: "+input.currentControlScheme);
        switch(input.currentControlScheme) {
            case "Keyboard & Mouse":
                controlDevice = Constant.ControlDevice.KeyboardMouse;
                break;
            // case "Mobile":
            //     controlDevice = Constant.ControlDevice.Mobile;
            //     break;
            default:
                controlDevice = Constant.ControlDevice.Gamepad;
                break;
        }
    }

    public void Melee() {
        //TODO Set Melee para
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

    public void SetShootDir() {
        switch(controlDevice) {
            case Constant.ControlDevice.KeyboardMouse:
                Ray ray = Camera.main.ScreenPointToRay(m_AimDir);
                Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);
                float rayDistance;
                if(groundPlane.Raycast(ray, out rayDistance)) {
                    Vector3 point = ray.GetPoint(rayDistance);
                    Debug.DrawLine(m_avatar.Center, point, Color.blue);
                    
                    m_ShootDir = point.normalized;
                    Debug.Log("Shoot m_ShootDir "+m_ShootDir);
                }
                break;
            default:
                float joystickDeadZone = 0.1f;
                //float gamePadRotatrSmoothing = 1000f;
                if(Mathf.Abs(m_AimDir.x) > joystickDeadZone || Mathf.Abs(m_AimDir.y) > joystickDeadZone) {
                    Vector3 aimDir = Vector3.right * m_AimDir.x + Vector3.up * m_AimDir.y;
                    //Color color = m_raycastHit.collider != null ? Color.red : Color.green;
                    Debug.DrawLine(m_avatar.Center, m_avatar.Center + aimDir, Color.red);
                    m_ShootDir = aimDir.normalized;
                }
                break;

        }
    }


    public void Shoot() {
        IsShooting =  true;
        Debug.Log("Shoot m_ShootDir "+m_ShootDir);
        GameObject bullet = Instantiate(m_bulletPrefab, enabledFirePoint.position, enabledFirePoint.rotation);
        bullet.GetComponent<Projectile_Bullet>().SetDirection(m_ShootDir);
        var angle = Vector3.Angle(m_ShootDir, bullet.GetComponent<Projectile_Bullet>().referenceAxis);
        var quaternion = m_ShootDir.x > 0 ? Quaternion.Euler(0, 0, -angle) : Quaternion.Euler(0, 0, angle);
        bullet.transform.rotation = quaternion;


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

}
