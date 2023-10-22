using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StunnedFeedback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;

    private bool isStunned = false;
    private int armorToStunned = 3;
    [SerializeField] private float stunRecoveryTime = 1.5f;

    public UnityEvent OnBegin, OnDone;

    public void ActiveFeedback() {
        StopAllCoroutines();
        OnBegin?.Invoke();
        
        //TODO

        StartCoroutine(BeingStunned());
    }

    protected IEnumerator BeingStunned() {
        Debug.Log("BeingStunned");
        //SetMoveSpeed(0f);
        //SetAnimation stunned
        yield return new WaitForSeconds(stunRecoveryTime);  // hardcasted casted time for debugged
        FinishBeingStunned();
    }

    public void FinishBeingStunned() {
        isStunned = false;
        armorToStunned = 3;
        
        Debug.Log("FinishBeingStunned");
    }

}
