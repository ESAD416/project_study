using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class HealthSystemModel
{
    private float m_currHealth = 20;
    /// <summary>
    /// 當前血量
    /// </summary>
    public float CurrHealth => m_currHealth;
    public void SetCurrHealth(float currH) {
        this.m_currHealth = currH;
    }

    private float m_maxHealth = 20;
    /// <summary>
    /// 最大血量
    /// </summary>
    public float MaxHealth => m_maxHealth;
    public void SetMaxHealth(float maxH) {
        this.m_maxHealth = maxH;
    }

    /// <summary>
    /// 血量變化觸發事件
    /// </summary>
    public UnityEvent OnHealthChange;
    

    public HealthSystemModel(float currH, float maxH) {
        this.m_currHealth = currH;
        this.m_maxHealth = maxH;
    }

    public HealthSystemModel(float maxH) {
        this.m_currHealth = maxH;
        this.m_maxHealth = maxH;
    }

    public float GetCurrHealthNormalized() {
        float result = 1.0f;
        float normalized = 100.0f / MaxHealth;
        result = CurrHealth * normalized / 100.0f;
        return result;
    }

    public void OnDamage(int dmgAmount) {
        Debug.Log("TakeDamage: "+dmgAmount);
        m_currHealth -= dmgAmount;
        if(m_currHealth < 0) m_currHealth = 0;
        OnHealthChange?.Invoke();
    }

    public void OnHeal(int healAmount) {
        Debug.Log("GetHeal: "+healAmount);
        m_currHealth += healAmount;
        if(m_currHealth > 0) m_currHealth = MaxHealth;
        OnHealthChange?.Invoke();
    }
}
