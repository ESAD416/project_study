using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    [Header("Enemy 基本物件")]
    [SerializeField] protected Movement_Enemy m_enemyMovement;
    public Movement_Enemy EnemyMovement => m_enemyMovement;
    [SerializeField] protected HealthBar healthBar;

    #region 敵人狀態
    protected BaseStateMachine_Enemy m_currentBaseState;
    public BaseStateMachine_Enemy CurrentBaseState => m_currentBaseState;
    public void SetCurrentBaseState(BaseStateMachine_Enemy state) {
        this.m_currentBaseState.OnExit();
        this.m_currentBaseState = state;
        this.m_currentBaseState.OnEnter(this);
    }

    protected BaseStateMachine_Enemy m_idle;
    public BaseStateMachine_Enemy Idle => this.m_idle;
    protected BaseStateMachine_Enemy m_move;
    public BaseStateMachine_Enemy Move => this.m_move;
    protected BaseStateMachine_Enemy m_attack;
    public BaseStateMachine_Enemy Attack => this.m_attack;
    protected BaseStateMachine_Enemy m_hurt;
    public BaseStateMachine_Enemy Hurt => this.m_hurt;
    protected BaseStateMachine_Enemy m_dead;
    public BaseStateMachine_Enemy Dead => this.m_dead;


    protected EnemyStateMachine m_currentEnemyState;
    public EnemyStateMachine CurrentEnemyState => m_currentEnemyState;
    public void SetCurrentEnemyState(EnemyStateMachine state) {
        this.m_currentEnemyState.OnExit();
        this.m_currentEnemyState = state;
        this.m_currentEnemyState.OnEnter(this);
    }

    protected EnemyStateMachine m_patrol;
    public EnemyStateMachine Patrol => this.m_patrol;
    protected EnemyStateMachine m_chase;
    public EnemyStateMachine Chase => this.m_chase;

    #endregion

    [Header("Enemy 基本參數")]
    public bool isPatroling = false;
    public bool isChasing = false;

    protected override void Awake()
    {
        base.Awake();
        m_idle = new IdleState_Enemy(this);
        m_move = new MoveState_Enemy(this);
        m_attack = new AttackState_Enemy(this);
        m_hurt = new HurtState_Enemy(this);
        m_dead = new DeadState_Boss(this);

        m_patrol = new PatrolState_Enemy(this);
        m_chase = new ChaseState_Enemy(this);
    }

    protected override void OnEnable() {
        base.OnEnable();

        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter();

        m_currentEnemyState = m_patrol;
        m_currentEnemyState.OnEnter();

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
        m_currentEnemyState.OnUpdate();

        base.Update();
        //m_Animator?.SetFloat("moveSpeed", MoveSpeed);
    }

    protected override void FixedUpdate() {
        m_currentBaseState.OnFixedUpdate();
        m_currentEnemyState.OnFixedUpdate();

        base.FixedUpdate();
    }

    protected virtual void OnDisable() {
        m_currentBaseState.OnExit();
        m_currentEnemyState.OnExit();
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
