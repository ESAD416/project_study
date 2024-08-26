using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamniatStateController : MonoBehaviour
{
    [SerializeField] private Lamniat m_lamniatPlayer;

    [SerializeField] protected Constant.CharactorState m_currBaseStateName;
    public Constant.CharactorState CurrentBaseStateName => this.m_currBaseStateName;
    LamniatStateMachine m_currentBaseState;
    public LamniatStateMachine CurrentBaseState => this.m_currentBaseState;
    public void SetCurrentBaseState(LamniatStateMachine state) {
        Debug.Log("state "+state.GetType());

        this.m_currentBaseState?.OnExit();
        this.m_currentBaseState = state;
        this.m_currentBaseState.OnEnter(m_lamniatPlayer);
    }

    protected LamniatStateMachine m_idle;
    public LamniatStateMachine Idle => m_idle;
    protected LamniatStateMachine m_move;
    public LamniatStateMachine Move => m_move;
    protected LamniatStateMachine m_attack;
    public LamniatStateMachine Attack => m_attack;
    protected LamniatStateMachine m_jump;
    public LamniatStateMachine Jump => m_jump;
    protected LamniatStateMachine m_hurt;
    public LamniatStateMachine Hurt => m_hurt;
    protected LamniatStateMachine m_dodge;
    public LamniatStateMachine Dodge => m_dodge;
    protected LamniatStateMachine m_dead;
    public LamniatStateMachine Dead => m_dead;

    protected void OnEnable() 
    {
        m_idle = new IdleState_Lamniat(m_lamniatPlayer);
        m_move = new MoveState_Lamniat(m_lamniatPlayer);
        m_attack = new AttackState_Lamniat(m_lamniatPlayer);
        m_jump = new JumpState_Lamniat(m_lamniatPlayer);
        m_dodge = new DodgeState_Lamniat(m_lamniatPlayer);
        m_hurt = new HurtState_Lamniat(m_lamniatPlayer);
        m_dead = new DeadState_Lamniat(m_lamniatPlayer);
        
        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        m_currentBaseState.OnUpdate();
        m_currBaseStateName = m_currentBaseState.State;
    }

    protected void FixedUpdate() {
        m_currentBaseState.OnFixedUpdate();
    }

    protected virtual void OnDisable() {
        m_currentBaseState?.OnExit();
    }
}
