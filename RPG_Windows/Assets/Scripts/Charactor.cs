using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static JumpMechanismUtils;

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
    [Header("Basic Objects")]
    public Vector3 m_Center;
    /// <summary>
    /// 角色底部
    /// </summary>
    public Vector3 m_Buttom;
    /// <summary>
    /// 角色橫縱高座標
    /// </summary>
    public Vector3 m_Coordinate;
    /// <summary>
    /// 角色中心物件名稱
    /// </summary>
    public string centerObjName;
    /// <summary>
    /// 角色底部物件名稱
    /// </summary>
    public string buttomObjName;
    /// <summary>
    /// 角色相關資訊存取
    /// </summary>
    public PlayerStorage infoStorage;

    #endregion

    #region 移動參數
    [Header("Movement Parameters")]
    /// <summary>
    /// 角色移速
    /// </summary>
    [SerializeField] protected float moveSpeed = 5f;
    public float MoveSpeed => moveSpeed;
    public void SetMoveSpeed(float speed) {
        this.moveSpeed = speed;
    }
    /// <summary>
    /// 角色移動向量
    /// </summary>
    protected Vector3 movement = Vector3.zero;
    public Vector3 Movement => movement;
    public void SetMovement(Vector3 vector3) {
        this.movement = vector3;
    }
    /// <summary>
    /// 角色面向方向
    /// </summary>
    protected Vector2 facingDir = Vector2.down;
    public Vector3 FacingDir => facingDir;
    /// <summary>
    /// 角色目前是否為移動中
    /// </summary>
    public bool isMoving {
        get {
            return movement.x != 0 || movement.y != 0;
        }
    }
    /// <summary>
    /// 角色目前是否能移動
    /// </summary>
    public bool cantMove {
        get {
            bool jumpingUpButNotFinish = isJumping && jumpState == JumpState.JumpUp && jumpIncrement < 1f;
            return jumpingUpButNotFinish || jumpHitColli || (isTakingHit && !hyperArmor);
        }
    }
    #endregion

    #region 跳躍參數
    [Header("Jumping Parameters")]
    public float currHeight = 0f;
    public float CurrentHeight => currHeight;
    public void SetCurrentHeight(float height) {
        this.currHeight = height;
    }
    protected Vector3 takeOffCoord = Vector3.zero;
    protected Vector2 takeOffDir = Vector2.zero;
    protected float lastHeight = 0f;
    protected float minjumpOffSet = -0.3f;
    protected float jumpOffset = 0.3f;
    protected float maxJumpHeight = 1.5f;
    protected float jumpIncrement = 0f;
    protected float g = -0.0565f;
    protected bool isJumping;
    protected float jumpingMovementVariable = 0.5f;
    protected JumpState jumpState;
    protected bool jumpHitColli;
    protected Coroutine jumpDelayRoutine;
    protected float jumpDelay = 0.1f;
    protected bool jumpDelaying = false;
    protected Coroutine groundDelayRoutine;
    protected float groundDelay = 0.2f;
    protected bool groundDelaying = false;
    
    #endregion

    #region 攻擊參數
    [Header("Attack Parameters")]
    /// <summary>
    /// 角色攻擊動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine attackRoutine;
    /// <summary>
    /// 角色正在攻擊
    /// </summary>
    public bool isAttacking;
    /// <summary>
    /// 記錄攻擊動作完成後的角色移動向量
    /// </summary>
    protected Vector3 movementAfterAttack;
    /// <summary>
    /// 一次攻擊動畫所需的時間
    /// </summary>
    protected float attackClipTime;
    
    #endregion

    #region 血量系統參數

    private HealthSystemModel healthSystem;
    // Getter
    public HealthSystemModel HealthSystem => healthSystem;
    /// <summary>
    /// 血量
    /// </summary>
    // private int currHealth = 20;
    /// <summary>
    /// 血量
    /// </summary>
    private int maxHealth = 20;
    /// <summary>
    /// 已死亡
    /// </summary>
    public bool isDead = false;

    #endregion

    #region 受擊參數
    [Header("TakingHit Parameters")]
    /// <summary>
    /// 正在受擊
    /// </summary>
    public bool isTakingHit = false;
    /// <summary>
    /// 受擊動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine takeHitRoutine = null;
    /// <summary>
    /// 一次受擊動畫所需的時間
    /// </summary>
    protected float hitRecoveryTime;

    public int knockOffThrust ;

    public bool hyperArmor;

    #endregion

    #region 眩暈參數
    [Header("Stunned Parameters")]
    public bool stunnable;

    protected bool isStunned = false;

    protected int armorToStunned = 3;

    protected float stunRecoveryTime = 1.5f;

    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Coordinate = Vector3.zero;
        healthSystem = new HealthSystemModel(maxHealth);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Debug.Log("takeHitRoutine == null: "+(takeHitRoutine == null));

        var center = transform.Find(centerObjName).GetComponent<Transform>();
        m_Center = new Vector3(center.position.x, center.position.y);

        var buttom = transform.Find(buttomObjName).GetComponent<Transform>();
        m_Buttom = new Vector3(buttom.position.x, buttom.position.y);

        UpdateCoordinate();
        //Debug.Log("coordinate: "+m_Coordinate);
        HandleAnimatorLayers();
        SetAnimateMovementPara(movement, facingDir);
        if(!string.IsNullOrEmpty(infoStorage.jumpCollidersName)) {
            if(isJumping) {
            FocusCollidersWithHeightWhileJumping();
            } else {
                FocusCollidersWithHeight();
            }
        }
    }

    protected virtual void FixedUpdate() {
        // Debug.Log("FixedUpdate start player height: "+height);
        // Debug.Log("FixedUpdate start player jumpOffset: "+jumpOffset);
        if(isAttacking) {
            //Debug.Log("attacking");
            if(isMoving) {
                movementAfterAttack = movement;
                //Debug.Log("movementAfterAttack: "+movementAfterAttack);
            }
            movement = Vector3.zero;
        }
        // else if(isJumping) {
        //     transform.position = GetWorldPosByCoordinate(m_Coordinate) - new Vector3(0, 1.7f);   // 預設中心點是(x, y+1.7)
        //     HandleJumpingProcess();
        // }
        // Debug.Log("FixedUpdate end player height: "+height);
        // Debug.Log("FixedUpdate end player jumpOffset: "+jumpOffset);

        //Debug.Log("cantMove: "+cantMove);
        if(!cantMove) {
            if(isJumping && jumpState == JumpState.JumpUp) {
                MoveWhileJump();
            } else {
                Move();
            }
        }
    }

    #region 位移控制
    public void Move() {
        m_Rigidbody.velocity = movement.normalized * moveSpeed;
        //m_Rigidbody.AddForce(movement.normalized* moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        // transform.Translate(movement*moveSpeed*Time.deltaTime);
    }

    public void MoveWhileJump() {
        m_Rigidbody.velocity = movement.normalized * jumpingMovementVariable * moveSpeed;
    }

    public void UpdateCoordinate() {
        Vector3 result = Vector3.zero;
        result.x = m_Center.x;
        result.z = currHeight;
        result.y = m_Center.y - lastHeight;

        m_Coordinate = result;
    }

    public Vector3 GetWorldPosByCoordinate(Vector3 coordinate) {
        //Debug.Log("GetWorldPosByCoordinate coordinate" + coordinate);
        Vector3 result = Vector3.zero;
        result.x = coordinate.x;
        result.y = coordinate.y + coordinate.z;
        result.z = coordinate.z;

        //Debug.Log("GetWorldPosByCoordinate result" + result);
        return result;
    }

    #endregion

    #region 動畫控制
    public void HandleAnimatorLayers() {
        if(isAttacking) {
            AnimeUtils.ActivateAnimatorLayer(m_Animator, "AttackLayer");
        }
        else if(isMoving) {
            AnimeUtils.ActivateAnimatorLayer(m_Animator, "MoveLayer");
        }
        else {
            AnimeUtils.ActivateAnimatorLayer(m_Animator, "IdleLayer");
        }
    }

    public void SetAnimateMovementPara(Vector3 movement, Vector2 facingDir) {
        // Debug.Log("movement.x: "+movement.x + "movement.y: "+movement.y);
        // Debug.Log("facingDir.x: "+facingDir.x + "facingDir.y: "+facingDir.y);
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add("movementX", movement.x);
        dict.Add("movementY", movement.y);
        dict.Add("facingDirX", facingDir.x);
        dict.Add("facingDirY", facingDir.y);

        AnimeUtils.SetAnimateFloatPara(m_Animator, dict);
    }

    #endregion

    #region 攻擊控制
    protected IEnumerator Attack() {
        //Debug.Log("attack start");
        isAttacking = true;
        m_Animator.SetBool("attack", isAttacking);
        yield return new WaitForSeconds(attackClipTime);  // hardcasted casted time for debugged
        FinishAttack();
    }

    public virtual void FinishAttack() {
        Debug.Log("FinishAttack start");
        if(attackRoutine != null) {
            StopCoroutine(attackRoutine);
        }

        isAttacking = false;
        m_Animator.SetBool("attack", isAttacking);

        movement = movementAfterAttack;
        movementAfterAttack = Vector3.zero;
        Debug.Log("FinishAttack end");
    }
    #endregion
    
    #region 跳躍控制
    /*
    private void HandleJumpingProcess() {
        Debug.Log("currHeight start: "+currHeight);
        Debug.Log("lastHeight start: "+lastHeight);
        Debug.Log("jumpOffset start: "+jumpOffset);
        float goalheight = currHeight + jumpOffset;
        Debug.Log("goalheight: "+goalheight);

        // if(facingDir == takeOffDir) {
        //     moveSpeed = 5f;
        // } else {
        //     moveSpeed = 14f;
        // }

        if(jumpOffset >= 0) {
            lastHeight = currHeight;
            currHeight = goalheight;
            jumpOffset += (g / 2); 
        } 
        else {
            var hm = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;

            float groundCheckHeight = Mathf.Floor(currHeight);

            Debug.Log("Center: "+m_Center);
            Debug.Log("Coordinate: "+m_Coordinate);
            Debug.Log("transform_pos: "+transform.position);
            Vector3 shadowCoordinate = new Vector3(m_Coordinate.x, m_Coordinate.y, groundCheckHeight);
            Debug.Log("shadowCoordinate: "+shadowCoordinate);
            Vector3 shadowWorldPos = new Vector3(shadowCoordinate.x, shadowCoordinate.y + shadowCoordinate.z);

            if(hm.GroundableChecked(shadowWorldPos, groundCheckHeight)) {
            // if(hm.GroundableChecked(m_Coordinate)) {
                Debug.Log("Groundable true");
                if(goalheight <= groundCheckHeight) {
                    if(hm.NotGroundableChecked(m_Center, groundCheckHeight) || hm.NotGroundableChecked(shadowWorldPos, groundCheckHeight)) {
                        // NotGroundable(ex: 岩壁)判定
                        Debug.Log("NotGroundable true");
                        bool hasCeiling = hm.CeilingChecked(m_Center, groundCheckHeight); 
                        if(hasCeiling) {
                            lastHeight = currHeight;
                            currHeight = goalheight;
                            // currHeight = groundCheckHeight - 0.01f;
                            jumpOffset += g;
                        }
                        
                    } else {
                        Debug.Log("NotGroundable false");
                        lastHeight = currHeight;
                        currHeight = groundCheckHeight;
                        FinishJump();
                        groundDelayRoutine = StartCoroutine(GroundDelay());
                    }                    
                } else {
                    lastHeight = currHeight;
                    currHeight = goalheight;
                    jumpOffset += g;
                }
            } else {
                Debug.Log("Groundable false");
                lastHeight = currHeight;
                currHeight = goalheight;
                jumpOffset += g; 
            }
        }

        if(jumpOffset <= minjumpOffSet) {
            jumpOffset = minjumpOffSet;
        }
        
        Debug.Log("currHeight end: "+currHeight);
        Debug.Log("lastHeight end: "+lastHeight);
        Debug.Log("jumpOffset end: "+jumpOffset);
    }

    protected IEnumerator GroundDelay() {
        groundDelaying = true;
        yield return new WaitForSeconds(groundDelay);  // hardcasted casted time for debugged
        //FinishJump();
        groundDelaying = false;
    }

    protected IEnumerator JumpDelay() {
        jumpDelaying = true;
        yield return new WaitForSeconds(jumpDelay);  // hardcasted casted time for debugged
        jumpDelaying = false;
        
        if(!isJumping) {
            takeOffCoord = m_Coordinate;
            takeOffDir = facingDir;
            isJumping = true;
            Debug.Log("takeOffPos: "+takeOffCoord);
        }
    }

    public void FinishJump() {
        if(groundDelayRoutine != null) {
            StopCoroutine(groundDelayRoutine);
        }

        Debug.Log("StopJump currHeight: "+currHeight);
        Debug.Log("StopJump takeOffPos: "+takeOffCoord);

        isJumping = false;
        jumpHitColli = false;
        groundDelaying = false;
        jumpOffset = 0.3f;
        lastHeight = currHeight;
        moveSpeed = 14f;

        transform.position = new Vector3(transform.position.x, transform.position.y, currHeight);
        takeOffCoord = Vector3.zero;
    }
    */
    #endregion

    #region 受擊控制
    protected IEnumerator TakeHit() {
        Debug.Log("TakeHit");
        isTakingHit = true;
        if(!hyperArmor)
            moveSpeed = 0f;
        
        yield return new WaitForSeconds(hitRecoveryTime);  // hardcasted casted time for debugged
        FinishTakeHit();
    }

    public void FinishTakeHit() {
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        isTakingHit = false;
        moveSpeed = 3f;
        Debug.Log("FinishTakeHit");
    }

    public void TakeHitProcess(Vector3 senderPos) {
        if(stunnable && !isStunned) {
            armorToStunned--;
            if(armorToStunned <= 0 ) isStunned = true;
        }
        
        Debug.Log("TakeDamage isStunned: "+isStunned);
        if(isStunned) {
            //TODO set stunned animation
            takeHitRoutine = StartCoroutine(BeingStunned());
        } else {
            m_Animator.SetTrigger("hurt");
            if(!hyperArmor) {
                KnockbackFeedback feedback = GetComponent<KnockbackFeedback>();
                feedback.ActiveFeedback(senderPos);
            } 
            takeHitRoutine = StartCoroutine(TakeHit());
        }

        if(healthSystem.CurrHealth <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log("Enemy Die");
        isDead = true;
        m_Animator.SetBool("isDead", isDead); 
        GetComponent<Collider2D>().enabled = false;
    }

    
    protected IEnumerator BeingStunned() {
        Debug.Log("BeingStunned");
        moveSpeed = 0f;
        yield return new WaitForSeconds(stunRecoveryTime);  // hardcasted casted time for debugged
        FinishBeingStunned();
    }

    public void FinishBeingStunned() {
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        isStunned = false;
        armorToStunned = 3;
        moveSpeed = 3f;
        Debug.Log("FinishBeingStunned");
    }
    #endregion

    #region 碰撞控制
    private void FocusCollidersWithHeight() {
        Collider2D[] jumpColls = GridUtils.GetColliders(infoStorage.jumpCollidersName);

        if(jumpColls != null) {
            foreach(var collider2D in jumpColls) {
                var heightObj = collider2D.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    // Debug.Log("FocusCollidersWithHeight collider2D name: "+collider2D.name);
                    // Debug.Log("FocusCollidersWithHeight collider2D type: "+collider2D.GetType());
                    if(currHeight == heightObj.GetSelfHeight()) {
                        collider2D.enabled = true;
                    } else {
                        collider2D.enabled = false;
                    }
                    // Debug.Log("FocusCollidersWithHeight collider2D enabled: "+collider2D.enabled);
                }
            }
        }
    }

    private void FocusCollidersWithHeightWhileJumping() {
        Collider2D[] jumpColls = GridUtils.GetColliders(infoStorage.jumpCollidersName);

        if(jumpColls != null) {
            foreach(var collider2D in jumpColls) {
                var heightObj = collider2D.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    float groundCheckHeight = Mathf.Floor(currHeight);
                    float maxGroundCheckHeight = groundCheckHeight + 1;
                    // Debug.Log("FocusCollidersWithHeight collider2D name: "+collider2D.name);
                    // Debug.Log("FocusCollidersWithHeight collider2D type: "+collider2D.GetType());
                    if(groundCheckHeight + 1 == heightObj.GetSelfHeight()) {
                        collider2D.enabled = true;
                    } else {
                        collider2D.enabled = false;
                    }
                    // Debug.Log("FocusCollidersWithHeight collider2D enabled: "+collider2D.enabled);
                }
            }
        }
    }
    #endregion
}
