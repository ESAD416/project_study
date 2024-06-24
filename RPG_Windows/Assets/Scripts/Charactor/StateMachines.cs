using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 角色基本狀態機
public abstract class CharactorStateMachine<T1, T2>
    where T1 : Charactor<T2>
    where T2 : Collider2D
{
    protected T1 m_currentCharactor;

    protected Constant.CharactorState m_cState;
    public Constant.CharactorState State => m_cState;

    public abstract void OnEnter(T1 character);
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}


#region 角色基本狀態機 for Player

public abstract class CharactorStateMachine_Player<T> : CharactorStateMachine<Player<T>, T> where T : Collider2D 
{
    public abstract override void OnEnter(Player<T> player);
    public abstract override void OnEnter();
    public abstract override void OnUpdate();
    public abstract override void OnFixedUpdate();
    public abstract override void OnExit();
}

#region 基本狀態機 for Lamniat

public abstract class LamniatStateMachine : CharactorStateMachine_Player<BoxCollider2D>, ILamniatStateMachine 
{
    void ILamniatStateMachine.OnEnter(Lamniat lamniat)
    {
        this.m_currentCharactor = lamniat;
        this.OnEnter();
    }

    public abstract override void OnEnter(Player<BoxCollider2D> player);
    public abstract override void OnEnter();
    public abstract override void OnUpdate();
    public abstract override void OnFixedUpdate();
    public abstract override void OnExit();

}

// Lamniat 狀態機接口
public interface ILamniatStateMachine
{
    void OnEnter(Lamniat lamniat);
}

public class IdleState_Lamniat : LamniatStateMachine
{
    public IdleState_Lamniat(Lamniat lamniat) 
    {
        this.m_currentCharactor = lamniat;
        this.m_cState = Constant.CharactorState.Idle;
    }

    public override void OnEnter(Player<BoxCollider2D> lamniat)
    {
        this.m_currentCharactor = lamniat as Lamniat;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
    }
    public override void OnUpdate()
    {
        AnimeUtils.ActivateAnimatorLayer(this.m_currentCharactor.Animator, "IdleLayer");
    }
    public override void OnFixedUpdate()
    {
        // OnFixedUpdate
    }
    public override void OnExit()
    {
        // OnExit
    }
}

public class MoveState_Lamniat : LamniatStateMachine
{
    public MoveState_Lamniat(Lamniat lamniat)
    {
        this.m_currentCharactor = lamniat;
        this.m_cState = Constant.CharactorState.Move;
    }

    public override void OnEnter(Player<BoxCollider2D> lamniat)
    {
        this.m_currentCharactor = lamniat as Lamniat;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
    }

    public override void OnUpdate()
    {
        AnimeUtils.ActivateAnimatorLayer(this.m_currentCharactor.Animator, "MoveLayer");
    }

    public override void OnFixedUpdate()
    {
        // OnFixedUpdate
        //this.currentAvatar.Rigidbody.velocity = this.currentAvatar.AvatarMovement.Movement.normalized * this.currentAvatar.AvatarMovement.MoveSpeed;
    }

    public override void OnExit()
    {
        // OnExit
    }
}

public class AttackState_Lamniat : LamniatStateMachine
{
    public AttackState_Lamniat(Lamniat lamniat) 
    {
        this.m_currentCharactor = lamniat;
        this.m_cState = Constant.CharactorState.Attack;
    }


    public override void OnEnter(Player<BoxCollider2D> lamniat)
    {
        this.m_currentCharactor = lamniat as Lamniat;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
        //this.currentAvatar.Animator?.SetBool("isAttack", true);
    }

    public override void OnUpdate()
    {
        // var velocity = this.currentAvatar.AvatarMovement.MoveVelocity;
        // var moveSpeed = this.currentAvatar.AvatarMovement.MoveSpeed;
        // this.currentAvatar.AvatarMovement.SetMoveVelocity(Vector2.ClampMagnitude(velocity*0f, moveSpeed*0f));
        
        AnimeUtils.ActivateAnimatorLayer(this.m_currentCharactor.Animator, "AttackLayer");
    }

    public override void OnFixedUpdate()
    {
       //this.currentAvatar.AvatarMovement.SetMovement(Vector3.zero);
    }

    public override void OnExit()
    {
        // OnExit
    }
}

public class JumpState_Lamniat : LamniatStateMachine
{
    public JumpState_Lamniat(Lamniat lamniat) {
        this.m_currentCharactor = lamniat;
        this.m_cState = Constant.CharactorState.Jump;
    }

    public override void OnEnter(Player<BoxCollider2D> lamniat)
    {
        this.m_currentCharactor = lamniat as Lamniat;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
        this.m_currentCharactor.Animator?.SetTrigger("jump");
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        AnimeUtils.ActivateAnimatorLayer(this.m_currentCharactor.Animator, "TriggerLayer");
    }

    public override void OnFixedUpdate()
    {
    }

}

public class DodgeState_Lamniat : LamniatStateMachine
{

    public DodgeState_Lamniat(Lamniat lamniat) {
        this.m_currentCharactor = lamniat;
        this.m_cState = Constant.CharactorState.Dodge;
    }

    public override void OnEnter(Player<BoxCollider2D> lamniat)
    {
        this.m_currentCharactor = lamniat as Lamniat;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
        this.m_currentCharactor.Animator?.SetTrigger("dodge");
    }

    public override void OnUpdate()
    {
        //AnimeUtils.ActivateAnimatorLayer(this.currentAvatar.Animator, "TriggerLayer");
    }

    public override void OnFixedUpdate()
    {
        this.m_currentCharactor.PlayerMovement.SetMovement(Vector3.zero);
    }

    public override void OnExit()
    {
        // OnExit
    }
}

public class HurtState_Lamniat : LamniatStateMachine
{
    public HurtState_Lamniat(Lamniat lamniat) {
        this.m_currentCharactor = lamniat;
        this.m_cState = Constant.CharactorState.Hurt;
    }

    public override void OnEnter(Player<BoxCollider2D> lamniat)
    {
        this.m_currentCharactor = lamniat as Lamniat;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
        this.m_currentCharactor.Animator?.SetTrigger("hurt");
    }

    public override void OnUpdate()
    {
        //AnimeUtils.ActivateAnimatorLayer(this.currentAvatar.Animator, "TriggerLayer");
    }

    public override void OnFixedUpdate()
    {
       //this.currentAvatar.AvatarMovement.SetMovement(Vector3.zero);
    }

    public override void OnExit()
    {
        // OnExit
    }
}

public class DeadState_Lamniat : LamniatStateMachine
{
    public DeadState_Lamniat(Lamniat lamniat)
    { 
        this.m_currentCharactor = lamniat;
        this.m_cState = Constant.CharactorState.Dead;
    }

    public override void OnEnter(Player<BoxCollider2D> lamniat)
    {
        this.m_currentCharactor = lamniat as Lamniat;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
    }

    public override void OnUpdate()
    {
        // OnUpdate
    }

    public override void OnFixedUpdate()
    {
        // OnFixedUpdate
    }

    public override void OnExit()
    {
        // OnExit
    }
}

#endregion

#endregion

#region 基本狀態機 for Enemy

public abstract class CharactorStateMachine_Enemy<T> : CharactorStateMachine<Enemy<T>, T> where T : Collider2D 
{
    public abstract override void OnEnter(Enemy<T> enemy);
    public abstract override void OnEnter();
    public abstract override void OnUpdate();
    public abstract override void OnFixedUpdate();
    public abstract override void OnExit();
}

public class IdleState_Enemy<T> : CharactorStateMachine_Enemy<T> where T : Collider2D
{
    public IdleState_Enemy(Enemy<T> enemy) {
        this.m_currentCharactor = enemy;
        this.m_cState = Constant.CharactorState.Idle;
    }

    public override void OnEnter(Enemy<T> enemy)
    {
        this.m_currentCharactor = enemy;
        OnEnter();
    }
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }
    public override void OnFixedUpdate()
    {
    }
    public override void OnUpdate()
    {
        // Debug.Log("IdleState_Enemy OnUpdate");
        AnimeUtils.ActivateAnimatorLayer(this.m_currentCharactor.Animator, "IdleLayer");
    }
}

public class MoveState_Enemy<T> : CharactorStateMachine_Enemy<T> where T : Collider2D
{
    public MoveState_Enemy(Enemy<T> enemy) {
        this.m_currentCharactor = enemy;
        this.m_cState = Constant.CharactorState.Move;
    }

    public override void OnEnter(Enemy<T> enemy)
    {
        this.m_currentCharactor = enemy;
        OnEnter();
    }
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }
    public override void OnFixedUpdate()
    {
    }
    public override void OnUpdate()
    {
        
    }
}

public class AttackState_Enemy<T> : CharactorStateMachine_Enemy<T> where T : Collider2D 
{
    public AttackState_Enemy(Enemy<T> enemy) {
        this.m_currentCharactor = enemy;
        this.m_cState = Constant.CharactorState.Attack;
    }

    public override void OnEnter(Enemy<T> enemy)
    {
        this.m_currentCharactor = enemy;
        OnEnter();
    }
    public override void OnEnter()
    {
    }


    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}

public class JumpState_Enemy<T> : CharactorStateMachine_Enemy<T> where T : Collider2D 
{
    public JumpState_Enemy(Enemy<T> enemy) {
        this.m_currentCharactor = enemy;
        this.m_cState = Constant.CharactorState.Jump;
    }

    public override void OnEnter(Enemy<T> enemy)
    {
        this.m_currentCharactor = enemy;
        OnEnter();
    }
    public override void OnEnter()
    {
    }


    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}

public class DodgeState_Enemy<T> : CharactorStateMachine_Enemy<T> where T : Collider2D 
{
    public DodgeState_Enemy(Enemy<T> enemy) {
        this.m_currentCharactor = enemy;
        this.m_cState = Constant.CharactorState.Dodge;
    }

    public override void OnEnter(Enemy<T> enemy)
    {
        this.m_currentCharactor = enemy;
        OnEnter();
    }
    public override void OnEnter()
    {
    }


    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}

public class HurtState_Enemy<T> : CharactorStateMachine_Enemy<T> where T : Collider2D 
{
    public HurtState_Enemy(Enemy<T> enemy) {
        this.m_currentCharactor = enemy;
        this.m_cState = Constant.CharactorState.Hurt;
    }

    public override void OnEnter(Enemy<T> enemy)
    {
        this.m_currentCharactor = enemy;
        OnEnter();
    }
    public override void OnEnter()
    {
    }


    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}

public class DeadState_Enemy<T> : CharactorStateMachine_Enemy<T> where T : Collider2D 
{
    public DeadState_Enemy(Enemy<T> enemy) {
        this.m_currentCharactor = enemy;
        this.m_cState = Constant.CharactorState.Dead;
    }

    public override void OnEnter(Enemy<T> enemy)
    {
        this.m_currentCharactor = enemy;
        OnEnter();
    }
    public override void OnEnter()
    {
    }


    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}

#endregion


#region 基本狀態機 for Boss

public class IdleState_Boss : IdleState_Enemy<CircleCollider2D> 
{
    public IdleState_Boss(Enemy<CircleCollider2D> boss) : base(boss)
    {
        // 基類建構子已經初始化 m_currentCharactor 和 m_cState
        // 如果需要額外的初始化程式碼，可以在這裡添加
    }

    public override void OnEnter(Enemy<CircleCollider2D> boss)
    {
        this.m_currentCharactor = boss;
        OnEnter();
    }
}

public class MoveState_Boss : MoveState_Enemy<CircleCollider2D>
{
    public MoveState_Boss(Enemy<CircleCollider2D> boss) : base(boss)
    {
        // 基類建構子已經初始化 m_currentCharactor 和 m_cState
        // 如果需要額外的初始化程式碼，可以在這裡添加
    }

    public override void OnEnter(Enemy<CircleCollider2D> boss)
    {
        this.m_currentCharactor = boss;
        OnEnter();
    }

    public override void OnUpdate()
    {
        // OnUpdate
        AnimeUtils.ActivateAnimatorLayer(this.m_currentCharactor.Animator, "MoveLayer");
    }
}

public class DeadState_Boss : DeadState_Enemy<CircleCollider2D>
{
    public DeadState_Boss(Enemy<CircleCollider2D> boss) : base(boss)
    { 
        // 基類建構子已經初始化 m_currentCharactor 和 m_cState
        // 如果需要額外的初始化程式碼，可以在這裡添加
    }

    public override void OnEnter(Enemy<CircleCollider2D> boss)
    {
        this.m_currentCharactor = boss;
        OnEnter();
    }
}

#endregion


// Enemy狀態機
#region Enemy狀態機

public abstract class EnemyStateMachine<T> where T : Collider2D
{
    protected Constant.EnemyState m_eState;
    public Constant.EnemyState State => m_eState;

    protected Enemy<T> currentEnemy;

    public abstract void OnEnter(Enemy<T> enemy);
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}

public class PatrolState_Enemy<T> : EnemyStateMachine<T> where T : Collider2D
{
    public PatrolState_Enemy(Enemy<T> enemy) {
        this.currentEnemy = enemy;
        this.m_eState = Constant.EnemyState.Patrol;
    }

    public override void OnEnter(Enemy<T> enemy)
    {
        this.currentEnemy = enemy;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
    }

    public override void OnUpdate()
    {
        // 發現Player(Avatar)就轉移至ChaseState
    }

    public override void OnFixedUpdate()
    {
        // 物理引擎相關的Update
    }

    public override void OnExit()
    {
        // OnExit
    }
}

public class ChaseState_Enemy<T> : EnemyStateMachine<T> where T : Collider2D
{
    public ChaseState_Enemy(Enemy<T> enemy) {
        this.currentEnemy = enemy;
        this.m_eState = Constant.EnemyState.Chase;
    }

    public override void OnEnter(Enemy<T> enemy)
    {
        this.currentEnemy = enemy;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
    }

    public override void OnUpdate()
    {
        // OnUpdate
    }

    public override void OnFixedUpdate()
    {
        // 物理引擎相關的Update
    }

    public override void OnExit()
    {
        // OnExit
    }
}

#endregion

// Boss狀態機
#region Boss狀態機

public abstract class BossStateMachine<T> where T : Collider2D
{
    protected Constant.BossState m_bState;
    public Constant.BossState State => m_bState;

    protected Enemy<T> currentBoss;

    public abstract void OnEnter(Enemy<T> enemy);
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}

public class BeforeStartState<T> : BossStateMachine<T> where T : Collider2D 
{
    public BeforeStartState(Enemy<T> boss)
    {
        this.m_bState = Constant.BossState.BeforeStart;
        this.currentBoss = boss;
    }

    public override void OnEnter()
    {
        // OnEnter
        this.currentBoss.EnemyMovement.SetMoveSpeed(0f);
        this.currentBoss.SetCurrentBaseState(this.currentBoss.Idle);

        var hitSystem = this.currentBoss.GetComponent<HitSystem_Enemy>();
        if(hitSystem) hitSystem.IsIgnoreHit = true;
    }
    public override void OnEnter(Enemy<T> boss2)
    {
        this.currentBoss = boss2;
        OnEnter();
    }

    public override void OnUpdate()
    {
        // OnUpdate
    }

    public override void OnFixedUpdate()
    {
        // OnFixedUpdate
    }

    public override void OnExit()
    {
        // OnExit
    }
}

public class DuringBattleState<T> : BossStateMachine<T> where T : Collider2D 
{
    public DuringBattleState(Enemy<T> boss)
    {
        this.m_bState = Constant.BossState.DuringBattle;
        this.currentBoss = boss;
    }

    public override void OnEnter(Enemy<T> boss)
    {
        this.currentBoss = boss;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
        this.currentBoss.EnemyMovement.SetMoveSpeed(5f);
        this.currentBoss.SetCurrentBaseState(this.currentBoss.Move);

        var hitSystem = this.currentBoss.GetComponent<HitSystem_Enemy>();
        if(hitSystem) hitSystem.IsIgnoreHit = false;
    }

    public override void OnUpdate()
    {
        // OnUpdate
    }

    public override void OnFixedUpdate()
    {
        // OnFixedUpdate
    }

    public override void OnExit()
    {
        // OnExit
    }
}

public class BattleFinishState<T> : BossStateMachine<T> where T : Collider2D 
{
    public BattleFinishState(Enemy<T> boss)
    {
        this.m_bState = Constant.BossState.BattleFinish;
        this.currentBoss = boss;
    }

    public override void OnEnter(Enemy<T> boss)
    {
        this.currentBoss = boss;
        OnEnter();
    }
    public override void OnEnter()
    {
        // OnEnter
    }

    public override void OnUpdate()
    {
        // OnUpdate
    }

    public override void OnFixedUpdate()
    {
        // OnFixedUpdate
    }

    public override void OnExit()
    {
        // OnExit
    }
}

#endregion