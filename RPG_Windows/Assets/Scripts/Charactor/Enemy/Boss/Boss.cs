using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private BossStateMachine m_currentBossState;
    public BossStateMachine CurrentBossState => this.m_currentBossState;
    public void SetCurrentBossState(BossStateMachine state) {
        this.m_currentBossState.OnExit();
        this.m_currentBossState = state;
        this.m_currentBossState.OnEnter(this);
    }

    protected BossStateMachine m_beforeStart;
    public BossStateMachine BeforeStart => this.m_beforeStart;
    protected BossStateMachine m_duringBattle;
    public BossStateMachine DuringBattle => this.m_duringBattle;
    protected BossStateMachine m_battleFinish;
    public BossStateMachine BattleFinish => this.m_battleFinish;

    protected override void Awake() {
        m_Coordinate = transform.position;
        
        m_idle = new IdleState_Boss(this);
        m_move = new MoveState_Boss(this);
        // m_attack = new AttackState_Boss(this);
        // m_hurt = new HurtState_Boss(this);
        m_dead = new DeadState_Boss(this);

        m_beforeStart = new BeforeStartState_Boss(this);
        m_duringBattle = new DuringBattleState_Boss(this);
        m_battleFinish = new BattleFinishState_Boss(this);
    }
    protected override void OnEnable() {
        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter();

        m_currentBossState = m_beforeStart;
        m_currentBossState.OnEnter();

    }
    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        m_currentBossState.OnUpdate();

        m_currentBaseState.OnUpdate();

        m_Center = m_CenterObj?.position ?? Vector3.zero;
        m_Buttom = m_ButtomObj?.position ?? Vector3.zero;
    }
    protected override void FixedUpdate() {
        m_currentBossState.OnFixedUpdate();

        m_currentBaseState.OnFixedUpdate();
    }

    protected override void OnDisable() {
        m_currentBossState.OnExit();

        m_currentBaseState.OnExit();
    }

    private void OnDamaged() {
        // Area took damage
        // switch(stage) {
        //     case Stage.Stage1:
        //         if(boss.HealthSystem.GetCurrHealthNormalized() <= 0.7f) {
        //             // if under 70% health
        //             StartNextStage();
        //         }
        //         break;
        //     case Stage.Stage2:
        //         if(boss.HealthSystem.GetCurrHealthNormalized() <= 0.5f) {
        //             // if under 50% health
        //             StartNextStage();
        //         }
        //         break;
        //     case Stage.Stage3:
        //         if(boss.HealthSystem.GetCurrHealthNormalized() <= 0.2f) {
        //             // if under 20% health
        //             StartNextStage();
        //         }
        //         break;
        // }
    }
    
}
