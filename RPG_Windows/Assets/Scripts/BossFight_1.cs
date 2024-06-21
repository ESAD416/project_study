using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BossFight_1 : MonoBehaviour
{
    public enum Stage {
        WaitingToStart,
        Stage1,
        Stage2,
        Stage3
    }

    [SerializeField] private ColliderTrigger colliderTrigger;
    private Stage stage;
    private Enemy<Collider2D> boss;
    private float timeToStartBattle = 3f;
    private float indicatorDuration = 3f;
    private float projectileDuration = 2.5f;
    private float delayTime = 1f;
    private int countOfLaunches = 2;
    
    [SerializeField] private Player<Collider2D> player;

    [SerializeField] private DynamicOffsets attackOffsets;
    

    [SerializeField] private OffScreenIndicatorCtrl_Slider offScreenIndicatorCtrl;


    private List<int> randomNums = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        colliderTrigger.OnTargetEnterTriggerEvent.AddListener(OnPlayerEnterFight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected void OnPlayerEnterFight() {
        Debug.Log("OnPlayerEnterFight");
        colliderTrigger.OnTargetEnterTriggerEvent.RemoveListener(OnPlayerEnterFight);
        Invoke("StartBattle", timeToStartBattle);
    }

    private void BossFight_1_OnDead() {
        Debug.Log("Boss Battle Over");
    }

    private void BossFight_1_OnDamaged() {
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

    private void StartNextStage() {
        switch(stage) {
            case Stage.WaitingToStart: 
                stage = Stage.Stage1;
                //CancelInvoke("LineAttackProcess");
                LineAttackProcess();
                break;
            case Stage.Stage1:
                stage = Stage.Stage2;
                indicatorDuration = 2f;
                projectileDuration = 4f;
                countOfLaunches = 3;
                //CancelInvoke("LineAttackProcess");
                //InvokeRepeating("LineAttackProcess", 0f, timeBetweenAttacks);
                break;
            case Stage.Stage2:
                stage = Stage.Stage3;
                indicatorDuration = 1.5f;
                projectileDuration = 3f;
                countOfLaunches = 4;
                //CancelInvoke("LineAttackProcess");
                //InvokeRepeating("LineAttackProcess", 0f, timeBetweenAttacks);
                break;
        }
    }

    private void StartBattle() {
        Debug.Log("StartBattle");
        StartNextStage();
    }

    private void LineAttackProcess() {
        // 包含指示器顯示與Projectile發射
        Debug.Log("stage: "+stage);
        Debug.Log("timeBeforeAttacks: "+indicatorDuration);
        Debug.Log("timeBetweenAttacks: "+projectileDuration);
        Debug.Log("countOfLaunches: "+countOfLaunches);
        
        if(attackOffsets.offsets.Count == 0) {
            Debug.Log("No attack points");
            return;
        }
        if(countOfLaunches > 0) {
            randomNums = new List<int>();
            for(int i = 0; i < countOfLaunches; i++) {
                int randomNum = UnityEngine.Random.Range(0, attackOffsets.offsets.Count);
                if(randomNums.Contains(randomNum)) {
                    i--;
                    continue;
                } else {
                    randomNums.Add(randomNum);
                }
            }

            StartCoroutine(LineAttackCoroutines());
        }

    }

    private IEnumerator LineAttackCoroutines() {
        while(true) {
            // 1. 動態生成攻擊點
            StartCoroutine(GenerateAttackOffsets());
            // 2. 指示器
            StartCoroutine(DisplayAttackIndicators());
            // 3. 產生Projectile
            StartCoroutine(LaunchProjectile());

            yield return new WaitForSeconds(indicatorDuration + projectileDuration - delayTime);
        }
    }

    private IEnumerator GenerateAttackOffsets() {
        Debug.Log("LineAttackCoroutines: GenerateAttackOffsets start");
        attackOffsets.UpdateCombination();

        yield return null;
        Debug.Log("LineAttackCoroutines: GenerateAttackOffsets end");
    }

    private IEnumerator DisplayAttackIndicators() {
        Debug.Log("LineAttackCoroutines: DisplayAttackIndicators start");
        yield return null;
        foreach(int randomNum in randomNums) {
            var attackOffset = attackOffsets.offsets[randomNum];

            offScreenIndicatorCtrl.targets.Add(new IndicatorModel() {
                target = attackOffset,
                referenceAxis = Vector3.right,
            });
        }
        offScreenIndicatorCtrl.InstantiateIndicators(indicatorDuration);
        Debug.Log("LineAttackCoroutines: DisplayAttackIndicators end");
    }

    private IEnumerator LaunchProjectile() {
        Debug.Log("LineAttackCoroutines: LaunchProjectile start");
        yield return new WaitForSeconds(indicatorDuration);
        foreach(int randomNum in randomNums) {
            var attackOffset = attackOffsets.offsets[randomNum];
            Vector3 dir = player.Center - attackOffset.position;

            //Debug.Log("LunchProjectile player.m_Center: "+player.m_Center);
            //Debug.Log("LunchProjectile attackOffset: "+attackOffset.position);
            //Debug.Log("LunchProjectile dir: "+dir);
            GameObject projectile = ProjectilePool.instance.GetPooledGameObject();
            if(projectile != null) {
                projectile.GetComponent<Projectile_FireBall>().SetDirection(dir.normalized);
                
                //Debug.Log("LunchProjectile referenceAxis: "+projectile.GetComponent<Projectile>().referenceAxis);
                // var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                // var quaternion =Quaternion.Euler(0, 0, angle);
                var angle = Vector3.Angle(dir, projectile.GetComponent<Projectile_FireBall>().referenceAxis);
                Debug.Log("LunchProjectile angle: "+angle);
                var quaternion = dir.x > 0 ? Quaternion.Euler(0, 0, angle) : Quaternion.Euler(0, 0, -angle);

                projectile.transform.position = attackOffset.position;
                projectile.transform.rotation = quaternion;
                // projectile.transform.rotation = Quaternion.LookRotation(dir.normalized, projectile.GetComponent<Projectile>().referenceAxis);;
                projectile.SetActive(true);
            }
        }
        Debug.Log("LineAttackCoroutines: LaunchProjectile end");
    }

    private void StopBattle() {
        Debug.Log("StopBattle");
    }
}
