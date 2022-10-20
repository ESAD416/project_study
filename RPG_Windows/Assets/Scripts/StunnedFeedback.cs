using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StunnedFeedback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;

    [SerializeField] private float stunRecoveryTime;

    public UnityEvent OnBegin, OnDone;

    public void ActiveFeedback(Vector3 senderPos) {
        StopAllCoroutines();
        OnBegin?.Invoke();
        
        //TODO

        StartCoroutine(EndProcess());
    }

    private IEnumerator EndProcess() {
        yield return new WaitForSeconds(stunRecoveryTime);  // hardcasted casted time for debugged

        OnDone?.Invoke();
    }
}
