using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;

public class BossStage_2 : Attack
{
    [Header("關卡物件")]
    [SerializeField] private ColliderTrigger m_bossVisableRange;
    [SerializeField] private Boss boss;
    [SerializeField] private AOECtrl indicatorCtrl;
    [SerializeField] private GameObject collider_beforeBattle;
    [SerializeField] private GameObject collider_duringBattle;

    [Header("關卡參數")]
    [SerializeField] private float m_indicatorDuration = 3f;
    [SerializeField] private float m_attackDuration = 0.65f;
    [SerializeField] private int m_countOfRandomLaunches = 2;
    private float m_delayTime = 1f;
    private Coroutine m_attackRoutine;
    private float m_timeToStartBattle = 5f;
    private float m_timeToShowAndHideBossUI = 3f;

    [Header("控制鏡頭")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner confiner;
    [SerializeField] private PolygonCollider2D defaultMapBound;
    [SerializeField] private PolygonCollider2D battleMapBound;

    // Start is called before the first frame update
    private void Start()
    {
        var healthBar = GetComponent<HealthSystem>().HealthBar;
        if(healthBar) healthBar.HideHealthBarUI();

        m_bossVisableRange.OnTargetEnterTriggerEvent.AddListener(OnPlayerEnterFight);

        collider_beforeBattle.SetActive(true);
        collider_duringBattle.SetActive(false);

        confiner = virtualCamera.GetComponent<CinemachineConfiner>();
    }

    protected override void Update()
    {

    }

    protected void OnPlayerEnterFight() {
        // Animation 戰鬥前動畫。duration : 1.5f
        Observable.Timer(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => boss.Animator.SetTrigger("fadeIn"))
            .AddTo(this);
        

        Observable.Timer(TimeSpan.FromSeconds(m_timeToShowAndHideBossUI +1f))
            .Subscribe(_ => DelayShowBossHealthUI())
            .AddTo(this);

        Observable.Timer(TimeSpan.FromSeconds(m_timeToStartBattle))
            .Subscribe(_ => {
                DelayStartBattle();

                collider_beforeBattle.SetActive(false);
                collider_duringBattle.SetActive(true);

                confiner.m_BoundingShape2D = battleMapBound;
                // 如果你在更改 Bounding Shape 后需要重新计算包围框，可以调用以下方法
                confiner.InvalidatePathCache();
            })
            .AddTo(this);
    }

    private void NextBossState() {
        switch(boss.CurrentBossState.State) {
            case Constant.BossState.BeforeStart: 
                boss.SetCurrentBossState(boss.DuringBattle);
                StopBattle();
                //AttackSimultaneouslyProcess();
                //AttackSeparatelyProcess(0.5f);
                AttackByTrailProcess(m_attackRate);
                break;
            case Constant.BossState.DuringBattle:
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

    public void DelayShowBossHealthUI() {
        var healthBar = GetComponent<HealthSystem>().HealthBar;
        if(healthBar) healthBar.ShowHealthBarUI();
    }

    private void DelayStartBattle() {
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

        // int count = indicatorCtrl.aoePositions.Count + m_countOfRandomLaunches;
        // if(count > 0) {
            m_attackRoutine = StartCoroutine(AttackCoroutines_Simultaneously());
        // }
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
            //Debug.Log("SeparatelyAttackCoroutines: one round end");
        }
    }

    private IEnumerator AttackCoroutines_ByTrail(float attackTimeSpan) {
        // 每個AOE攻擊的時段需要不同時地進行
        while(true) {
            // 1. 動態生成攻擊點
            Vector3 pos = indicatorCtrl.GetTrailedAOEPosition();
             // 2. 顯示指示器
            StartCoroutine(DisplayIndicator(pos));
            // 3. 控制攻擊的動畫與傷害機制 etc.
            StartCoroutine(LaunchAttack(pos));
            StartCoroutine(SetExplosion(pos));
            
            //TODO 增加隨機AOE指示器至Player附近By countOfRandomLaunches
           
            yield return new WaitForSeconds(attackTimeSpan);
            //Debug.Log("SeparatelyAttackCoroutines: one round end");
        }
    }

    private IEnumerator AttackCoroutines_Simultaneously() {
        // 所有的aoe攻擊是同時發動的

        while(true) {
            // 1. 動態生成攻擊點
            //indicatorCtrl.UpdateCombination();
            indicatorCtrl.GenerateSpawnPositions();
            // 2. 同時顯示指示器
            StartCoroutine(DisplayIndicators_Simultaneously());
            // 3. 同時控制攻擊的動畫與傷害機制 etc.
            StartCoroutine(LaunchAttack_Simultaneously(indicatorCtrl.SpawnPositions));
            StartCoroutine(SetExplosion_Simultaneously(indicatorCtrl.SpawnPositions));

            // 等待下一輪攻擊
            yield return new WaitForSeconds(m_indicatorDuration + m_attackDuration);
            // yield return new WaitForSeconds(m_indicatorDuration + m_attackDuration - m_delayTime);
        }
    }

    #endregion
    
    #region 顯示指示器
    
    private IEnumerator DisplayIndicator(Vector3 pos) {
        //Debug.Log("AttackCoroutines: DisplayIndicator start");

        indicatorCtrl.InstantiateAreaIndicator(pos, m_indicatorDuration);
        yield return null;

        //Debug.Log("AttackCoroutines: DisplayIndicator end");
    }

    private IEnumerator DisplayIndicators_Simultaneously() {
        //Debug.Log("AttackCoroutines: DisplayMutipleIndicatorsSimultaneously start");

        indicatorCtrl.InstantiateAreaIndicators(m_indicatorDuration);
        yield return null;
        
        //Debug.Log("AttackCoroutines: DisplayMutipleIndicatorsSimultaneously end");

        // TODO 增加隨機AOE指示器至Player附近By countOfRandomLaunches
    }

    #endregion

    #region 控制攻擊的動畫與傷害機制 etc.

    private IEnumerator LaunchAttack(Vector3 indicatorPos) {
        //Debug.Log("AttackCoroutines: LaunchAttack start");
        yield return new WaitForSeconds(m_indicatorDuration - m_attackDuration);

        GameObject projectile = ProjectilePool.instance.GetPooledGameObject();
        if(projectile != null) {
            // projectile.GetComponent<IndirectProjectile>();
            projectile.GetComponent<IndirectProjectile>().SetPositionOfBezierCurve(boss.Center, indicatorPos, 12f);
            projectile.GetComponent<IndirectProjectile>().SetDuration(m_attackDuration);

            projectile.SetActive(true);
        }
        //Debug.Log("AttackCoroutines: LaunchAttack end");
    }

    private IEnumerator SetExplosion(Vector3 indicatorPos) {
        //Debug.Log("AttackCoroutines: SetAttackHitBox start");
        yield return new WaitForSeconds(m_indicatorDuration);

        GameObject explosion = HitBoxPool.instance.GetPooledGameObject();
        if(explosion != null) {
            explosion.GetComponent<AreaExplosion>().SetByWhom(this);
            explosion.GetComponent<AreaExplosion>().SetPosition(indicatorPos);
            explosion.GetComponent<AreaExplosion>().SetDuration(1f);

            explosion.GetComponent<HitBox_Overlap2D>().SetAttacker(this);
            explosion.GetComponent<HitBox_Overlap2D>().DetectTagName = "Player";
            
            explosion.SetActive(true);
        }
        //Debug.Log("AttackCoroutines: SetAttackHitBox end");
    }

    private IEnumerator LaunchAttack_Simultaneously(List<Vector3> posList) {
        //Debug.Log("AttackCoroutines: LaunchAttackSimultaneously start");
        yield return new WaitForSeconds(m_indicatorDuration - m_attackDuration);
        //TODO 攻擊
        foreach(Vector2 pos in posList) {
            GameObject projectile = ProjectilePool.instance.GetPooledGameObject();
            if(projectile != null) {
                // projectile.GetComponent<IndirectProjectile>();
                projectile.GetComponent<IndirectProjectile>().SetPositionOfBezierCurve(boss.Center, pos, 12f);
                projectile.GetComponent<IndirectProjectile>().SetDuration(m_attackDuration);

                projectile.SetActive(true);
            }
        }

        //Debug.Log("AttackCoroutines: LaunchAttackSimultaneously end");
    }

    private IEnumerator SetExplosion_Simultaneously(List<Vector3> posList) {
        yield return new WaitForSeconds(m_indicatorDuration);

        foreach(Vector2 pos in posList) {
            GameObject explosion = HitBoxPool.instance.GetPooledGameObject();
            if(explosion != null) {
                explosion.GetComponent<AreaExplosion>().SetByWhom(this);
                explosion.GetComponent<AreaExplosion>().SetPosition(pos);
                explosion.GetComponent<AreaExplosion>().SetDuration(1f);

                explosion.GetComponent<HitBox_Overlap2D>().SetAttacker(this);
                explosion.GetComponent<HitBox_Overlap2D>().DetectTagName = "Player";
                
                explosion.SetActive(true);
            }
        }
        
    }

    #endregion

}
