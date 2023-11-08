using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem_Enemy : HitSystem
{
    [Header("Enemy基本物件")]
    [SerializeField] protected Enemy m_target;
    [SerializeField] protected Movement_Enemy m_targetMovement;


    private Animator m_targetAnimator;

    protected virtual void Awake() {
        m_targetAnimator = m_target.Animator;
    }

    protected override IEnumerator TakeHit(Attack attacker) {
        
        isTakingHit = true;

        var enemyTakenHit = GetComponent<Enemy>();
        if(enemyTakenHit != null) {
            if(isInvulnerable)
                yield break;
            else if(!isInvulnerable) isInvulnerable = true;
            
            m_target.SetCurrentBaseState(m_target.Hurt);

            if(!isHyperArmor && m_KnockbackFeedback != null) {
                m_KnockbackFeedback.ActiveFeedback(attacker.transform.position);
                Invoke("SetHurtTrigger", m_KnockbackFeedback.HitRecoveryTime);
            } else {
                m_targetAnimator?.SetTrigger("hurt");
            }

            attacker.DamageSystem.OnDamage(m_HealthSystem);
            
            yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

        }
        
        FinishTakeHit();
    }

    protected override IEnumerator TakeHit(DamageSystem damageSystem, Transform attackedLocation, int damageCounter = 1) {
        
        isTakingHit = true;

        var enemyTakenHit = GetComponent<Enemy>();
        if(enemyTakenHit != null) {
            if(isInvulnerable)
                yield break;
            else if(!isInvulnerable) isInvulnerable = true;

            m_target.SetCurrentBaseState(m_target.Hurt);

            if(!isHyperArmor && m_KnockbackFeedback != null) {
                m_KnockbackFeedback.ActiveFeedback(attackedLocation.position);
                Invoke("SetHurtTrigger", m_KnockbackFeedback.HitRecoveryTime);
            } else {
                m_targetAnimator?.SetTrigger("hurt");
            }

            damageSystem.OnDamage(m_HealthSystem);
            
            yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

        }
        
        FinishTakeHit();
    }

    protected override void FinishTakeHit() {
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        isTakingHit = false;
        isInvulnerable = false;

        if(m_targetMovement.isMoving) m_target.SetCurrentBaseState(m_target.Move);
        else m_target.SetCurrentBaseState(m_target.Idle);
        
        Debug.Log("FinishTakeHit");
    }

    private void SetHurtTrigger()
    {
        m_targetAnimator?.SetTrigger("hurt");
    }
}