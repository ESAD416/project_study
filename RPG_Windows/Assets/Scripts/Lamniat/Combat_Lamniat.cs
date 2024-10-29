using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Combat_Lamniat : Attack_System
{
    [Header("")]
    [SerializeField] protected Lamniat m_lamniatPlayer;
    protected Movement_Lamniat m_lamniatMovement;    
    protected Animator m_lamniatAnimator;

    [Header("Combat_Avatar 參數")]
    public bool IsAttacking = false;                            // 角色是否為正在攻擊中
    public bool CanAttack = false;                              // 角色是否可以攻擊
    public bool IsPreAttacking = false;                         // 角色是否為正在攻擊前搖中
    public bool IsPostAttacking = false;                        // 角色是否為正在攻擊後搖中
    public bool CancelRecovery = false;                         // 角色是否可以取消硬直(前/後搖)
    [SerializeField] protected float m_attackClipTime;
    

    [Header("Lamniat近戰參數")]
    private bool m_getMeleeInput = true;
    protected int m_maximumMeleeComboCount = 3;
    [SerializeField] protected int m_meleeComboCounter = 0;
    public int MeleeComboCounter => this.m_meleeComboCounter;
    public void SetMeleeComboCounter(int value) => this.m_meleeComboCounter = value;

    [Header("Lamniat遠程參數")]
    public bool IsShooting;
    public bool IsAiming;
    [SerializeField] protected Vector2 m_AimDir = Vector2.zero;
    [SerializeField] protected Vector2 m_ShootDir = Vector2.zero;
    public Vector2 ShootDir => this.m_ShootDir;

    [Header("Lamniat近戰物件")]
    [SerializeField] protected HitBox_Overlap2D m_hitBox_left;
    [SerializeField] protected HitBox_Overlap2D m_hitBox_right;
    private HitBox_Overlap2D enabledMeleeHitbox;

    [Header("Lamniat遠程物件")]
    [SerializeField] protected GameObject m_bulletPrefab;
    

    protected void Awake() {
        m_lamniatMovement = m_lamniatPlayer.LamniatMovement;
        m_lamniatAnimator = m_lamniatPlayer.Animator;

        m_attackClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_lamniatAnimator, "Lamniat_attack_1_chop_clockwise");
        // Debug.Log("Combat_Lamniat Start SetAnimateClipTime attackClipTime: "+attackClipTime);
    }

    protected void Start() {
        #region InputSystem事件設定

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Melee.performed += content => {
            PerformMelee();
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Shoot.performed += content => {
            PerformShoot();
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Shoot_Hold.performed += content => {
            Debug.Log("Lamniat_Land.Shoot_Hold.started");
            //m_isHoldShoot = true;
            
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Shoot_Hold.canceled += content => {
            //Debug.Log("Lamniat_Land.Shoot_Hold.canceled");
            //m_isHoldShoot = false;
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.AimAt_GamePad.performed += content => {
            //Debug.Log("Lamniat_Land.AimAt_GamePad.started");
            var inputVecter2 = content.ReadValue<Vector2>();

            m_AimDir = inputVecter2;
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.AimAt_Mouse.performed += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            //Debug.Log("Lamniat_Land.AimAt_Mouse.inputVecter2: "+inputVecter2);

            m_AimDir = inputVecter2;
        };

        #endregion
    }

    protected override void Update() {
        if(m_lamniatPlayer.SprtRenderer.flipX) enabledMeleeHitbox = m_hitBox_left;
        else enabledMeleeHitbox = m_hitBox_right;

        if(m_meleeComboCounter == 3) m_getMeleeInput = false;
        else m_getMeleeInput = true;

        SetAnimateCombatPara();
        SetShootDir();
        
    }

    protected void SetToAttackState() {
        if(m_lamniatPlayer.StateController.CurrentBaseStateName.Equals(Constant.CharactorState.Move)) {
            m_lamniatMovement.SetFacingDir(m_lamniatMovement.Movement);
            //m_avatar.SetFacingDir(m_avatar.Movement);
        }

        m_lamniatMovement.CanMove = false;
        if(!IsAttacking) IsAttacking = true;
        if(!m_lamniatPlayer.StateController.CurrentBaseStateName.Equals(Constant.CharactorState.Attack)) 
            m_lamniatPlayer.StateController.SetCurrentBaseState(m_lamniatPlayer.StateController.Attack);
    }

    public void SetAnimateCombatPara() 
    {
        m_lamniatAnimator.SetBool("isAttacking", IsAttacking);
        m_lamniatAnimator.SetBool("isPreAttacking", IsPreAttacking);
        m_lamniatAnimator.SetBool("isPostAttacking", IsPostAttacking);
        m_lamniatAnimator.SetBool("cancelRecovery", CancelRecovery);
        m_lamniatAnimator.SetBool("cancelRecovery", CancelRecovery);
        m_lamniatAnimator.SetBool("isFlipX", m_lamniatPlayer.SprtRenderer.flipX);
        // m_avatarAnimator.SetBool("isHoldShoot", m_isHoldShoot);
        // m_avatarAnimator.SetBool("isTakingAim", IsAiming);
    }

    public void PerformMelee() {
        Debug.Log("Lamniat_Land.Melee.started");
        SetToAttackState();

        if(m_getMeleeInput) m_lamniatAnimator.SetTrigger("melee");
    }

    public void PerformShoot() {
        Debug.Log("Lamniat_Land.Shoot.started");
        SetToAttackState();
        
        if(PlayerInputManager.instance.ControlDevice == Constant.ControlDevice.KeyboardMouse) {
            if(m_ShootDir.x < 0) {
                // Left
                m_lamniatPlayer.SprtRenderer.flipX = true;
            } else if(m_ShootDir.x > 0) {
                // Right
                m_lamniatPlayer.SprtRenderer.flipX = false;
            } 
        }

        m_lamniatAnimator.SetTrigger("shoot");
        IsShooting = true;
    }

    public void Melee() {
        //TODO Set Melee para
        Debug.Log("UI Melee()");
    }


    public void FinishMelee() 
    {
        Debug.Log("FinishMeleeAttack start"); 
        IsAttacking = false;
        IsPreAttacking = false;
        IsPostAttacking = false;
        CancelRecovery = false;
        m_lamniatMovement.CanMove = true;

        //m_avatarMovement.SetMovement(m_avatarMovement.MovementAfterTrigger);
        
        if(m_lamniatMovement.IsMoving) m_lamniatPlayer.StateController.SetCurrentBaseState(m_lamniatPlayer.StateController.Move);
        else m_lamniatPlayer.StateController.SetCurrentBaseState(m_lamniatPlayer.StateController.Idle);

        m_hitBox_left.GetComponentsInChildren<Transform>(true).ToList().ForEach(h => h.gameObject.SetActive(false));
        m_hitBox_right.GetComponentsInChildren<Transform>(true).ToList().ForEach(h => h.gameObject.SetActive(false));

        // m_hitBox_left.gameObject.SetActive(false);
        // m_hitBox_right.gameObject.SetActive(false);
        // m_slashEffects_left.ForEach(h => 
        // {
        //     h.gameObject.SetActive(false);
        // });
        // m_slashEffects_right.ForEach(h => 
        // {
        //     h.gameObject.SetActive(false);
        // });

        this.m_meleeComboCounter = 0;


        Debug.Log("FinishMeleeAttack end");
    }

    public void SetMeleeHitboxDir() {
        // Debug.Log("SetHitboxDir m_avatarMovement.FacingDir: "+m_avatarMovement.FacingDir);
        // Debug.Log("SetHitboxDir m_meleeComboCounter: "+m_meleeComboCounter);
        UpdateMeleeHitboxsEnabled();
    }
    
    private void UpdateMeleeHitboxsEnabled()
    {
        //Debug.Log("UpdateMeleeHitboxsEnabled set hitbox name: "+enabledMeleeHitbox.gameObject.name);
        enabledMeleeHitbox.gameObject.SetActive(true);

        var visualEffects = enabledMeleeHitbox.GetComponentsInChildren<VisualEffect>(true);
        foreach(VisualEffect effect in visualEffects) {
            //Debug.Log("effect.gameObject.name: "+effect.gameObject.name);
            if(effect.gameObject.name.Equals("_combo"+m_meleeComboCounter)) {
                effect.gameObject.SetActive(true);
            } else {
                effect.gameObject.SetActive(false);
            }
        }
    }

    public void SetShootDir() {
        switch(PlayerInputManager.instance.ControlDevice) {
            case Constant.ControlDevice.KeyboardMouse:
                Ray ray = Camera.main.ScreenPointToRay(m_AimDir);
                Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);
                float rayDistance;
                if(groundPlane.Raycast(ray, out rayDistance)) {
                    Vector3 point = ray.GetPoint(rayDistance);
                    Debug.DrawLine(m_lamniatPlayer.Center, point, Color.blue);
                    
                    m_ShootDir = (point - m_lamniatPlayer.Center).normalized;
                    //Debug.Log("Shoot m_ShootDir "+m_ShootDir);
                }
                break;
            default:
                float joystickDeadZone = 0.1f;
                //float gamePadRotatrSmoothing = 1000f;
                if(Mathf.Abs(m_AimDir.x) > joystickDeadZone || Mathf.Abs(m_AimDir.y) > joystickDeadZone) {
                    Vector3 aimDir = Vector3.right * m_AimDir.x + Vector3.up * m_AimDir.y;
                    //Color color = m_raycastHit.collider != null ? Color.red : Color.green;
                    Debug.DrawLine(m_lamniatPlayer.Center, m_lamniatPlayer.Center + aimDir, Color.red);
                    m_ShootDir = aimDir.normalized;
                }
                break;

        }
    }

    public void Shoot() {
        IsShooting =  true;
        Debug.Log("Shoot m_ShootDir "+m_ShootDir);
        GameObject bullet = Instantiate(m_bulletPrefab, m_lamniatPlayer.Center, transform.rotation);
        bullet.GetComponent<Projectile_Bullet>().SetDirection(m_ShootDir);
        bullet.GetComponent<HitBox_Overlap2D>().SetAttacker(this);
        bullet.GetComponent<HitBox_Overlap2D>().DetectTagName = "Enemy";

        var angle = Vector3.Angle(m_ShootDir, bullet.GetComponent<Projectile_Bullet>().referenceAxis);
        bullet.GetComponent<HitBox_Overlap2D>().Angle = m_ShootDir.x > 0 ? -angle : angle;

        var quaternion = m_ShootDir.x > 0 ? Quaternion.Euler(0, 0, -angle) : Quaternion.Euler(0, 0, angle);
        bullet.transform.rotation = quaternion;
    }

    public void FinishShoot() {
        Debug.Log("FinishShoot start"); 
        IsAttacking = false;
        IsShooting =false;
        m_lamniatMovement.CanMove = true;

        //m_avatarMovement.SetMovement(m_avatarMovement.MovementAfterTrigger);
        
        if(m_lamniatMovement.IsMoving) m_lamniatPlayer.StateController.SetCurrentBaseState(m_lamniatPlayer.StateController.Move);
        else m_lamniatPlayer.StateController.SetCurrentBaseState(m_lamniatPlayer.StateController.Idle);

        Debug.Log("FinishShoot end");
    }

}
