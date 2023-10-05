using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    [SerializeField] protected Movement_Enemy m_enemyMovement;
    [SerializeField] protected HealthBar healthBar;

    #region 敵人狀態
    private BaseStateMachine_Enemy m_currentBaseState;
    public BaseStateMachine_Enemy CurrentBaseState => m_currentBaseState;
    protected BaseStateMachine_Enemy m_idle;
    public BaseStateMachine_Enemy Idle => m_idle;
    protected BaseStateMachine_Enemy m_move;
    public BaseStateMachine_Enemy Move => m_move;
    protected BaseStateMachine_Enemy m_dead;
    public BaseStateMachine_Enemy Dead => m_dead;


    private EnemyStateMachine m_currentEnemyState;
    public EnemyStateMachine CurrentEnemyState => m_currentEnemyState;
    protected EnemyStateMachine m_patrol;
    protected EnemyStateMachine Patrol => m_patrol;
    protected EnemyStateMachine m_chase;
    protected EnemyStateMachine Chase => m_chase;

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
    }

    protected override void OnEnable() {
        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter(this);

        base.OnEnable();
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
        base.Update();

        m_currentBaseState.OnUpdate();
        //m_Animator?.SetFloat("moveSpeed", MoveSpeed);
    }

    protected override void FixedUpdate() {
        m_currentBaseState.OnFixedUpdate();
    }

    protected virtual void OnDisable() {
        m_currentBaseState.OnExit();
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
