using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Avatar : MonoBehaviour
{
    #region 基本物件

    [Header("基本物件")]
    [SerializeField] protected Avatar m_avatar;
    [SerializeField] protected Movement_Avatar m_avatarMovement;
    protected Animator m_avatarAnimator;
    protected AvatarInputActionsControls inputControls;

    #endregion

    #region 基本參數

    [Header("基本參數")]
    public bool isDodging;
    /// <summary>
    /// 一次閃避動畫所需的時間
    /// </summary>
    [SerializeField] protected float dodgeClipTime;
    /// <summary>
    /// 角色閃避動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine dodgeRoutine;
    /// <summary>
    /// 記錄當下角色閃避動作時的移動向量並於攻擊動作完成後復原
    /// </summary>
    protected Vector3 movementAfterDodge;

    #endregion

    protected virtual void Awake() 
    {
        m_avatarAnimator = m_avatar.Animator;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    #region 閃避控制

    protected IEnumerator Dodge() {
        isDodging = true;
        //m_avatarAnimator?.SetTrigger("dodge");
        m_avatar.SetCurrentBaseState(m_avatar.Dodge);
        yield return new WaitForSeconds(dodgeClipTime);  // hardcasted casted time for debugged
        FinishDodge();
    }

    protected virtual void FinishDodge() {
        if(dodgeRoutine != null) {
            StopCoroutine(dodgeRoutine);
        }

        isDodging = false;

        m_avatarMovement.SetMovement(movementAfterDodge);
        
        if(m_avatarMovement.isMoving) m_avatar.SetCurrentBaseState(m_avatar.Move);
        else m_avatar.SetCurrentBaseState(m_avatar.Idle);

        movementAfterDodge = Vector3.zero;
    }

    #endregion
}
