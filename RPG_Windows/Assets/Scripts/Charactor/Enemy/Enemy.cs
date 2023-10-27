using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    [Header("Avatar基本物件")]
    [SerializeField] protected Movement_Enemy m_enemyMovement;
    [SerializeField] protected HealthBar healthBar;

    #region 敵人狀態
    private BaseStateMachine_Enemy m_currentBaseState;
    public BaseStateMachine_Enemy CurrentBaseState => m_currentBaseState;
    public void SetCurrentBaseState(BaseStateMachine_Enemy state) {
        this.m_currentBaseState.OnExit();
        this.m_currentBaseState = state;
        this.m_currentBaseState.OnEnter(this);
    }

    protected BaseStateMachine_Enemy m_idle;
    public BaseStateMachine_Enemy Idle => m_idle;
    protected BaseStateMachine_Enemy m_move;
    public BaseStateMachine_Enemy Move => m_move;
    protected BaseStateMachine_Enemy m_hurt;
    public BaseStateMachine_Enemy Hurt => m_hurt;
    protected BaseStateMachine_Enemy m_dead;
    public BaseStateMachine_Enemy Dead => m_dead;


    private EnemyStateMachine m_currentEnemyState;
    public EnemyStateMachine CurrentEnemyState => m_currentEnemyState;
    public void SetCurrentEnemyState(EnemyStateMachine state) {
        this.m_currentEnemyState.OnExit();
        this.m_currentEnemyState = state;
        this.m_currentEnemyState.OnEnter(this);
    }

    protected EnemyStateMachine m_patrol;
    protected EnemyStateMachine Patrol => m_patrol;
    protected EnemyStateMachine m_chase;
    protected EnemyStateMachine Chase => m_chase;

    #endregion

    public bool isPatroling = false;
    public bool isChasing = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable() {
        base.OnEnable();

        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter();

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
        m_currentBaseState.OnUpdate();

        base.Update();
        //m_Animator?.SetFloat("moveSpeed", MoveSpeed);
    }

    protected override void FixedUpdate() {
        m_currentBaseState.OnFixedUpdate();

        base.FixedUpdate();
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
