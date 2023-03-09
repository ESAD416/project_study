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
    private Enemy_Abstract boss;

    // Start is called before the first frame update
    void Start()
    {
        colliderTrigger.OnPlayerEnterTrigger += OnPlayerEnterFight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected void OnPlayerEnterFight() {
        StartBattle();
        colliderTrigger.OnPlayerEnterTrigger -= OnPlayerEnterFight;
    }

    private void BossFight_1_OnDead() {
        Debug.Log("Boss Battle Over");
    }

    private void BossFight_1_OnDamaged() {
        // Area took damage
        switch(stage) {
            case Stage.Stage1:
                if(boss.GetCurrHealthNormalized() <= 0.7f) {
                    // if under 70% health
                    StartNextStage();
                }
                break;
            case Stage.Stage2:
                if(boss.GetCurrHealthNormalized() <= 0.5f) {
                    // if under 50% health
                    StartNextStage();
                }
                break;
            case Stage.Stage3:
                if(boss.GetCurrHealthNormalized() <= 0.2f) {
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
                break;
            case Stage.Stage1:
                stage = Stage.Stage2;
                break;
            case Stage.Stage2:
                stage = Stage.Stage3;
                break;
        }
    }

    private void StartBattle() {
        Debug.Log("StartBattle");
    }

    private void StopBattle() {
        Debug.Log("StopBattle");
    }
}
