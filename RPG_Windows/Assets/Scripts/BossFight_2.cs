using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight_2 : MonoBehaviour
{
    public enum Stage {
        WaitingToStart,
        Stage1,
        Stage2,
        Stage3
    }

    private Stage stage;
    private float timeToStartBattle = 3f;
    private float indicatorDuration = 3f;
    private float attackDuration = 0.65f;
    private float delayTime = 1f;
    private int countOfRandomLaunches = 2;
    private Coroutine attackRoutine;

    [SerializeField] private Enemy_Abstract boss;
    [SerializeField] private Player player;

    [SerializeField] private AOEIndicatorCtrl indicatorCtrl;
    


    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartBattle", timeToStartBattle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BossFight_1_OnDead() {
        Debug.Log("Boss Battle Over");
    }

    private void BossFight_1_OnDamaged() {
        // Area took damage
        switch(stage) {
            case Stage.Stage1:
                if(boss.HealthSystem.GetCurrHealthNormalized() <= 0.7f) {
                    // if under 70% health
                    StartNextStage();
                }
                break;
            case Stage.Stage2:
                if(boss.HealthSystem.GetCurrHealthNormalized() <= 0.5f) {
                    // if under 50% health
                    StartNextStage();
                }
                break;
            case Stage.Stage3:
                if(boss.HealthSystem.GetCurrHealthNormalized() <= 0.2f) {
                    // if under 20% health
                    StartNextStage();
                }
                break;
        }
    }

    private void StartNextStage() {
        switch(stage) {
            case Stage.WaitingToStart: 
                stage = Stage.Stage1;
                StopBattle();
                //AttackSimultaneouslyProcess();
                //AttackSeparatelyProcess(0.5f);
                AttackByTrailProcess(0.5f);
                break;
            case Stage.Stage1:
                stage = Stage.Stage2;
                indicatorDuration = 2f;
                attackDuration = 0.6f;
                countOfRandomLaunches = 3;
                StopBattle();
                AttackSeparatelyProcess(0.5f);
                //InvokeRepeating("LineAttackProcess", 0f, timeBetweenAttacks);
                break;
            case Stage.Stage2:
                stage = Stage.Stage3;
                indicatorDuration = 1.5f;
                attackDuration = 0.5f;
                countOfRandomLaunches = 4;
                //CancelInvoke("LineAttackProcess");
                //InvokeRepeating("LineAttackProcess", 0f, timeBetweenAttacks);
                break;
        }
    }

    private void StartBattle() {
        Debug.Log("StartBattle");
        StartNextStage();
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

    #region 攻擊模式

    private void AttackSeparatelyProcess(float attackTimeSpan) {
        Debug.Log("stage: "+stage);
        Debug.Log("indicatorDuration: "+indicatorDuration);
        Debug.Log("attackDuration: "+attackDuration);
        Debug.Log("countOfRandomLaunches: "+countOfRandomLaunches);
        // int count = indicatorCtrl.aoePositions.Count + countOfRandomLaunches;
        // if(count > 0) {
        //     attackRoutine = StartCoroutine(SeparatelyAttackCoroutines(attackTimeSpan));
        // }

        attackRoutine = StartCoroutine(SeparatelyAttackCoroutines(attackTimeSpan));
    }

    private void AttackByTrailProcess(float attackTimeSpan) {
        Debug.Log("stage: "+stage);
        Debug.Log("indicatorDuration: "+indicatorDuration);
        Debug.Log("attackDuration: "+attackDuration);
        Debug.Log("countOfRandomLaunches: "+countOfRandomLaunches);
        
        attackRoutine = StartCoroutine(AttackByTrailCoroutines(attackTimeSpan));
    }

    private void AttackSimultaneouslyProcess() {
        Debug.Log("stage: "+stage);
        Debug.Log("indicatorDuration: "+indicatorDuration);
        Debug.Log("attackDuration: "+attackDuration);
        Debug.Log("countOfRandomLaunches: "+countOfRandomLaunches);

        int count = indicatorCtrl.aoePositions.Count + countOfRandomLaunches;
        if(count > 0) {
            attackRoutine = StartCoroutine(SimultaneouslyAttackCoroutines());
        }
    }

    #endregion

    #region 攻擊模式的協程

    private IEnumerator SeparatelyAttackCoroutines(float attackTimeSpan) {
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

    private IEnumerator AttackByTrailCoroutines(float attackTimeSpan) {
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

    private IEnumerator SimultaneouslyAttackCoroutines() {
        // 所有的aoe攻擊*count是同時發動的

        while(true) {
            // 1. 動態生成攻擊點
            indicatorCtrl.UpdateCombination();
            // 2. 同時顯示指示器
            StartCoroutine(DisplayMutipleIndicatorSimultaneously());
            // 3. 同時控制攻擊的動畫與傷害機制 etc.
            StartCoroutine(LaunchAttackSimultaneously());

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

    private IEnumerator DisplayMutipleIndicatorSimultaneously() {
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

    private IEnumerator LaunchAttackSimultaneously() {
        Debug.Log("AttackCoroutines: LaunchAttackSimultaneously start");
        yield return new WaitForSeconds(indicatorDuration);
        //TODO 攻擊
        Debug.Log("AttackCoroutines: LaunchAttackSimultaneously end");
    }

    #endregion

}
