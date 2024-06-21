using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy<T> : Charactor<T> where T : Collider2D
{
    [Header("Enemy 基本物件")]
    [SerializeField] protected Movement_Enemy m_enemyMovement;
    public Movement_Enemy EnemyMovement => m_enemyMovement;
    [SerializeField] protected HealthBar healthBar;

    #region 敵人狀態
    protected CharactorStateMachine<Enemy<T>, T> m_currentBaseState;
    public CharactorStateMachine<Enemy<T>, T> CurrentBaseState => m_currentBaseState;
    public void SetCurrentBaseState(CharactorStateMachine<Enemy<T>, T> state) {
        this.m_currentBaseState.OnExit();
        this.m_currentBaseState = state;
        this.m_currentBaseState.OnEnter(this);
    }

    protected CharactorStateMachine<Enemy<T>, T> m_idle;
    public CharactorStateMachine<Enemy<T>, T> Idle => this.m_idle;
    protected CharactorStateMachine<Enemy<T>, T> m_move;
    public CharactorStateMachine<Enemy<T>, T> Move => this.m_move;
    protected CharactorStateMachine<Enemy<T>, T> m_attack;
    public CharactorStateMachine<Enemy<T>, T> Attack => this.m_attack;
    protected CharactorStateMachine<Enemy<T>, T> m_hurt;
    public CharactorStateMachine<Enemy<T>, T> Hurt => this.m_hurt;
    protected CharactorStateMachine<Enemy<T>, T> m_dead;
    public CharactorStateMachine<Enemy<T>, T> Dead => this.m_dead;


    protected EnemyStateMachine<T> m_currentEnemyState;
    public EnemyStateMachine<T> CurrentEnemyState => m_currentEnemyState;
    public void SetCurrentEnemyState(EnemyStateMachine<T> state) {
        this.m_currentEnemyState.OnExit();
        this.m_currentEnemyState = state;

        this.m_currentEnemyState.OnEnter(this);
    }

    protected EnemyStateMachine<T> m_patrol;
    public EnemyStateMachine<T> Patrol => this.m_patrol;
    protected EnemyStateMachine<T> m_chase;
    public EnemyStateMachine<T> Chase => this.m_chase;

    #endregion

    [Header("Enemy 基本參數")]
    public bool isPatroling = false;
    public bool isChasing = false;

    protected override void Awake()
    {
        base.Awake();
        m_idle = new IdleState_Enemy<T>(this);
        m_move = new MoveState_Enemy<T>(this);
        m_attack = new AttackState_Enemy<T>(this);
        m_hurt = new HurtState_Enemy<T>(this);
        m_dead = new DeadState_Enemy<T>(this);

        m_patrol = new PatrolState_Enemy<T>(this);
        m_chase = new ChaseState_Enemy<T>(this);
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
