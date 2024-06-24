using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Combat_Lamniat : Combat_Player<BoxCollider2D>
{
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
    

    protected override void Awake() {
        base.Awake();
        m_attackClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_playerAnimator, "Lamniat_attack_1_chop_clockwise");
        // Debug.Log("Combat_Lamniat Start SetAnimateClipTime attackClipTime: "+attackClipTime);
    }

    protected override void Start() {
        #region InputSystem事件設定

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Melee.performed += content => {
            Debug.Log("Lamniat_Land.Melee.started");
            SetToAttackState();

            if(m_getMeleeInput) m_playerAnimator.SetTrigger("melee");
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Shoot.performed += content => {
            Debug.Log("Lamniat_Land.Shoot.started");
            SetToAttackState();
            
            if(PlayerInputManager.instance.ControlDevice == Constant.ControlDevice.KeyboardMouse) {
                if(m_ShootDir.x < 0) {
                    // Left
                    m_player.SprtRenderer.flipX = true;
                } else if(m_ShootDir.x > 0) {
                    // Right
                    m_player.SprtRenderer.flipX = false;
                } 
            }

            m_playerAnimator.SetTrigger("shoot");
            IsShooting = true;
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
        base.Update();
        if(m_player.SprtRenderer.flipX) enabledMeleeHitbox = m_hitBox_left;
        else enabledMeleeHitbox = m_hitBox_right;

        if(m_meleeComboCounter == 3) m_getMeleeInput = false;
        else m_getMeleeInput = true;

        SetAnimateCombatPara();
        SetShootDir();
        
    }

    protected void SetToAttackState() {
        if(m_player.CurrentBaseState.State.Equals(Constant.CharactorState.Move)) {
            m_playerMovement.SetFacingDir(m_playerMovement.Movement);
            //m_avatar.SetFacingDir(m_avatar.Movement);
        }

        m_playerMovement.CanMove = false;
        if(!IsAttacking) IsAttacking = true;
        if(!m_player.CurrentBaseState.State.Equals(Constant.CharactorState.Attack)) m_player.SetCurrentBaseState(m_player.Attack);
    }

    public void SetAnimateCombatPara() 
    {
        m_playerAnimator.SetBool("isAttacking", IsAttacking);
        m_playerAnimator.SetBool("isPreAttacking", IsPreAttacking);
        m_playerAnimator.SetBool("isPostAttacking", IsPostAttacking);
        m_playerAnimator.SetBool("cancelRecovery", CancelRecovery);
        m_playerAnimator.SetBool("cancelRecovery", CancelRecovery);
        m_playerAnimator.SetBool("isFlipX", m_player.SprtRenderer.flipX);
        // m_avatarAnimator.SetBool("isHoldShoot", m_isHoldShoot);
        // m_avatarAnimator.SetBool("isTakingAim", IsAiming);
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
        m_playerMovement.CanMove = true;

        //m_avatarMovement.SetMovement(m_avatarMovement.MovementAfterTrigger);
        
        if(m_playerMovement.IsMoving) m_player.SetCurrentBaseState(m_player.Move);
        else m_player.SetCurrentBaseState(m_player.Idle);

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
                    Debug.DrawLine(m_player.Center, point, Color.blue);
                    
                    m_ShootDir = (point - m_player.Center).normalized;
                    //Debug.Log("Shoot m_ShootDir "+m_ShootDir);
                }
                break;
            default:
                float joystickDeadZone = 0.1f;
                //float gamePadRotatrSmoothing = 1000f;
                if(Mathf.Abs(m_AimDir.x) > joystickDeadZone || Mathf.Abs(m_AimDir.y) > joystickDeadZone) {
                    Vector3 aimDir = Vector3.right * m_AimDir.x + Vector3.up * m_AimDir.y;
                    //Color color = m_raycastHit.collider != null ? Color.red : Color.green;
                    Debug.DrawLine(m_player.Center, m_player.Center + aimDir, Color.red);
                    m_ShootDir = aimDir.normalized;
                }
                break;

        }
    }


    public void Shoot() {
        IsShooting =  true;
        Debug.Log("Shoot m_ShootDir "+m_ShootDir);
        GameObject bullet = Instantiate(m_bulletPrefab, m_player.Center, transform.rotation);
        bullet.GetComponent<Projectile_Bullet>().SetDirection(m_ShootDir);
        bullet.GetComponent<HitBox_Overlap2D>().SetAttacker(this);
        bullet.GetComponent<HitBox_Overlap2D>().DetectTagName = "Enemies";

        var angle = Vector3.Angle(m_ShootDir, bullet.GetComponent<Projectile_Bullet>().referenceAxis);
        bullet.GetComponent<HitBox_Overlap2D>().Angle = m_ShootDir.x > 0 ? -angle : angle;

        var quaternion = m_ShootDir.x > 0 ? Quaternion.Euler(0, 0, -angle) : Quaternion.Euler(0, 0, angle);
        bullet.transform.rotation = quaternion;
    }

    public void FinishShoot() {
        Debug.Log("FinishShoot start"); 
        IsAttacking = false;
        IsShooting =false;
        m_playerMovement.CanMove = true;

        //m_avatarMovement.SetMovement(m_avatarMovement.MovementAfterTrigger);
        
        if(m_playerMovement.IsMoving) m_player.SetCurrentBaseState(m_player.Move);
        else m_player.SetCurrentBaseState(m_player.Idle);

        Debug.Log("FinishShoot end");
    }

}
