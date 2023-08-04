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
    private Enemy_Abstract boss;
    private float indicatorDuration = 3f;
    private float attackDuration = 3f;
    private int countOfRandomLaunches = 2;

    [SerializeField] private Player player;

    [SerializeField] private DynamicAOE aoeOffsets;

    // Start is called before the first frame update
    void Start()
    {
        
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
                //CancelInvoke("LineAttackProcess");
                AttackProcess();
                break;
            case Stage.Stage1:
                stage = Stage.Stage2;
                indicatorDuration = 2f;
                attackDuration = 4f;
                countOfRandomLaunches = 3;
                //CancelInvoke("LineAttackProcess");
                //InvokeRepeating("LineAttackProcess", 0f, timeBetweenAttacks);
                break;
            case Stage.Stage2:
                stage = Stage.Stage3;
                indicatorDuration = 1.5f;
                attackDuration = 3f;
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

    private void AttackProcess() {
        Debug.Log("stage: "+stage);
        Debug.Log("timeBeforeAttacks: "+indicatorDuration);
        Debug.Log("timeBetweenAttacks: "+attackDuration);
        Debug.Log("countOfLaunches: "+countOfRandomLaunches);

        
    }
}
