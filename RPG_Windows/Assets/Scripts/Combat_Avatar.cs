using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Avatar : MonoBehaviour
{
    [SerializeField] protected Avatar m_avatar;
    [SerializeField] protected Movement_Avatar m_targetMovement;
    protected Animator m_targetAnimator;

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

    protected virtual void Awake() {
        m_targetAnimator = m_avatar.Animator;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    #region 攻擊控制
    protected IEnumerator Attack() {
        Debug.Log("Combat_Avatar attack start");
        isAttacking = true;
        m_targetAnimator?.SetTrigger("attack");
        m_avatar.SetStatus(Charactor.CharactorStatus.Attack);
        yield return new WaitForSeconds(attackClipTime);  // hardcasted casted time for debugged
        FinishAttack();
    }

    protected virtual void FinishAttack() {
        Debug.Log("Combat_Avatar FinishAttack start");
        if(attackRoutine != null) {
            StopCoroutine(attackRoutine);
        }

        isAttacking = false;

        m_targetMovement.SetMovement(movementAfterAttack);
        
        if(m_targetMovement.isMoving) m_avatar.SetStatus(Charactor.CharactorStatus.Move);
        else m_avatar.SetStatus(Charactor.CharactorStatus.Idle);

        movementAfterAttack = Vector3.zero;
        Debug.Log("Combat_Avatar FinishAttack end");
    }
    #endregion
}
