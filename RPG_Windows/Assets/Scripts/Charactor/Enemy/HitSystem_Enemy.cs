using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem_Enemy : HitSystem
{
    [Header("Enemy基本物件")]
    [SerializeField] protected Enemy<Collider2D> m_target;
    [SerializeField] protected Movement_Enemy m_targetMovement;


    private Animator m_targetAnimator;

    protected virtual void Awake() {
        m_targetAnimator = m_target.Animator;
    }

    protected override IEnumerator TakeHit(Attack attacker) {
        Debug.Log("Enemy TakeHit start");
        
        if(IsIgnoreHit) yield break;

        if(IsInvulnerable)
            yield break;
        else {
            if(invulnerableDuration > 0) {
                invulnerableElapsed = 0f;
                IsInvulnerable = true;
            }
        }

        IsTakingHit = true;
        if(m_target != null) {
            //根據有無Hurt_State決定是否新增
            //m_target.SetCurrentBaseState(m_target.Hurt);

            // foreach (Collider2D hitCollider in attacker.OnHit)
            // {
            //     // 獲取碰撞器的中心位置
            //     Vector2 hitPosition = hitCollider.bounds.center;

            //     // 獲取碰撞的相對位置
            //     Vector2 relativeHitPosition = hitCollider.transform.InverseTransformPoint(attacker.transform.position);

            //     // 在這裡使用碰撞位置進行後續處理
            //     Debug.Log("Hit at position: " + hitPosition);
            //     Debug.Log("Relative hit position: " + relativeHitPosition);

            //     ViewHitEffect(new Vector3(hitPosition.x, hitPosition.y, 99));
            // }

            
            if(!isHyperArmor && m_targetKnockbackFeedback != null) {
                var dir = SetAttackForceDir(attacker.transform);
                m_targetKnockbackFeedback.ActiveFeedbackByDir(dir);
            }

            attacker.DamageSystem.OnDamage(m_targetHealthSystem);
            
            if(damageFlash != null) damageFlash.CallDamageFlasher();

            yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged

            Debug.Log("enemyTakenHit end");
        }
        
        FinishTakeHit();
        Debug.Log("TakeHit end");
    }

    protected override IEnumerator TakeHit(DamageSystem damageSystem, Transform attackedLocation, int damageCounter = 1) {
        Debug.Log("Enemy TakeHit start");

        if(IsIgnoreHit) yield break;

        if(IsInvulnerable)
            yield break;
        else {
            if(invulnerableDuration > 0) {
                invulnerableElapsed = 0f;
                IsInvulnerable = true;
            }
        }
        
        IsTakingHit = true;

        if(m_target != null) {
            //根據有無Hurt_State決定是否新增
            //m_target.SetCurrentBaseState(m_target.Hurt);

            if(!isHyperArmor && m_targetKnockbackFeedback != null) {
                var dir = SetAttackForceDir(attackedLocation);
                m_targetKnockbackFeedback.ActiveFeedbackByDir(dir);
            }

            damageSystem.OnDamage(m_targetHealthSystem);

            if(damageFlash != null) damageFlash.CallDamageFlasher();
            
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

        IsTakingHit = false;
        IsInvulnerable = false;

        if(m_targetMovement != null && m_targetMovement.IsMoving) m_target.SetCurrentBaseState(m_target.Move);
        else m_target.SetCurrentBaseState(m_target.Idle);
        
        Debug.Log("FinishTakeHit");
    }

}
