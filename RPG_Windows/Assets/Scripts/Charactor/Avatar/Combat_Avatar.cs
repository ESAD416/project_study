using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Avatar : Attack
{
    #region 基本物件

    [Header("可操作角色戰鬥物件")]
    [SerializeField] protected Avatar m_avatar;
    [SerializeField] protected Movement_Avatar m_avatarMovement;
    protected Animator m_avatarAnimator;
    protected AvatarInputActionsControls inputControls;

    #endregion

    #region 基本參數

    [Header("可操作角色戰鬥參數")]
    /// <summary>
    /// 角色是否為正在攻擊中
    /// </summary>
    public bool isAttacking;
    /// <summary>
    /// 一次攻擊動畫所需的時間
    /// </summary>
    [SerializeField] protected float attackClipTime;
    /// <summary>
    /// 角色攻擊動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine attackRoutine;
    /// <summary>
    /// 記錄當下角色攻擊動作時的移動向量並於攻擊動作完成後復原
    /// </summary>
    protected Vector3 movementAfterAttack;

    #endregion

    protected virtual void Awake() {
        m_avatarAnimator = m_avatar.Animator;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    #region 攻擊控制

    protected IEnumerator Attack() {
        Debug.Log("Combat_Avatar attack start");
        isAttacking = true;
        m_avatarAnimator?.SetTrigger("attack");
        m_avatar.SetCurrentBaseState(m_avatar.Attack);
        yield return new WaitForSeconds(attackClipTime);  // hardcasted casted time for debugged
        FinishAttack();
    }

    protected virtual void FinishAttack() {
        Debug.Log("Combat_Avatar FinishAttack start");
        if(attackRoutine != null) {
            StopCoroutine(attackRoutine);
        }

        isAttacking = false;

        m_avatarMovement.SetMovement(movementAfterAttack);
        
        if(m_avatarMovement.isMoving) m_avatar.SetCurrentBaseState(m_avatar.Move);
        else m_avatar.SetCurrentBaseState(m_avatar.Idle);

        movementAfterAttack = Vector3.zero;
        Debug.Log("Combat_Avatar FinishAttack end");
    }

    #endregion
}
