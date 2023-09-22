using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class HealthSystemModel
{
    /// <summary>
    /// 當前血量
    /// </summary>
    [SerializeField] private int currHealth = 20;
    public int CurrHealth => currHealth;

    /// <summary>
    /// 最大血量
    /// </summary>
    [SerializeField] private int maxHealth = 20;
    public int MaxHealth => maxHealth;

    /// <summary>
    /// 血量變化觸發事件
    /// </summary>
    public UnityAction OnHealthChange;
    

    public HealthSystemModel(int currH, int maxH) {
        this.currHealth = currH;
        this.maxHealth = maxH;
    }

    public HealthSystemModel(int maxH) {
        this.currHealth = maxH;
        this.maxHealth = maxH;
    }

    public float GetCurrHealthNormalized() {
        float result = 1.0f;
        float normalized = 100.0f / maxHealth;
        result = currHealth * normalized / 100.0f;
        return result;
    }

    public void OnDamage(int dmgAmount) {
        Debug.Log("TakeDamage: "+dmgAmount);
        currHealth -= dmgAmount;
        if(currHealth < 0) currHealth = 0;
        OnHealthChange?.Invoke();
    }

    public void OnHeal(int healAmount) {
        Debug.Log("GetHeal: "+healAmount);
        currHealth += healAmount;
        if(currHealth > 0) currHealth = maxHealth;
        OnHealthChange?.Invoke();
    }
}
