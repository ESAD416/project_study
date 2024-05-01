using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem_Lamniat : HitSystem_Avatar
{
    [Header("HitSystem_Lamniat 基本物件")]
    [SerializeField] protected Movement_Lamniat m_targetMovement;
    [SerializeField] protected Combat_Lamniat m_targetCombat;
    [SerializeField] protected Dodge_Lamniat m_targetDodge;

    protected Vector2 m_defaultHitBoxOffset = new Vector2(0, 1.8f);
    protected Vector2 m_defaultHitBoxSize = new Vector2(1.25f, 2.75f);
    protected Vector2 m_crouchHitBoxOffset = new Vector2(0, 1.36f);
    protected Vector2 m_crouchHitBoxSize = new Vector2(1.25f, 1.86f);

    protected override void Start() {
        base.Start();
        m_targetHitBoxCollider.offset = m_defaultHitBoxOffset;
        m_targetHitBoxCollider.size = m_defaultHitBoxSize;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(m_targetCombat.IsShooting) {
            m_targetHitBoxCollider.offset = m_crouchHitBoxOffset;
            m_targetHitBoxCollider.size = m_crouchHitBoxSize;
        }
        else {
            m_targetHitBoxCollider.offset = m_defaultHitBoxOffset;
            m_targetHitBoxCollider.size = m_defaultHitBoxSize;
        }
    }

    protected override IEnumerator TakeHit(Attack attacker) 
    {    
        IsTakingHit = true;
        m_targetMovement.CanMove = false;

        if(m_target != null) {
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
                if(!isHyperArmor && m_targetKnockbackFeedback != null) {
                    m_targetKnockbackFeedback.ActiveFeedbackByDir(dir);
                    Invoke("SetHurtTrigger", m_targetKnockbackFeedback.HitRecoveryTime);
                } else {
                    SetHurtTrigger();
                }

                attacker.DamageSystem.OnDamage(m_targetHealthSystem);
                
                yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged
            }
        }
        
        FinishTakeHit();
    }

    protected override IEnumerator TakeHit(DamageSystem damageSystem, Transform attackedLocation, int damageCounter = 1) 
    {
        Debug.Log("TakeHit start");
        IsTakingHit = true;
        m_targetMovement.CanMove = false;

        if(m_target != null) {
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
                if(!isHyperArmor && m_targetKnockbackFeedback != null) {
                    m_targetKnockbackFeedback.ActiveFeedbackByDir(dir);
                    Invoke("SetHurtTrigger", m_targetKnockbackFeedback.HitRecoveryTime);
                } else {
                    SetHurtTrigger();
                }

                damageSystem.OnDamage(m_targetHealthSystem);
                
                yield return new WaitForSeconds(invulnerableDuration);  // hardcasted casted time for debugged
            }
        }
        Debug.Log("TakeHit end");

        FinishTakeHit();
    }

    protected override void FinishTakeHit() {
        Debug.Log("HitSystem_Avatar FinishTakeHit start");
        if(takeHitRoutine != null) {
            StopCoroutine(takeHitRoutine);
        }

        IsTakingHit = false;
        isInvulnerable = false;
        m_targetMovement.CanMove = true;
        
        //m_targetMovement.SetMovement(m_targetMovement.MovementAfterTrigger);
        m_targetMovement.SetMovementAfterTrigger(Vector3.zero);

        if(m_targetMovement.IsMoving) m_target.SetCurrentBaseState(m_target.Move);
        else m_target.SetCurrentBaseState(m_target.Idle);
        
        Debug.Log("HitSystem_Avatar FinishTakeHit end");
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

    protected bool DodgedTheHit() 
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


}
