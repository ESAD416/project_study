using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaser : MonoBehaviour
{
    [Header("Chaser Parameters")]
    [Range(.1f, 5)]
    public float chaseRadius;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask visibilityLayer;
    public bool targetDetected;
    public bool targetVisable;
    private float detectionCheckDelay = 0.1f;

    private Transform target = null;
    public Transform TargetModel {
        get => target;
        set {
            target = value;
            targetVisable = false;
        }
    }

    [Header("Gizmo Parameters")]
    public Color gizmoColor = Color.red;
    public bool showGizmos = true;

    private void Start() {
        Debug.Log("AIChaser start");
        StartCoroutine(Detection());
    }

    private void Update() {
        if(TargetModel != null) {
            Debug.Log("TargetModel != null)");
            CheckTargetVisible();
        }
    }

    private void OnDrawGizmos() {
        if(showGizmos) {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }

    private void DectectTarget() {
        if(TargetModel == null) {
            CheckIfTargetInRange();
        } else if(TargetModel != null) {
            DetectIfTargetOutOfRange();
        }
    }

    private void CheckIfTargetInRange() {
        Collider2D col = Physics2D.OverlapCircle(transform.position, chaseRadius, targetLayer);
        if(col != null) {
            Debug.Log("Target transform: "+col.transform);
            var charactor = col.GetComponent<Charactor>() as Charactor;
            if(charactor != null) {
                Debug.Log("Target m_Center: "+charactor.m_Center);
                TargetModel = col.transform.Find(charactor.centerObjName).GetComponent<Transform>();
            }
            else {
                TargetModel = col.transform;
            }
        }
    }

    private void DetectIfTargetOutOfRange() {
        if(TargetModel == null || TargetModel.gameObject.activeSelf == false || Vector2.Distance(transform.position, TargetModel.position) > chaseRadius) {
            TargetModel = null;
        }
    }

    private bool CheckTargetVisible() {
        var result = Physics2D.Raycast(transform.position, TargetModel.position - transform.position, chaseRadius, visibilityLayer);
        Debug.DrawRay(transform.position, TargetModel.position - transform.position, gizmoColor);
        if(result.collider != null) {
            Debug.Log("CheckTargetVisible gameObject: "+result.collider.gameObject);
            Debug.Log("CheckTargetVisible layer: "+result.collider.gameObject.layer);
            Debug.Log("targetLayer: "+targetLayer);
            Debug.Log("(1 << result.collider.gameObject.layer): "+(1 << result.collider.gameObject.layer));
            //return (targetLayer & (1 << result.collider.gameObject.layer)) != 0;
        }
        return false;
    }

    private IEnumerator Detection() {
        yield return new WaitForSeconds(detectionCheckDelay);  // hardcasted casted time for debugged
        DectectTarget();
        StartCoroutine(Detection());
    }
}
