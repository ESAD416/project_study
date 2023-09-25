using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combat_Charactor : MonoBehaviour
{
    [SerializeField] protected Charactor m_charactor;
    [SerializeField] protected Animator m_targetAnimator;

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
    /// 記錄攻擊動作完成後的角色移動向量
    /// </summary>
    protected Vector3 movementAfterAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttacking) {
            Debug.Log("Combat_Charactor attack start");
        }
    }

    #region 攻擊控制
    protected IEnumerator Attack() {
        Debug.Log("Combat_Charactor attack start");
        isAttacking = true;
        m_targetAnimator?.SetBool("attack", isAttacking);
        yield return new WaitForSeconds(attackClipTime);  // hardcasted casted time for debugged
        FinishAttack();
    }

    public virtual void FinishAttack() {
        Debug.Log("Combat_Charactor FinishAttack start");
        if(attackRoutine != null) {
            StopCoroutine(attackRoutine);
        }

        isAttacking = false;
        m_targetAnimator?.SetBool("attack", isAttacking);

        m_charactor.SetMovement(movementAfterAttack);
        movementAfterAttack = Vector3.zero;
        Debug.Log("FinishAttack end");
    }
    #endregion
}
