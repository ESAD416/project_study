using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Abstract : Charactor
{
    [SerializeField] protected HealthBar healthBar;

    #region 敵人狀態

    public enum EnemyStatus {
        Patrol, Chase, 
    }
    protected EnemyStatus e_Status;
    public EnemyStatus EneStatus => e_Status;
    public void SetStatus(EnemyStatus status) => this.e_Status = status;

    #endregion

    public bool isPatroling = false;
    public bool isChasing = false;

    protected override void Awake()
    {
        base.Awake();
        e_Status = EnemyStatus.Patrol;
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        //if(m_Animator != null) hitRecoveryTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_Animator, "Hurt");
        //isPatroling = true;
        
    }

    // Update is called once per frame
    protected override void Update() {
        //Debug.Log("moveSpeed: "+moveSpeed);
        m_Center = m_CenterObj?.position ?? Vector3.zero;
        m_Buttom = m_ButtomObj?.position ?? Vector3.zero;

        UpdateCoordinate();

        //m_Animator?.SetFloat("moveSpeed", MoveSpeed);
    }

    protected override void FixedUpdate() {
        
    }

    public virtual void OnAttack() {
        Debug.Log("Enemy_Abstract onAttack: ");
        // if(isMoving) {
        //     SetFacingDir(Movement);
        // }
        //attackRoutine = StartCoroutine(Attack());
    }

    // public override void DmgCalculate(int damage)
    // {
    //     base.DmgCalculate(damage);
    //     healthBar.SetHealth(currHealth, maxHealth);
    // }
}
