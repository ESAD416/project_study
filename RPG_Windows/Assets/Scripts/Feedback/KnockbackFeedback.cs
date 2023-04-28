using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;

    [SerializeField] private Transform objectCenter;

    [SerializeField] private float thrust = 5;

    [SerializeField] private float hitRecoveryTime = 0.2f;

    public UnityEvent OnBegin, OnDone;

    public void ActiveFeedback(Vector3 senderPos) {
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector3 dir = objectCenter.position - senderPos;
        Vector3 force = dir.normalized * thrust;
        rb2d.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(EndProcess());
    }

    public void ActiveFeedbackByDir(Vector3 dir) {
        Debug.Log("ActiveFeedbackByDir dir"+dir);
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector3 force = dir.normalized * thrust;
        rb2d.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(EndProcess());
    }

    private IEnumerator EndProcess() {
        yield return new WaitForSeconds(hitRecoveryTime);  // hardcasted casted time for debugged
        rb2d.velocity = Vector3.zero;
        OnDone?.Invoke();
    }
}
