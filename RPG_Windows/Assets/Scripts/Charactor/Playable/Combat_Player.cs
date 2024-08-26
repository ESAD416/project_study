using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Combat_Player<T> : Attack_System where T : Collider2D
{
    #region 基本物件

    [Header("Combat_Avatar 物件")]
    [SerializeField] protected Player<T> m_player;
    
    protected Movement_Player<T> m_playerMovement;    
    protected Animator m_playerAnimator;

    #endregion

    #region 基本參數

    [Header("Combat_Avatar 參數")]
    /// <summary>
    /// 角色是否為正在攻擊中
    /// </summary>
    public bool IsAttacking = false;
    /// <summary>
    /// 角色是否可以攻擊
    /// </summary>
    public bool CanAttack = false;
    /// <summary>
    /// 角色是否為正在攻擊前搖中
    /// </summary>
    public bool IsPreAttacking = false;
    /// <summary>
    /// 角色是否為正在攻擊後搖中
    /// </summary>
    public bool IsPostAttacking = false;
    /// <summary>
    /// 角色是否可以取消硬直(前/後搖)
    /// </summary>
    public bool CancelRecovery = false;
    [SerializeField] protected float m_attackClipTime;
    
    #endregion

    protected virtual void Awake() {
        m_playerMovement = m_player.PlayerMovement;
        m_playerAnimator = m_player.Animator;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // Debug.Log("Combat_Avatar IsAttacking: "+IsAttacking);
        // Debug.Log("Combat_Avatar IsPreAttacking: "+IsPreAttacking);
        // Debug.Log("Combat_Avatar IsPostAttacking: "+IsPostAttacking);
    }

    #region 攻擊控制

    protected virtual IEnumerator Attack() {
        Debug.Log("Combat_Avatar attack start");
        IsAttacking = true;
        m_playerMovement.CanMove = false;
        
        m_player.SetCurrentBaseState(m_player.Attack);
        yield return new WaitForSeconds(m_attackClipTime);  // hardcasted casted time for debugged
        FinishAttack();
    }

    protected virtual void FinishAttack() {
        Debug.Log("Combat_Avatar FinishAttack start");

        IsAttacking = false;
        IsPreAttacking = false;
        IsPostAttacking = false;
        m_playerMovement.CanMove = true;
        
        //m_avatarMovement.SetMovement(m_avatarMovement.MovementAfterTrigger);
        
        if(m_playerMovement.IsMoving) m_player.SetCurrentBaseState(m_player.Move);
        else m_player.SetCurrentBaseState(m_player.Idle);

        Debug.Log("Combat_Avatar FinishAttack end");
    }

    #endregion

}
