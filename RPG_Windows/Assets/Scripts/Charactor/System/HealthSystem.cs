using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("基本物件")]
    [SerializeField] private HealthBar m_healthBar;
    public HealthBar HealthBar => m_healthBar;

    [Header("基本參數")]
    [SerializeField] private float m_maxHealth = 100;
    /// <summary>
    /// 最大血量
    /// </summary>
    public float MaxHealth => m_maxHealth;
    public void SetMaxHealth(float maxH) {
        this.m_maxHealth = maxH;
    }

    [SerializeField] private float m_currHealth;
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
    public UnityEvent<float> OnTakenDamage;
    public UnityEvent<float> OnGetHeal;


    // Start is called before the first frame update
    private void Start()
    {
        SetMaxHealth(m_maxHealth);
        m_healthBar.SetMaxValue(m_maxHealth);

        SetCurrHealth(m_maxHealth);
        m_healthBar.SetValue(m_maxHealth);

        isDead = false;
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

        m_healthBar.SetValue(m_currHealth);
        OnTakenDamage?.Invoke(m_currHealth);
    }

    public void Increase(float amount) {
        Debug.Log("GetHeal: "+amount);
        m_currHealth += amount;

        if(m_currHealth > 0) m_currHealth = MaxHealth;

        m_healthBar.SetValue(m_currHealth);
        OnGetHeal?.Invoke(m_currHealth);
    }
}
