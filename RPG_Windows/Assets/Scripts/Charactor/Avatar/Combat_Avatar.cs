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
    protected AvatarInputActionsControls m_inputControls;

    #endregion

    #region 基本參數

    [Header("可操作角色戰鬥參數")]
    /// <summary>
    /// 角色是否為正在攻擊中
    /// </summary>
    public bool IsAttacking;
    /// <summary>
    /// 一次攻擊動畫所需的時間
    /// </summary>
    [SerializeField] protected float m_attackClipTime;
    /// <summary>
    /// 角色攻擊動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine m_attackRoutine;

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
        IsAttacking = true;
        
        m_avatar.SetCurrentBaseState(m_avatar.Attack);
        yield return new WaitForSeconds(m_attackClipTime);  // hardcasted casted time for debugged
        FinishAttack();
    }

    protected virtual void FinishAttack() {
        Debug.Log("Combat_Avatar FinishAttack start");
        if(m_attackRoutine != null) {
            StopCoroutine(m_attackRoutine);
        }

        IsAttacking = false;
        m_avatarMovement.SetMovement(m_avatarMovement.MovementAfterTrigger);
        m_avatarMovement.SetMovementAfterTrigger(Vector3.zero);
        
        if(m_avatarMovement.IsMoving) m_avatar.SetCurrentBaseState(m_avatar.Move);
        else m_avatar.SetCurrentBaseState(m_avatar.Idle);

        Debug.Log("Combat_Avatar FinishAttack end");
    }

    #endregion
}
