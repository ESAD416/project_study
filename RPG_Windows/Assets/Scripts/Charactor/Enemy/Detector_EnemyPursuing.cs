using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System;

public class Detector_EnemyPursuing : Detector_Overlap2D
{
    [Header("Detector_EnemyPursuing 基本物件")]
    [SerializeField] protected MonoBehaviour m_enemy;
    protected IEnemy iEnemy;

    public bool targetVisable;
    private float detectionCheckDelay = 0.1f;
    public float chaseModeDelay = 1.0f;

    [SerializeField] private Transform target = null;
    public Transform TargetModel {
        get => target;
        set {
            target = value;
            targetVisable = false;
        }
    }

    public UnityEvent OnTargetDetectEnter, OnTargetDetectExit;

    protected void Awake() {
        iEnemy = m_enemy as IEnemy;
    }

    private void OnEnable() {
        
        // iEnemy.SetIsPatroling(false);
        // iEnemy.SetIsPursuing(true);
        //m_enemy.SetCurrentEnemyState(iEnemy.Pursuing);
        StartCoroutine(Detection());
    }
    private void OnDisable() {
        // iEnemy.SetIsPatroling(true);
        // iEnemy.SetIsPursuing(false);
        //m_enemy.SetCurrentEnemyState(m_enemy.Patrol);
        StopCoroutine(Detection());
    }

    private void Start() {
        // Observable.Timer(TimeSpan.FromSeconds(detectionCheckDelay))
        //     .Subscribe(_ => {
                
        //     })
        //     .AddTo(this);
    }

    protected override void Update() {
        base.Update();
    }

    protected void FixedUpdate() {
        if(iEnemy.CurrentBaseStateName.Equals(Constant.CharactorState.Dead)) return;

        if(TargetModel != null) 
        {
            targetVisable = CheckTargetVisible();
            if(targetVisable) {
                Debug.Log("FixedUpdate targetVisable true");
                OnTargetDetectEnter?.Invoke();

                // Set To Movement_Pursuing
                if(!iEnemy.CurrentEnemyStateName.Equals(Constant.EnemyState.Pursuing)) iEnemy.SetPursuingState();

            } else {
                // Ending pursuing and Set To Default Movement
                if(!iEnemy.CurrentEnemyStateName.Equals(Constant.EnemyState.Patrol)) iEnemy.SetPatrolingState();

                OnTargetDetectExit?.Invoke();
            }
        } else {
            // Ending pursuing and Set To Default Movement
            if(!iEnemy.CurrentEnemyStateName.Equals(Constant.EnemyState.Patrol)) iEnemy.SetPatrolingState();
        }

        // if(!iEnemy.CurrentBaseStateName.Equals(Constant.CharactorState.Attack))
        // {
        //     if(TargetModel != null)
        //     {
        //         // Debug.Log("TargetModel != null");
        //         targetVisable = CheckTargetVisible();
        //         if(targetVisable) {
        //             //Debug.Log("targetVisable: "+(TargetModel.position - transform.position));
        //             m_enemyMovement.SetMovement(TargetModel.position - transform.position);
        //         } else {
        //             StartCoroutine(ChaseModeEndingProcess());
        //         }
        //     } else {
        //         // Debug.Log("TargetModel == null");
        //         StartCoroutine(ChaseModeEndingProcess());
        //     }
            
        // }
    }

    private void OnDrawGizmos() {
        Gizmos.color = HandlesColor;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    private IEnumerator Detection() {
        yield return new WaitForSeconds(detectionCheckDelay);  // hardcasted casted time for debugged
        DectectTarget();
        StartCoroutine(Detection());
    }

    private void DectectTarget() {
        if(TargetModel == null) {
            if(OverlapDetected != null && OverlapDetected.Length > 0) {
                var col2d = OverlapDetected[0];

                var charactor = col2d.GetComponentInParent<ICharactor>();
                if(charactor != null) {
                    Debug.Log("Target m_CenterObj: "+charactor.CenterObj);
                    TargetModel = charactor.CenterObj;
                }
                else {
                    TargetModel = col2d.transform;
                }
            } 
        } else {
            if(TargetModel == null || TargetModel.gameObject.activeSelf == false || Vector2.Distance(transform.position, TargetModel.position) > Radius) {
                TargetModel = null;
            }
        }
    }

    private bool CheckTargetVisible() {
        var result = Physics2D.Raycast(transform.position, TargetModel.position - transform.position, Radius, TargetLayer);
        Debug.DrawRay(transform.position, TargetModel.position - transform.position, HandlesColor);
        if(result.collider != null) {
            //Debug.Log("CheckTargetVisible gameObject: "+result.collider.gameObject);
            //Debug.Log("CheckTargetVisible layer: "+result.collider.gameObject.layer);
            // Debug.Log("targetLayer: "+TargetLayer);

            var binaryTgtLayer = 1 << result.collider.gameObject.layer;
            //Debug.Log("binaryTgtLayer: "+binaryTgtLayer);

            // targetLayer AND binaryTgtLayer will be zero if there are different
            //Debug.Log("CheckTargetVisible: "+((TargetLayer & binaryTgtLayer) != 0));
            return (TargetLayer & binaryTgtLayer) != 0;
        }
        return false;
    }

    private IEnumerator ChaseModeEndingProcess() {

        yield return new WaitForSeconds(chaseModeDelay);  // hardcasted casted time for debugged
        if(TargetModel == null) {
            iEnemy.SetPatrolingState();
            targetVisable = false;
            //m_enemyMovement.SetDefaultMovement();
            transform.gameObject.SetActive(false);
        }
    }

}
