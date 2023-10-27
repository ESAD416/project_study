using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage_2 : Attack
{
    [Header("關卡物件")]
    [SerializeField] private Boss boss;
    [SerializeField] private Avatar player;
    [SerializeField] private AOECtrl indicatorCtrl;

    [Header("關卡參數")]
    [SerializeField] private float m_indicatorDuration = 3f;
    [SerializeField] private float m_attackDuration = 0.65f;
    [SerializeField] private int m_countOfRandomLaunches = 2;
    private float m_delayTime = 1f;
    private Coroutine m_attackRoutine;
    private float m_timeToStartBattle = 3f;

    // Start is called before the first frame update
    private void Start()
    {
        Invoke("StartBattle", m_timeToStartBattle);
    }

    protected override void Update()
    {
        bool targetAttacked = m_OnHit != null && m_OnHit.Length > 0;
        if (targetAttacked) {
            Debug.Log("targetAttacked");
            foreach (Collider2D col in m_OnHit) {
                if (col.GetComponent<HitSystem>() != null) {
                    col.GetComponent<HitSystem>().TakeHiProcess(this.DamageSystem, this.transform);
                }
            }
        }
    }

    private void NextBossState() {
        switch(boss.CurrentBossState.Stage) {
            case BossStateMachine.BossState.BeforeStart: 
                boss.SetCurrentBossState(boss.DuringBattle);
                StopBattle();
                //AttackSimultaneouslyProcess();
                //AttackSeparatelyProcess(0.5f);
                AttackByTrailProcess(m_attackRate);
                break;
            case BossStateMachine.BossState.DuringBattle:
                StopBattle();
                //AttackSeparatelyProcess(0.5f);
                //InvokeRepeating("LineAttackProcess", 0f, timeBetweenAttacks);
                break;
        }
    }

    private void UpdateStageMode(float id, float ad, int corl) {
        this.m_indicatorDuration = id;
        this.m_attackDuration = ad;
        this.m_countOfRandomLaunches = corl;
    }

    #region 攻擊 啟動/關閉

    private void StartBattle() {
        Debug.Log("StartBattle");
        NextBossState();
    }

    private void StopBattle() {
        StopAttack();
    }

    private void StopAttack() {
        StopAllCoroutines();

        if(m_attackRoutine != null) {
            StopCoroutine(m_attackRoutine);
        }
    }

    #endregion

    #region 攻擊模式

    public void AttackSeparatelyProcess(float attackTimeSpan) {
        Debug.Log("indicatorDuration: "+m_indicatorDuration);
        Debug.Log("attackDuration: "+m_attackDuration);
        Debug.Log("countOfRandomLaunches: "+m_countOfRandomLaunches);
        // int count = indicatorCtrl.aoePositions.Count + countOfRandomLaunches;
        // if(count > 0) {
        //     attackRoutine = StartCoroutine(SeparatelyAttackCoroutines(attackTimeSpan));
        // }

        m_attackRoutine = StartCoroutine(AttackCoroutines_Separately(attackTimeSpan));
    }

    public void AttackByTrailProcess(float attackTimeSpan) {
        Debug.Log("indicatorDuration: "+m_indicatorDuration);
        Debug.Log("attackDuration: "+m_attackDuration);
        Debug.Log("countOfRandomLaunches: "+m_countOfRandomLaunches);
        
        m_attackRoutine = StartCoroutine(AttackCoroutines_ByTrail(attackTimeSpan));
    }

    public void AttackSimultaneouslyProcess() {
        Debug.Log("indicatorDuration: "+m_indicatorDuration);
        Debug.Log("attackDuration: "+m_attackDuration);
        Debug.Log("countOfRandomLaunches: "+m_countOfRandomLaunches);

        int count = indicatorCtrl.aoePositions.Count + m_countOfRandomLaunches;
        if(count > 0) {
            m_attackRoutine = StartCoroutine(AttackCoroutines_Simultaneously());
        }
    }

    #endregion

    #region 攻擊模式的協程

    private IEnumerator AttackCoroutines_Separately(float attackTimeSpan) {
        // 每個AOE攻擊的時段需要不同時地進行
        while(true) {
            // 1. 動態生成攻擊點
            Vector3 pos = indicatorCtrl.GetAOEPositionByStackCombination();
            // 2. 顯示指示器
            StartCoroutine(DisplayIndicator(pos));
            // 3. 控制攻擊的動畫與傷害機制 etc.
            StartCoroutine(LaunchAttack(pos));
            
            //TODO 增加隨機AOE指示器至Player附近By countOfRandomLaunches

            yield return new WaitForSeconds(attackTimeSpan);
            Debug.Log("SeparatelyAttackCoroutines: one round end");
        }
    }

    private IEnumerator AttackCoroutines_ByTrail(float attackTimeSpan) {
        // 每個AOE攻擊的時段需要不同時地進行
        while(true) {
            // 1. 動態生成攻擊點
            Vector3 pos = indicatorCtrl.GetTrailedAOEPosition(player);

            StartCoroutine(DisplayIndicator(pos));
            // 3. 控制攻擊的動畫與傷害機制 etc.
            StartCoroutine(LaunchAttack(pos));
            StartCoroutine(SetExplosion(pos));
            
            //TODO 增加隨機AOE指示器至Player附近By countOfRandomLaunches

            yield return new WaitForSeconds(attackTimeSpan);
            Debug.Log("SeparatelyAttackCoroutines: one round end");
        }
    }

    private IEnumerator AttackCoroutines_Simultaneously() {
        // 所有的aoe攻擊*count是同時發動的

        while(true) {
            // 1. 動態生成攻擊點
            indicatorCtrl.UpdateCombination();
            // 2. 同時顯示指示器
            StartCoroutine(DisplayIndicators_Simultaneously());
            // 3. 同時控制攻擊的動畫與傷害機制 etc.
            StartCoroutine(LaunchAttack_Simultaneously());

            // 等待下一輪攻擊
            yield return new WaitForSeconds(m_indicatorDuration + m_attackDuration - m_delayTime);
        }
    }

    #endregion
    
    #region 顯示指示器
    
    private IEnumerator DisplayIndicator(Vector3 pos) {
        Debug.Log("AttackCoroutines: DisplayIndicator start");

        indicatorCtrl.InstantiateAreaIndicator(pos, m_indicatorDuration);
        yield return null;

        Debug.Log("AttackCoroutines: DisplayIndicator end");
    }

    private IEnumerator DisplayIndicators_Simultaneously() {
        Debug.Log("AttackCoroutines: DisplayMutipleIndicatorsSimultaneously start");
        yield return null;

        indicatorCtrl.InstantiateAreaIndicators(m_indicatorDuration);
        Debug.Log("AttackCoroutines: DisplayMutipleIndicatorsSimultaneously end");

        // TODO 增加隨機AOE指示器至Player附近By countOfRandomLaunches
    }

    #endregion

    #region 控制攻擊的動畫與傷害機制 etc.

    private IEnumerator LaunchAttack(Vector3 indicatorPos) {
        Debug.Log("AttackCoroutines: LaunchAttack start");
        yield return new WaitForSeconds(m_indicatorDuration - m_attackDuration);

        GameObject projectile = ProjectilePool.instance.GetPooledGameObject();
        if(projectile != null) {
            // projectile.GetComponent<IndirectProjectile>();
            projectile.GetComponent<IndirectProjectile>().SetPositionOfBezierCurve(boss.Center, indicatorPos, 12f);
            projectile.GetComponent<IndirectProjectile>().SetDuration(m_attackDuration);

            projectile.SetActive(true);
        }
        Debug.Log("AttackCoroutines: LaunchAttack end");
    }

    private IEnumerator SetExplosion(Vector3 indicatorPos) {
        Debug.Log("AttackCoroutines: SetAttackHitBox start");
        yield return new WaitForSeconds(m_indicatorDuration);

        GameObject explosion = HitBoxPool.instance.GetPooledGameObject();
        if(explosion != null) {
            explosion.GetComponent<AreaExplosion>().SetByWhom(this);
            explosion.GetComponent<AreaExplosion>().SetPosition(indicatorPos);
            explosion.GetComponent<AreaExplosion>().SetDuration(1f);

            explosion.GetComponent<HitBox_Overlap2D>().SetAttacker(this);
            
            explosion.SetActive(true);
        }
        Debug.Log("AttackCoroutines: SetAttackHitBox end");
    }

    private IEnumerator LaunchAttack_Simultaneously() {
        Debug.Log("AttackCoroutines: LaunchAttackSimultaneously start");
        yield return new WaitForSeconds(m_indicatorDuration);
        //TODO 攻擊
        Debug.Log("AttackCoroutines: LaunchAttackSimultaneously end");
    }

    #endregion

}
