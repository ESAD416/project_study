using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy<CircleCollider2D>
{
    private BossStateMachine<CircleCollider2D> m_currentBossState;
    public BossStateMachine<CircleCollider2D> CurrentBossState => this.m_currentBossState;
    public void SetCurrentBossState(BossStateMachine<CircleCollider2D> state) {
        this.m_currentBossState.OnExit();
        this.m_currentBossState = state;
        this.m_currentBossState.OnEnter(this);
    }


    protected BossStateMachine<CircleCollider2D> m_beforeStart;
    public BossStateMachine<CircleCollider2D> BeforeStart => this.m_beforeStart;
    protected BossStateMachine<CircleCollider2D> m_duringBattle;
    public BossStateMachine<CircleCollider2D> DuringBattle => this.m_duringBattle;
    protected BossStateMachine<CircleCollider2D> m_battleFinish;
    public BossStateMachine<CircleCollider2D> BattleFinish => this.m_battleFinish;

    protected override void Awake() {
        m_Coordinate = transform.position;
        
        
        m_idle = new IdleState_Boss(this);
        m_move = new MoveState_Boss(this);
        // m_attack = new AttackState_Boss(this);
        // m_hurt = new HurtState_Boss(this);
        m_dead = new DeadState_Boss(this);

        m_beforeStart = new BeforeStartState<CircleCollider2D>(this);
        m_duringBattle = new DuringBattleState<CircleCollider2D>(this);
        m_battleFinish = new BattleFinishState<CircleCollider2D>(this);
    }
    protected override void OnEnable() {
        m_enemyPatrolMovement.gameObject.SetActive(true);
        m_enemyPursuingMovement?.gameObject.SetActive(false);
        m_enemyCurrentMovement = this.m_enemyPatrolMovement;

        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter();

        m_currentBossState = m_beforeStart;
        m_currentBossState.OnEnter();

    }
    protected override void Start() {
        base.Start();
        
        this.m_enemyCurrentMovement.StopMovement();
    }

    protected override void Update() {
        m_currentBossState.OnUpdate();
        m_currentBaseState.OnUpdate();
        //Debug.Log("isMoving: "+this.m_enemyCurrentMovement.IsMoving);
        
        m_Center = CenterObj?.position ?? Vector3.zero;
        m_Buttom = ButtomObj?.position ?? Vector3.zero;
    }
    protected override void FixedUpdate() {
        m_currentBossState.OnFixedUpdate();

        m_currentBaseState.OnFixedUpdate();
    }

    protected override void OnDisable() {
        m_currentBossState.OnExit();

        m_currentBaseState.OnExit();
    }
    
}
