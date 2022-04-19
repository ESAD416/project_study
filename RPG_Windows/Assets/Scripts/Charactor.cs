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
    private Rigidbody2D m_Rigidbody;
    #endregion

    #region 角色動作與動畫參數

    #region 移動動作
    /// <summary>
    /// 角色移速
    /// </summary>
    [SerializeField] private float moveSpeed = 2f;
    /// <summary>
    /// 角色移動向量
    /// </summary>
    protected Vector3 movement;
    /// <summary>
    /// 角色面向方向
    /// </summary>
    protected Vector2 facingDir;
    /// <summary>
    /// 角色目前是否為移動中
    /// </summary>
    public bool isMoving {
        get {
            return movement.x != 0 || movement.y != 0;
        }
    }
    #endregion

    #region 攻擊動作
    /// <summary>
    /// 角色正在攻擊
    /// </summary>
    protected bool isAttacking;

    /// <summary>
    /// 角色攻擊動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine attackRoutine;
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
    }

    private void FixedUpdate() {
        if(isAttacking) {
            //Debug.Log("attacking");
            if(isMoving) {
                movementAfterAttack = movement;
                //Debug.Log("movementAfterAttack: "+movementAfterAttack);
            }
            movement = Vector3.zero;
        } 
        
        Move();
    }

    public void Move() {
        m_Rigidbody.velocity = movement.normalized * moveSpeed;
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

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    public Vector2 GetFacingDir() {
        return facingDir;
    }
}
