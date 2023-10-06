using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage_2 : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private Avatar player;
    [SerializeField] private AOEIndicatorCtrl indicatorCtrl;

    private float timeToStartBattle = 3f;
    private float indicatorDuration = 3f;
    private float attackDuration = 0.65f;
    private float delayTime = 1f;
    private int countOfRandomLaunches = 2;
    private Coroutine attackRoutine;

    // Start is called before the first frame update
    private void Start()
    {
        Invoke("StartBattle", timeToStartBattle);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void NextBossState() {
        switch(boss.CurrentBossState.Stage) {
            case BossStateMachine.BossState.BeforeStart: 
                boss.SetCurrentBossState(boss.DuringBattle);
                StopBattle();
                //AttackSimultaneouslyProcess();
                //AttackSeparatelyProcess(0.5f);
                AttackByTrailProcess(0.5f);
                break;
            case BossStateMachine.BossState.DuringBattle:
                StopBattle();
                //AttackSeparatelyProcess(0.5f);
                //InvokeRepeating("LineAttackProcess", 0f, timeBetweenAttacks);
                break;
        }
    }

    private void UpdateStageMode(float id, float ad, int corl) {
        this.indicatorDuration = id;
        this.attackDuration = ad;
        this.countOfRandomLaunches = corl;
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

        if(attackRoutine != null) {
            StopCoroutine(attackRoutine);
        }
    }

    #endregion

    #region 攻擊模式

    public void AttackSeparatelyProcess(float attackTimeSpan) {
        Debug.Log("indicatorDuration: "+indicatorDuration);
        Debug.Log("attackDuration: "+attackDuration);
        Debug.Log("countOfRandomLaunches: "+countOfRandomLaunches);
        // int count = indicatorCtrl.aoePositions.Count + countOfRandomLaunches;
        // if(count > 0) {
        //     attackRoutine = StartCoroutine(SeparatelyAttackCoroutines(attackTimeSpan));
        // }

        attackRoutine = StartCoroutine(AttackCoroutines_Separately(attackTimeSpan));
    }

    public void AttackByTrailProcess(float attackTimeSpan) {
        Debug.Log("indicatorDuration: "+indicatorDuration);
        Debug.Log("attackDuration: "+attackDuration);
        Debug.Log("countOfRandomLaunches: "+countOfRandomLaunches);
        
        attackRoutine = StartCoroutine(AttackCoroutines_ByTrail(attackTimeSpan));
    }

    public void AttackSimultaneouslyProcess() {
        Debug.Log("indicatorDuration: "+indicatorDuration);
        Debug.Log("attackDuration: "+attackDuration);
        Debug.Log("countOfRandomLaunches: "+countOfRandomLaunches);

        int count = indicatorCtrl.aoePositions.Count + countOfRandomLaunches;
        if(count > 0) {
            attackRoutine = StartCoroutine(AttackCoroutines_Simultaneously());
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
            yield return new WaitForSeconds(indicatorDuration + attackDuration - delayTime);
        }
    }

    #endregion
    
    #region 顯示指示器
    
    private IEnumerator DisplayIndicator(Vector3 pos) {
        Debug.Log("AttackCoroutines: DisplayIndicator start");

        indicatorCtrl.InstantiateAreaIndicator(pos, indicatorDuration);
        yield return null;

        Debug.Log("AttackCoroutines: DisplayIndicator end");
    }

    private IEnumerator DisplayIndicators_Simultaneously() {
        Debug.Log("AttackCoroutines: DisplayMutipleIndicatorsSimultaneously start");
        yield return null;

        indicatorCtrl.InstantiateAreaIndicators(indicatorDuration);
        Debug.Log("AttackCoroutines: DisplayMutipleIndicatorsSimultaneously end");

        // TODO 增加隨機AOE指示器至Player附近By countOfRandomLaunches
    }

    #endregion

    #region 控制攻擊的動畫與傷害機制 etc.

    private IEnumerator LaunchAttack(Vector3 indicatorPos) {
        Debug.Log("AttackCoroutines: LaunchAttack start");
        yield return new WaitForSeconds(indicatorDuration - attackDuration);
        //TODO 攻擊
        GameObject projectile = ProjectilePool.instance.GetPooledProjectile();
        if(projectile != null) {
            // projectile.GetComponent<IndirectProjectile>();
            projectile.GetComponent<IndirectProjectile>().SetPositionOfBezierCurve(boss.m_Center, indicatorPos, 12f);
            projectile.GetComponent<IndirectProjectile>().SetDuration(attackDuration);

            projectile.SetActive(true);
        }
        Debug.Log("AttackCoroutines: LaunchAttack end");
    }

    private IEnumerator LaunchAttack_Simultaneously() {
        Debug.Log("AttackCoroutines: LaunchAttackSimultaneously start");
        yield return new WaitForSeconds(indicatorDuration);
        //TODO 攻擊
        Debug.Log("AttackCoroutines: LaunchAttackSimultaneously end");
    }

    #endregion

}
