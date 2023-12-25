using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_target_rb2d;

    [SerializeField] private Transform m_targetCenter;

    [SerializeField] private float m_thrust = 5;

    [SerializeField] private float m_hitRecoveryTime = 0.2f;
    public float HitRecoveryTime => this.m_hitRecoveryTime;

    public UnityEvent OnBegin, OnDone;

    public void ActiveFeedback(Vector3 senderPos) {
        StopAllCoroutines();
        OnBegin?.Invoke();
        m_target_rb2d.velocity = Vector3.zero;

        Vector3 dir = m_targetCenter.position - senderPos;
        Vector3 force = dir.normalized * m_thrust;
        m_target_rb2d.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(EndProcess());
    }

    public void ActiveFeedbackByDir(Vector3 dir) {
        StopAllCoroutines();
        OnBegin?.Invoke();
        m_target_rb2d.velocity = Vector3.zero;

        Vector3 force = dir.normalized * m_thrust;
        m_target_rb2d.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(EndProcess());
    }

    private IEnumerator EndProcess() {
        yield return new WaitForSeconds(m_hitRecoveryTime);  // hardcasted casted time for debugged
        m_target_rb2d.velocity = Vector3.zero;
        OnDone?.Invoke();
    }
}
