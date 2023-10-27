using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("基本參數")]
    [SerializeField] private float m_maxHealth = 100;
    /// <summary>
    /// 最大血量
    /// </summary>
    public float MaxHealth => m_maxHealth;
    public void SetMaxHealth(float maxH) {
        this.m_maxHealth = maxH;
    }

    [SerializeField] private float m_currHealth = 20;
    /// <summary>
    /// 當前血量
    /// </summary>
    public float CurrHealth => m_currHealth;
    public void SetCurrHealth(float currH) {
        this.m_currHealth = currH;
    }

    /// <summary>
    /// 判定是否死亡
    /// </summary>
    [SerializeField] private bool isDead = false;


    [Header("血量變化觸發事件")]
    public UnityEvent OnTakenDamage;
    public UnityEvent OnGetHeal;


    // Start is called before the first frame update
    private void Start()
    {
        SetMaxHealth(100f);
        SetCurrHealth(100f);
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrHealth <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log("Die");
        isDead = true;
        //Animator?.SetBool("isDead", isDead); 
        //GetComponent<Collider2D>().enabled = false;
    }

    public float GetCurrHealthNormalized() {
        float result = 1.0f;
        float normalized = 100.0f / MaxHealth;
        result = CurrHealth * normalized / 100.0f;
        return result;
    }

    public void Decrease(float amount) {
        Debug.Log("TakeDamage: "+amount);
        m_currHealth -= amount;


        if(m_currHealth < 0) m_currHealth = 0;
        OnTakenDamage?.Invoke();
    }

    public void Increase(float amount) {
        Debug.Log("GetHeal: "+amount);
        m_currHealth += amount;
        if(m_currHealth > 0) m_currHealth = MaxHealth;
        OnGetHeal?.Invoke();
    }
}
