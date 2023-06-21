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
    private float timeToStartBattle = 3f;
    private float timeBeforeAttacks = 3f;
    private float timeBetweenAttacks = 5f;
    private int countOfLaunches = 2;
    [SerializeField] private Player player;
    [SerializeField] private Projectile_FireBall fireProjectile;
    [SerializeField] private List<Transform> attackPointOffsets;

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
                Debug.Log("stage: "+stage);
                Debug.Log("timeBeforeAttacks: "+timeBeforeAttacks);
                Debug.Log("timeBetweenAttacks: "+timeBetweenAttacks);
                Debug.Log("countOfLaunches: "+countOfLaunches);
                InvokeRepeating("LaunchProjectile", 0f, timeBeforeAttacks);
                break;
            case Stage.Stage1:
                stage = Stage.Stage2;
                timeBeforeAttacks = 2f;
                timeBetweenAttacks = 4f;
                countOfLaunches = 3;
                Debug.Log("stage: "+stage);
                Debug.Log("timeBeforeAttacks: "+timeBeforeAttacks);
                Debug.Log("timeBetweenAttacks: "+timeBetweenAttacks);
                Debug.Log("countOfLaunches: "+countOfLaunches);
                InvokeRepeating("LaunchProjectile", 0f, timeBeforeAttacks);
                break;
            case Stage.Stage2:
                stage = Stage.Stage3;
                timeBeforeAttacks = 1f;
                timeBetweenAttacks = 3f;
                countOfLaunches = 4;
                Debug.Log("stage: "+stage);
                Debug.Log("timeBeforeAttacks: "+timeBeforeAttacks);
                Debug.Log("timeBetweenAttacks: "+timeBetweenAttacks);
                Debug.Log("countOfLaunches: "+countOfLaunches);
                InvokeRepeating("LaunchProjectile", 0f, timeBeforeAttacks);
                break;
        }
    }

    private void StartBattle() {
        Debug.Log("StartBattle");
        StartNextStage();
    }

    private void LaunchProjectile() {
        if(attackPointOffsets.Count == 0) {
            Debug.Log("No attack points");
            return;
        }

        if(countOfLaunches > 0) {
            List<int> randomNums = new List<int>();
            for(int i = 0; i < countOfLaunches; i++) {
                int randomNum = UnityEngine.Random.Range(0, attackPointOffsets.Count);
                if(randomNums.Contains(randomNum)) {
                    i--;
                    continue;
                } else {
                    randomNums.Add(randomNum);
                }
            }

            foreach(int randomNum in randomNums) {
                var attackOffset = attackPointOffsets[randomNum];
                Vector3 dir = player.m_Center - attackOffset.position;

                //Debug.Log("LunchProjectile player.m_Center: "+player.m_Center);
                //Debug.Log("LunchProjectile attackOffset: "+attackOffset.position);
                //Debug.Log("LunchProjectile dir: "+dir);
                GameObject projectile = ProjectilePool.instance.GetPooledProjectile();
                if(projectile != null) {
                    projectile.GetComponent<Projectile>().SetDirection(dir.normalized);
                    
                    var angle = Vector3.Angle(dir, projectile.GetComponent<Projectile>().spritePointDirection);
                    //Debug.Log("LunchProjectile angle: "+angle);
                    var quaternion = dir.x > 0 ? Quaternion.Euler(0, 0, angle) : Quaternion.Euler(0, 0, -angle);

                    projectile.transform.position = attackOffset.position;
                    projectile.transform.rotation = quaternion;
                    projectile.SetActive(true);
                }
            }
        }
    }

    private void StopBattle() {
        Debug.Log("StopBattle");
    }
}
