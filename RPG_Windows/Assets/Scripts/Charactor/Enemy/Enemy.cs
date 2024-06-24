using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy : ICharactor 
{
    public Constant.CharactorState CurrentEnemyStateName { get; }
}

public class Enemy<T> : Charactor<T>, IEnemy where T : Collider2D
{
    [Header("Enemy 基本物件")]
    [SerializeField] protected Movement_Base m_enemyMovement;
    public Movement_Enemy EnemyMovement => m_enemyMovement as Movement_Enemy;

    #region 敵人狀態
    protected new CharactorStateMachine<Enemy<T>, T> m_currentBaseState;
    public new CharactorStateMachine<Enemy<T>, T> CurrentBaseState => m_currentBaseState;
    public void SetCurrentBaseState(CharactorStateMachine<Enemy<T>, T> state) {
        this.m_currentBaseState?.OnExit();
        this.m_currentBaseState = state;
        this.m_currentBaseState.OnEnter(this);
    }

    protected new CharactorStateMachine<Enemy<T>, T> m_idle;
    public new CharactorStateMachine<Enemy<T>, T> Idle => m_idle;
    protected new CharactorStateMachine<Enemy<T>, T> m_move;
    public new CharactorStateMachine<Enemy<T>, T> Move => m_move;
    protected new CharactorStateMachine<Enemy<T>, T> m_attack;
    public new CharactorStateMachine<Enemy<T>, T> Attack => m_attack;
    protected new CharactorStateMachine<Enemy<T>, T> m_jump;
    public new CharactorStateMachine<Enemy<T>, T> Jump => m_jump;
    protected new CharactorStateMachine<Enemy<T>, T> m_hurt;
    public new CharactorStateMachine<Enemy<T>, T> Hurt => m_hurt;
    protected new CharactorStateMachine<Enemy<T>, T> m_dodge;
    public new CharactorStateMachine<Enemy<T>, T> Dodge => m_dodge;
    protected new CharactorStateMachine<Enemy<T>, T> m_dead;
    public new CharactorStateMachine<Enemy<T>, T> Dead => m_dead;


    protected EnemyStateMachine<T> m_currentEnemyState;
    public EnemyStateMachine<T> CurrentEnemyState => m_currentEnemyState;
    public void SetCurrentEnemyState(EnemyStateMachine<T> state) {
        this.m_currentEnemyState?.OnExit();
        this.m_currentEnemyState = state;
        this.m_currentEnemyState.OnEnter(this);
    }
    [SerializeField] protected Constant.CharactorState m_currEnemyStateName;
    public Constant.CharactorState CurrentEnemyStateName => this.m_currEnemyStateName;

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

    protected virtual void OnEnable() {
        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter();

        m_currentEnemyState = m_patrol;
        m_currentEnemyState.OnEnter();
    }

    protected virtual void OnDisable() {
        m_currentBaseState?.OnExit();
        m_currentEnemyState?.OnExit();
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
        m_currBaseStateName = m_currentBaseState.State;

        m_currentEnemyState.OnUpdate();

        base.Update();
        //m_Animator?.SetFloat("moveSpeed", MoveSpeed);
    }

    protected override void FixedUpdate() {
        m_currentBaseState.OnFixedUpdate();
        m_currentEnemyState.OnFixedUpdate();

        base.FixedUpdate();
    }



    public virtual void OnAttack() {
        Debug.Log("Enemy_Abstract onAttack: ");
        // if(isMoving) {
        //     SetFacingDir(Movement);
        // }
        //attackRoutine = StartCoroutine(Attack());
    }

}
