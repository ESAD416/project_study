using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IPlayer : ICharactor 
{
    bool OnStairs { get; }
}

public class Player<T> : Charactor<T>, IPlayer  where T : Collider2D
{
    #region 可操作角色物件

    [Header("Player 基本物件")]
    [SerializeField] protected Movement_Base m_playerMovement;
    /// <summary>
    /// 可操作角色的移動控制
    /// </summary>
    public Movement_Player<T> PlayerMovement =>this.m_playerMovement as Movement_Player<T>;

    protected bool m_onStairs;
    public bool OnStairs => this.m_onStairs;
    public void SetOnStairs(bool onStairs) => this.m_onStairs = onStairs;

    #endregion

    #region 角色狀態

    protected new CharactorStateMachine<Player<T>, T> m_currentBaseState;
    public new CharactorStateMachine<Player<T>, T> CurrentBaseState => this.m_currentBaseState;
    public void SetCurrentBaseState(CharactorStateMachine<Player<T>, T> state)
    {
        this.m_currentBaseState?.OnExit();
        this.m_currentBaseState = state;
        this.m_currentBaseState.OnEnter(this);
    }

    protected new CharactorStateMachine<Player<T>, T> m_idle;
    public new CharactorStateMachine<Player<T>, T> Idle => m_idle;
    protected new CharactorStateMachine<Player<T>, T> m_move;
    public new CharactorStateMachine<Player<T>, T> Move => m_move;
    protected new CharactorStateMachine<Player<T>, T> m_attack;
    public new CharactorStateMachine<Player<T>, T> Attack => m_attack;
    protected new CharactorStateMachine<Player<T>, T> m_jump;
    public new CharactorStateMachine<Player<T>, T> Jump => m_jump;
    protected new CharactorStateMachine<Player<T>, T> m_hurt;
    public new CharactorStateMachine<Player<T>, T> Hurt => m_hurt;
    protected new CharactorStateMachine<Player<T>, T> m_dodge;
    public new CharactorStateMachine<Player<T>, T> Dodge => m_dodge;
    protected new CharactorStateMachine<Player<T>, T> m_dead;
    public new CharactorStateMachine<Player<T>, T> Dead => m_dead;


    #endregion

    protected override void Awake() {
        base.Awake();
    }

    protected virtual void OnEnable() {
        
        m_currHeight = InfoStorage.initialHeight;
        m_lastHeight = InfoStorage.initialHeight;
        transform.position = new Vector3(InfoStorage.initialPos.x, InfoStorage.initialPos.y);

        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter();
    }

    protected virtual void OnDisable() {
        m_currentBaseState?.OnExit();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update()
    {
        m_currentBaseState.OnUpdate();
        m_currBaseStateName = m_currentBaseState.State;

        if(m_onStairs) m_SprtRenderer.sortingLayerID = SortingLayer.NameToID("Character");
        else m_SprtRenderer.sortingLayerID = SortingLayer.NameToID("Default");

        base.Update();
        // Debug.Log("m_currentBaseState: "+m_currentBaseState.State);
    }

    protected override void FixedUpdate() {
        
        // if(isJumping && !CharStatus.Equals(CharactorStatus.Attack)) {
        //     transform.position = GetWorldPosByCoordinate(m_Coordinate) - new Vector3(0, 1.7f);   // 預設中心點是(x, y+1.7)
        //     HandleJumpingProcess(jumpState);
        // }

        base.FixedUpdate();
    }

}
