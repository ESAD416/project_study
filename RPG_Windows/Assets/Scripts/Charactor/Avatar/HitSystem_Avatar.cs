using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.Events;

public class HitSystem_Avatar : HitSystem
{
    [Header("Avatar基本物件")]
    [SerializeField] protected Avatar m_target;
    [SerializeField] protected Movement_Avatar m_targetMovement;
    [SerializeField] protected Combat_Avatar m_targetCombat;
    [SerializeField] protected Dodge_Avatar m_targetDodge;

    private Animator m_targetAnimator;
    

    protected virtual void Awake() {
        m_targetAnimator = m_target.Animator;
    }

    protected void Start() {
    }
    

    protected override IEnumerator TakeHit(Attack attacker) 
    {    
        isTakingHit = true;

        var avatarTakenHit = GetComponent<Avatar>();
        if(avatarTakenHit != null) {
            if(m_target.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Move)) {
                Debug.Log("TakeHit: SetMovementAfterTrigger "+m_targetMovement.Movement);
                m_targetMovement.SetMovementAfterTrigger(m_targetMovement.Movement);
            }

            bool dodged = DodgedTheHit();
            if(dodged) {
                m_target.SetCurrentBaseState(m_target.Dodge);

                yield return new WaitForSeconds(m_targetDodge.DodgeClipTime);
            } else {
                if(isInvulnerable)
                    yield break;
                else if(!isInvulnerable) isInvulnerable = true;
                
                
                m_target.SetCurrentBaseState(m_target.Hurt);

                var dir = SetAttackForceDir(attacker.transform);
                if(!isHyperArmor && m_KnockbackFeedback != null) {
                    m_KnockbackFeedback.ActiveFeedbackByDir(dir);
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
            if(m_target.CurrentBaseState.State.Equals(BaseStateMachine_Charactor.BaseState.Move)) {
                //m_avatar.SetFacingDir(m_avatar.Movement);
                m_targetMovement.SetMovementAfterTrigger(m_targetMovement.Movement);
            }

            bool dodged = DodgedTheHit();
            if(dodged) {
                m_target.SetCurrentBaseState(m_target.Dodge);

                yield return new WaitForSeconds(m_targetDodge.DodgeClipTime);
            } else {
                if(isInvulnerable) {
                    yield break;
                }
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
        Debug.Log("HitSystem_Avatar FinishTakeHit start");
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        isTakingHit = false;
        isInvulnerable = false;
        
        m_targetMovement.SetMovement(m_targetMovement.MovementAfterTrigger);
        m_targetMovement.SetMovementAfterTrigger(Vector3.zero);

        if(m_targetMovement.IsMoving) m_target.SetCurrentBaseState(m_target.Move);
        else m_target.SetCurrentBaseState(m_target.Idle);
        
        Debug.Log("HitSystem_Avatar FinishTakeHit end");
    }

    private bool DodgedTheHit() 
    {
        bool result = true;
        if(m_targetDodge.IsDodging) {
            if(m_targetCombat.IsPreAttacking || m_targetCombat.IsPostAttacking) {
                // 攻擊前後搖
                result = false;
            }
        } else {
            result = false;
        }

        return result;
    }

    private void SetHurtTrigger()
    {
        m_targetAnimator?.SetTrigger("hurt");
    }
}
