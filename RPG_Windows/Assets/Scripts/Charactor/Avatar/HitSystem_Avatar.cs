using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitSystem_Avatar : HitSystem
{
    [Header("Avatar基本物件")]
    [SerializeField] protected Avatar m_target;
    [SerializeField] protected Movement_Avatar m_targetMovement;
    [SerializeField] protected Dodge_Avatar m_targetDodge;

    private Animator m_targetAnimator;

    protected virtual void Awake() {
        m_targetAnimator = m_target.Animator;
    }
    

    protected override IEnumerator TakeHit(Attack attacker) 
    {    
        isTakingHit = true;

        var avatarTakenHit = GetComponent<Avatar>();
        if(avatarTakenHit != null) {
            if(m_targetDodge.IsDodging) {
                if(m_target.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Move)) {
                    m_targetMovement.SetMovementAfterTrigger(m_targetMovement.Movement);
                }
                m_target.SetCurrentBaseState(m_target.Dodge);

                yield return new WaitForSeconds(m_targetDodge.DodgeClipTime);
            } else {
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
        }
        
        FinishTakeHit();
    }

    protected override IEnumerator TakeHit(DamageSystem damageSystem, Transform attackedLocation, int damageCounter = 1) 
    {
        isTakingHit = true;

        var avatarTakenHit = GetComponent<Avatar>();
        if(avatarTakenHit != null) {
            if(m_targetDodge.IsDodging) {
                if(m_target.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Move)) {
                    //m_avatar.SetFacingDir(m_avatar.Movement);
                    m_targetMovement.SetMovementAfterTrigger(m_targetMovement.Movement);
                }
                m_target.SetCurrentBaseState(m_target.Dodge);

                yield return new WaitForSeconds(m_targetDodge.DodgeClipTime);
            } else {
                if(isInvulnerable) {
                    yield break;
                }
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
        }

        FinishTakeHit();
    }

    protected override void FinishTakeHit() {
        // Debug.Log("HitSystem_Avatar FinishTakeHit start");
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        isTakingHit = false;
        isInvulnerable = false;
        
        m_targetMovement.SetMovement(m_targetMovement.MovementAfterTrigger);
        m_targetMovement.SetMovementAfterTrigger(Vector3.zero);

        if(m_targetMovement.IsMoving) m_target.SetCurrentBaseState(m_target.Move);
        else m_target.SetCurrentBaseState(m_target.Idle);
        
        // Debug.Log("HitSystem_Avatar FinishTakeHit end");
    }

    private void SetHurtTrigger()
    {
        m_targetAnimator?.SetTrigger("hurt");
    }
}
