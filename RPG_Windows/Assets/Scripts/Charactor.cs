using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Charactor : MonoBehaviour
{
    #region 角色物件
    /// <summary>
    /// 角色動畫控制器
    /// </summary>
    protected Animator m_Animator;
    /// <summary>
    /// 角色物理物件
    /// </summary>
    protected Rigidbody2D m_Rigidbody;
    /// <summary>
    /// 角色中心
    /// </summary>
    public Vector3 m_center;
    #endregion

    #region 角色相關參數

    #region 移動
    /// <summary>
    /// 角色移速
    /// </summary>
    [SerializeField] private float moveSpeed = 5f;
    /// <summary>
    /// 角色移動向量
    /// </summary>
    protected Vector3 movement;
    /// <summary>
    /// 角色面向方向
    /// </summary>
    protected Vector2 facingDir = Vector2.down;
    /// <summary>
    /// 角色目前是否為移動中
    /// </summary>
    public bool isMoving {
        get {
            return movement.x != 0 || movement.y != 0;
        }
    }
    #endregion

    #region 跳躍

    public float height = 0;
    [SerializeField] protected float maxJumpHeight = 1.5f;
    protected float jumpOffset = 0.8f;
    protected float g = -0.5f;
    protected Coroutine jumpRoutine;
    protected bool isJumping;
    protected float jumpClipTime = 0.2f;

    #endregion

    #region 攻擊
    /// <summary>
    /// 角色攻擊動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine attackRoutine;
    /// <summary>
    /// 角色正在攻擊
    /// </summary>
    protected bool isAttacking;
    /// <summary>
    /// 記錄攻擊動作完成後的角色移動向量
    /// </summary>
    protected Vector3 movementAfterAttack;
    /// <summary>
    /// 一次攻擊動畫所需的時間
    /// </summary>
    protected float attackClipTime;
    
    #endregion

    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        SetAnimateAttackClipTime();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleAnimatorLayers();
        SetAnimateMovementPara(movement, facingDir);
        FocusCollidersWithHeight();
    }

    private void FixedUpdate() {
        Debug.Log("FixedUpdate start player height: "+height);
        Debug.Log("FixedUpdate start player jumpOffset: "+jumpOffset);
        if(isAttacking) {
            //Debug.Log("attacking");
            if(isMoving) {
                movementAfterAttack = movement;
                //Debug.Log("movementAfterAttack: "+movementAfterAttack);
            }
            movement = Vector3.zero;
        }
        
        if(isJumping) {
            height = height + jumpOffset;
            if(jumpOffset >= 0) {
                jumpOffset += (g / 2); 
            } else {
                var hm = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;
                float groundCheckHeight = Mathf.Floor(height);
                if(hm.GroundableChecked(m_center, groundCheckHeight)) {
                    Debug.Log("Groundable true");
                    if(height <= groundCheckHeight) {
                        height = groundCheckHeight;
                        StopJump();
                    } else {
                        jumpOffset += g; 
                    }
                } else {
                    Debug.Log("Groundable false");
                    jumpOffset += g; 
                }
            }
        }
        Debug.Log("FixedUpdate end player height: "+height);
        Debug.Log("FixedUpdate end player jumpOffset: "+jumpOffset);
        Move();
    }

    public void Move() {
        m_Rigidbody.velocity = movement.normalized * moveSpeed;
        //Debug.Log("Move velocity: "+m_Rigidbody.velocity);
        // transform.Translate(movement*moveSpeed*Time.deltaTime);
    }

    public void HandleAnimatorLayers() {
        if(isAttacking) {
            ActivateAnimatorLayer("AttackLayer");
        }
        else if(isMoving) {
            ActivateAnimatorLayer("MoveLayer");
        }
        else {
            ActivateAnimatorLayer("IdleLayer");
        }
    }

    public void ActivateAnimatorLayer(string layerName) {
        for(int i = 0 ; i < m_Animator.layerCount; i++) {
            m_Animator.SetLayerWeight(i, 0);
        }

        m_Animator.SetLayerWeight(m_Animator.GetLayerIndex(layerName), 1);
    }

    public void SetAnimateMovementPara(Vector3 movement, Vector2 facingDir) {
        // Debug.Log("movement.x: "+movement.x + "movement.y: "+movement.y);
        // Debug.Log("facingDir.x: "+facingDir.x + "facingDir.y: "+facingDir.y);
        m_Animator.SetFloat("movementX", movement.x);
        m_Animator.SetFloat("movementY", movement.y);
        m_Animator.SetFloat("facingDirX", facingDir.x);
        m_Animator.SetFloat("facingDirY", facingDir.y);
    }

    private void SetAnimateAttackClipTime() {
        RuntimeAnimatorController ac = m_Animator.runtimeAnimatorController;
        for(int i = 0; i < ac.animationClips.Length; i++) {
            if( ac.animationClips[i].name == "Attack_Down")        //If it has the same name as your clip
            {
                attackClipTime = ac.animationClips[i].length;
                break;
            }
        }
    }

    public void StopAttack() {
        if(attackRoutine != null) {
            StopCoroutine(attackRoutine);
        }

        isAttacking = false;
        m_Animator.SetBool("attack", isAttacking);

        movement = movementAfterAttack;
        movementAfterAttack = Vector3.zero;
        //Debug.Log("attack end");
    }

    public void StopJump() {
        if(jumpRoutine != null) {
            StopCoroutine(jumpRoutine);
        }

        isJumping = false;
        jumpOffset = 1f;
        //Debug.Log("attack end");
    }

    private void FocusCollidersWithHeight() {
        var grid = GameObject.FindObjectOfType(typeof(Grid)) as Grid;
        Collider2D[] collider2Ds = grid.GetComponentsInChildren<Collider2D>();
        foreach(var collider2D in collider2Ds) {
            if(collider2D.gameObject.layer != LayerMask.NameToLayer("Trigger")) {
                //Debug.Log("collider2D tag: "+collider2D.tag);
                var block = collider2D.GetComponent<HeightOfObject>() as HeightOfObject;
                if(block != null) {
                    //Debug.Log("collider2D name: "+collider2D.name);
                    if(height > block.GetCorrespondHeight()) {
                        collider2D.enabled = false;
                    } else {
                        collider2D.enabled = true;
                    }
                }

            }
        }
    }


    public float GetMoveSpeed() {
        return moveSpeed;
    }

    public Vector2 GetFacingDir() {
        return facingDir;
    }
}
