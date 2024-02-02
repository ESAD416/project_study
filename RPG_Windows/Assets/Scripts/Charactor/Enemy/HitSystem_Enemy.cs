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
        Debug.Log("TakeHit start");
        isTakingHit = true;

        var enemyTakenHit = GetComponent<Enemy>();
        if(enemyTakenHit != null) {
            Debug.Log("enemyTakenHit start");
            if(isInvulnerable)
                yield break;
            else if(!isInvulnerable) isInvulnerable = true;

            m_target.SetCurrentBaseState(m_target.Hurt);

            foreach (Collider2D hitCollider in attacker.OnHit)
            {
                // 獲取碰撞器的中心位置
                Vector2 hitPosition = hitCollider.bounds.center;

                // 獲取碰撞的相對位置
                Vector2 relativeHitPosition = hitCollider.transform.InverseTransformPoint(attacker.transform.position);

                // 在這裡使用碰撞位置進行後續處理
                Debug.Log("Hit at position: " + hitPosition);
                Debug.Log("Relative hit position: " + relativeHitPosition);

                ViewHitEffect(new Vector3(hitPosition.x, hitPosition.y, 99));
            }

            
            var dir = SetAttackForceDir(attacker.transform);
            if(!isHyperArmor && m_KnockbackFeedback != null) {
                m_KnockbackFeedback.ActiveFeedbackByDir(dir);
                Invoke("SetHurtTrigger", m_KnockbackFeedback.HitRecoveryTime);
            } else {
                m_targetAnimator?.SetTrigger("hurt");
            }

            attacker.DamageSystem.OnDamage(m_HealthSystem);
            
            yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

            Debug.Log("enemyTakenHit end");
        }
        
        FinishTakeHit();
        Debug.Log("TakeHit end");
    }

    protected override IEnumerator TakeHit(DamageSystem damageSystem, Transform attackedLocation, int damageCounter = 1) {
        
        isTakingHit = true;

        var enemyTakenHit = GetComponent<Enemy>();
        if(enemyTakenHit != null) {
            if(isInvulnerable)
                yield break;
            else if(!isInvulnerable) isInvulnerable = true;

            m_target.SetCurrentBaseState(m_target.Hurt);

            var dir = SetAttackForceDir(attackedLocation);

            if(!isHyperArmor && m_KnockbackFeedback != null) {
                m_KnockbackFeedback.ActiveFeedbackByDir(dir);
                Invoke("SetHurtTrigger", m_KnockbackFeedback.HitRecoveryTime);
            } else {
                m_targetAnimator?.SetTrigger("hurt");
            }

            damageSystem.OnDamage(m_HealthSystem);
            
            yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

        }
        
        FinishTakeHit();
    }

    protected Vector3 SetAttackForceDir(Transform attackedLocation) {
        var dir = Vector3.zero;

        if(attackedLocation.position.x > m_target.transform.position.x) dir = Vector3.left;
        else if(attackedLocation.position.x < m_target.transform.position.x) dir = Vector3.right;
        else if(attackedLocation.position.x == m_target.transform.position.x) {
            if(m_targetMovement.FacingDir.x > 0 ) dir = Vector3.left;
            else if(m_targetMovement.FacingDir.x < 0) dir = Vector3.right;
        }

        return dir;
    }

    protected override void FinishTakeHit() {
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        isTakingHit = false;
        isInvulnerable = false;

        if(m_targetMovement != null && m_targetMovement.isMoving) m_target.SetCurrentBaseState(m_target.Move);
        else m_target.SetCurrentBaseState(m_target.Idle);
        
        Debug.Log("FinishTakeHit");
    }

    private void SetHurtTrigger()
    {
        m_targetAnimator?.SetTrigger("hurt");
    }
}
