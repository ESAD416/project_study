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

    // Start is called before the first frame update
    void Start()
    {
        colliderTrigger.OnPlayerEnterTrigger += OnPlayerEnterFight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected void OnPlayerEnterFight(object sender, EventArgs e) {
        StartBattle();
        colliderTrigger.OnPlayerEnterTrigger -= OnPlayerEnterFight;
    }

    private void BossFight_1_OnDead(object sender, EventArgs e) {
        Debug.Log("Boss Battle Over");
    }

    private void BossFight_1_OnDamaged(object sender, EventArgs e) {
        // Area took damage
        switch(stage) {
            default:
            case Stage.Stage1:
                // if under 70% health
                break;
        }
    }

    private void StartNextStage() {
        switch(stage) {
            default:
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
}
