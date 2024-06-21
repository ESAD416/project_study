using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Detector_EnemyChase : Detector_Overlap2D
{
    public LayerMask VisibilityLayer;
    [SerializeField] protected Enemy<Collider2D> m_enemy;
    [SerializeField] protected Movement_Enemy m_enemyMovement;

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

    public UnityEvent OnTargetDected;

    private void OnEnable() {
        m_enemy.isPatroling = false;
        m_enemy.isChasing = true;
        m_enemy.SetCurrentEnemyState(m_enemy.Chase);

        StartCoroutine(Detection());
    }

    private void Start() {
        
    }

    protected override void Update() {
        base.Update();
        if(m_enemy.CurrentBaseState.Equals(Constant.CharactorState.Dead)) 
        {
            transform.gameObject.SetActive(false);
        } 
    }

    protected void FixedUpdate() {
        if(!m_enemy.CurrentBaseState.Equals(Constant.CharactorState.Attack))
        {
            if(TargetModel != null)
            {
                // Debug.Log("TargetModel != null");
                targetVisable = CheckTargetVisible();
                if(targetVisable) {
                    //Debug.Log("targetVisable: "+(TargetModel.position - transform.position));
                    m_enemyMovement.SetMovement(TargetModel.position - transform.position);
                } else {
                    StartCoroutine(ChaseModeEndingProcess());
                }
            } else {
                // Debug.Log("TargetModel == null");
                StartCoroutine(ChaseModeEndingProcess());
            }
            
        }
    }

    private void OnDisable() {
        m_enemy.isPatroling = true;
        m_enemy.isChasing = false;
        m_enemy.SetCurrentEnemyState(m_enemy.Patrol);
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
                OnTargetDected?.Invoke();
                var col2d = OverlapDetected[0];

                var charactor = col2d.GetComponent<Charactor<Collider2D>>() as Charactor<Collider2D>;
                if(charactor != null) {
                    Debug.Log("Target m_CenterObj: "+charactor.CenterObj);
                    TargetModel = charactor.CenterObj;
                }
                else {
                    TargetModel = col2d.transform;
                }
            } 
        } else if(TargetModel != null) {
            if(TargetModel == null || TargetModel.gameObject.activeSelf == false || Vector2.Distance(transform.position, TargetModel.position) > Radius) {
                TargetModel = null;
            }
        }
    }

    private bool CheckTargetVisible() {
        var result = Physics2D.Raycast(transform.position, TargetModel.position - transform.position, Radius, VisibilityLayer);
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
            m_enemy.isPatroling = true;
            m_enemy.isChasing = false;
            targetVisable = false;
            m_enemyMovement.SetDefaultMovement();
            transform.gameObject.SetActive(false);
        }
    }

}
