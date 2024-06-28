using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy : ICharactor 
{
    public Constant.EnemyState CurrentEnemyStateName { get; }

    public void SetPatrolingState();
    public void SetPursuingState();
}

public class Enemy<T> : Charactor<T>, IEnemy where T : Collider2D
{
    [Header("Enemy 移動模式")]
    [SerializeField] protected Movement_Base m_enemyPatrolMovement;
    public Movement_Base EnemyPatrolMovement => this.m_enemyPatrolMovement;
    [SerializeField] protected Movement_Base m_enemyPursuingMovement;
    public Movement_Base EnemyPursuingMovement => this.m_enemyPursuingMovement;

    protected Movement_Base m_enemyCurrentMovement;
    public Movement_Base EnemyCurrentMovement => this.m_enemyCurrentMovement;



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
    [SerializeField] protected Constant.EnemyState m_currEnemyStateName;
    public Constant.EnemyState CurrentEnemyStateName => this.m_currEnemyStateName;

    protected EnemyStateMachine<T> m_patrol;
    public EnemyStateMachine<T> Patrol => this.m_patrol;
    protected EnemyStateMachine<T> m_pursuing;
    public EnemyStateMachine<T> Pursuing => this.m_pursuing;

    public void SetPatrolingState() {
        this.m_currentEnemyState?.OnExit();
        this.m_currentEnemyState = m_patrol;
        this.m_currentEnemyState.OnEnter(this);
    }

    public void SetPursuingState() {
        this.m_currentEnemyState?.OnExit();
        this.m_currentEnemyState = m_pursuing;
        this.m_currentEnemyState.OnEnter(this);
    }

    #endregion

    [Header("Enemy 基本參數")]
    protected bool m_isPatroling = false;
    // public bool IsPatroling => this.m_isPatroling;
    // public void SetIsPatroling(bool isPatroling) => this.m_isPatroling = isPatroling;
    
    protected bool m_isPursuing = false;
    // public bool IsPursuing => this.m_isPursuing;
    // public void SetIsPursuing(bool isPursuing) => this.m_isPursuing = isPursuing;

    protected override void Awake()
    {
        base.Awake();
        m_idle = new IdleState_Enemy<T>(this);
        m_move = new MoveState_Enemy<T>(this);
        m_attack = new AttackState_Enemy<T>(this);
        m_hurt = new HurtState_Enemy<T>(this);
        m_dead = new DeadState_Enemy<T>(this);

        m_patrol = new PatrolState_Enemy<T>(this);
        m_pursuing = new PursuingState_Enemy<T>(this);
    }

    protected virtual void OnEnable() {
        m_enemyPatrolMovement.gameObject.SetActive(true);
        m_enemyPursuingMovement.gameObject.SetActive(false);
        m_enemyCurrentMovement = this.m_enemyPatrolMovement;

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
        
        StartCoroutine(UpdateBaseState());
        //if(m_Animator != null) hitRecoveryTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_Animator, "Hurt");
        //isPatroling = true;
        
    }

    // Update is called once per frame
    protected override void Update() {
        //Debug.Log("moveSpeed: "+moveSpeed);
        m_currentBaseState.OnUpdate();
        m_currBaseStateName = m_currentBaseState.State;

        m_currentEnemyState.OnUpdate();
        m_currEnemyStateName = m_currentEnemyState.State;

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

    public virtual void InitMovement() {
        m_enemyCurrentMovement.StopMovement();

        m_enemyPatrolMovement.gameObject.SetActive(true);
        m_enemyPursuingMovement.gameObject.SetActive(false);
        m_enemyCurrentMovement = this.m_enemyPatrolMovement;

        m_enemyCurrentMovement.StartMovement();
    }

    public virtual void ChangeMovement(Movement_Base _movement) {
        m_enemyPatrolMovement.gameObject.SetActive(false);
        m_enemyPursuingMovement.gameObject.SetActive(false);

        _movement.gameObject.SetActive(true);
        m_enemyCurrentMovement = _movement;
    }

    private IEnumerator UpdateBaseState() {
        yield return null;  // hardcasted casted time for debugged
        
        if(m_enemyCurrentMovement.IsMoving) {
            m_currentBaseState = m_move;
        } 
        else {
            m_currentBaseState = m_idle;
        }

        StartCoroutine(UpdateBaseState());
    }

    private IEnumerator UpdateEnemyState() {
        yield return null;  // hardcasted casted time for debugged
        if(m_isPatroling && !m_isPursuing) {
            m_currentEnemyState = m_patrol;
        } else if(!m_isPatroling && m_isPursuing) {
            m_currentEnemyState = m_patrol;
        }
    }

}
