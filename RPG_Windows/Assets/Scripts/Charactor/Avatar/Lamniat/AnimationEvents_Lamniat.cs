using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents_Lamniat : MonoBehaviour
{
    [SerializeField] private Movement_Lamniat m_movement;
    public Movement_Lamniat Movement => this.m_movement;
    [SerializeField] private Combat_Lamniat m_combat;
    public Combat_Lamniat Combat => this.m_combat;
    [SerializeField] private Dodge_Lamniat m_dodge;
    public Dodge_Lamniat Dodge => this.m_dodge;

    // Start is called before the first frame update
    void Start()
    {
        //AnimeUtils.GetAnimatorClipInfo(m_avatarAnimator, "TriggerLayer", "Lamniat_attack_1_chop_clockwise");
    }

    #region 戰鬥相關動畫事件
    public void OnMeleeCombo1() {
        m_combat.SetMeleeComboCounter(1);
    }

    public void OnMeleeCombo2() {
        m_combat.SetMeleeComboCounter(2);
    }

    public void OnMeleeCombo3() {
        m_combat.SetMeleeComboCounter(3);
    }

    public void OnPreAttack() 
    {
        if(!m_combat.IsPreAttacking) m_combat.IsPreAttacking = true;
        if(m_combat.IsPostAttacking) m_combat.IsPostAttacking = false;
    }

    public void OnAttackHitBox() 
    {
        if(m_combat.IsPreAttacking) m_combat.IsPreAttacking = false;
        if(m_combat.IsPostAttacking) m_combat.IsPostAttacking = false;
    }

    public void OnPostAttack() 
    {
        if(m_combat.IsPreAttacking) m_combat.IsPreAttacking = false;
        if(!m_combat.IsPostAttacking) m_combat.IsPostAttacking = true;
    }

    public void CanCancelRecovery() {
        m_combat.CancelRecovery = true;
    }

    public void CantCancelRecovery() {
        m_combat.CancelRecovery = false;
    }
    #endregion
}
