using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;

    [SerializeField] private float thrust = 16;

    [SerializeField] private float hitRecoveryTime = 0.2f;

    public UnityEvent OnBegin, OnDone;

    public void ActiveFeedback(GameObject sender) {
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector3 dir = transform.position - sender.transform.position;
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
