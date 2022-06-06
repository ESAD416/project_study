using System;
using System.Collections;
using System.Linq;
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
    public Vector3 m_Center;
    /// <summary>
    /// 角色橫縱高座標
    /// </summary>
    public Vector3 m_Coordinate;

    #endregion

    #region 移動參數
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

    #region 跳躍參數

    public float currHeight = 0f;
    private float lastHeight = 0f;
    [SerializeField] protected float maxJumpHeight = 1.5f;
    protected float jumpOffset = 0.35f;
    protected float g = -0.09f;
    protected Coroutine jumpRoutine;
    protected bool isJumping;
    protected float jumpClipTime = 0.2f;

    #endregion

    #region 攻擊參數
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


    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Coordinate = Vector3.zero;
        SetAnimateAttackClipTime();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateCoordinate();
        Debug.Log("coordinate: "+m_Coordinate);
        HandleAnimatorLayers();
        SetAnimateMovementPara(movement, facingDir);
        FocusCollidersWithHeight();
    }

    private void FixedUpdate() {
        // Debug.Log("FixedUpdate start player height: "+height);
        // Debug.Log("FixedUpdate start player jumpOffset: "+jumpOffset);
        transform.position = GetWorldPosByCoordinate(m_Coordinate) + new Vector3(0, 0.5f);   // 預設中心點是(x, y-0.5)
        if(isAttacking) {
            //Debug.Log("attacking");
            if(isMoving) {
                movementAfterAttack = movement;
                //Debug.Log("movementAfterAttack: "+movementAfterAttack);
            }
            movement = Vector3.zero;
        }
        else if(isJumping) {
            //FocusCollidersWithHeight();
            HandleJumpingProcess();
        }
        // Debug.Log("FixedUpdate end player height: "+height);
        // Debug.Log("FixedUpdate end player jumpOffset: "+jumpOffset);
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
        jumpOffset = 0.35f;
        lastHeight = currHeight;
        //Debug.Log("attack end");
    }

    public void UpdateCoordinate() {
        Vector3 result = Vector3.zero;
        result.x = m_Center.x;
        result.z = currHeight;
        result.y = m_Center.y - lastHeight;

        m_Coordinate = result;
    }

    public Vector3 GetShadowCoordinate(float groundLevel = 0) {
        return new Vector3(m_Coordinate.x, m_Coordinate.y, groundLevel);
    }

    public Vector3 GetWorldPosByCoordinate(Vector3 coordinate) {
        Vector3 result = Vector3.zero;
        result.x = coordinate.x;
        result.y = coordinate.y + coordinate.z;

        return result;
    }

    private void HandleJumpingProcess() {
        Debug.Log("currHeight start: "+currHeight);
        // Debug.Log("lastHeight start: "+lastHeight);
        // Debug.Log("jumpOffset start: "+jumpOffset);
        float goalheight = currHeight + jumpOffset;
        Debug.Log("goalheight: "+goalheight);
        if(jumpOffset >= 0) {
            lastHeight = currHeight;
            currHeight = goalheight;
            jumpOffset += (g / 2); 
        } else {
            var hm = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;
            float groundCheckHeight = Mathf.Floor(currHeight);
            List<float> levelsHeight = hm.defaultTileDatas.Select(h => h.height).ToList();   // 取現有Level的高，由高至低排序
            bool notGroundable = true;

            while(levelsHeight.Contains(groundCheckHeight)) {
                Vector3 shadowCoordinate = new Vector3(m_Coordinate.x, m_Coordinate.y, groundCheckHeight);
                Vector3 worldPos = new Vector3(shadowCoordinate.x, shadowCoordinate.y + shadowCoordinate.z);
                Debug.Log("groundCheck shadowCoordinate: "+shadowCoordinate);
                Debug.Log("groundCheck worldPos: "+worldPos);

                if(hm.GroundableChecked(worldPos, groundCheckHeight)) {
                // if(hm.GroundableChecked(m_Coordinate)) {
                    Debug.Log("Groundable true");
                    Debug.Log("goalheight: "+goalheight);
                    Debug.Log("groundCheckHeight: "+groundCheckHeight);
                    if(goalheight <= groundCheckHeight) {
                        lastHeight = currHeight;
                        currHeight = groundCheckHeight;
                        StopJump();
                    } else {
                        lastHeight = currHeight;
                        currHeight = goalheight;
                        jumpOffset += g;
                    }
                    notGroundable = false;
                    break;
                }

                groundCheckHeight--;
            }

            if(notGroundable) {
                Debug.Log("Groundable false");
                lastHeight = currHeight;
                currHeight = goalheight;
                jumpOffset += g; 
            }

            // if(hm.GroundableChecked(m_Center, groundCheckHeight)) {
            // if(hm.GroundableChecked(m_Coordinate)) {
            //     Debug.Log("Groundable true");
            //     if(goalheight <= groundCheckHeight) {
            //         lastHeight = currHeight;
            //         currHeight = groundCheckHeight;
            //         StopJump();
            //     } else {
            //         lastHeight = currHeight;
            //         currHeight = goalheight;
            //         jumpOffset += g;
            //     }
            // } else {
            //     Debug.Log("Groundable false");
            //     lastHeight = currHeight;
            //     currHeight = goalheight;
            //     jumpOffset += g; 
            // }

        }
        Debug.Log("currHeight end: "+currHeight);
        // Debug.Log("lastHeight end: "+lastHeight);
        // Debug.Log("jumpOffset end: "+jumpOffset);
    }

    private void FocusCollidersWithHeight() {
        var grid = GameObject.FindObjectOfType(typeof(Grid)) as Grid;
        Collider2D[] collider2Ds = grid.GetComponentsInChildren<Collider2D>();
        foreach(var collider2D in collider2Ds) {
            if(collider2D.gameObject.layer != LayerMask.NameToLayer("Trigger")) {
                //Debug.Log("collider2D tag: "+collider2D.tag);
                var block = collider2D.GetComponent<HeightOfObject>() as HeightOfObject;
                if(block != null) {
                    // Debug.Log("FocusCollidersWithHeight collider2D name: "+collider2D.name);
                    // Debug.Log("FocusCollidersWithHeight collider2D type: "+collider2D.GetType());
                    if(currHeight >= block.GetCorrespondHeight()) {
                        collider2D.enabled = false;
                    } else {
                        collider2D.enabled = true;
                    }
                    // Debug.Log("FocusCollidersWithHeight collider2D enabled: "+collider2D.enabled);
                }

            }
        }
    }
}
