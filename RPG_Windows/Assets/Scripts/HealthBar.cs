using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("基本物件")]
    [SerializeField] private Slider m_slider;
    [SerializeField] private Image m_fill;
    [SerializeField] private Gradient m_gradient;

    [Header("基本參數")]
    public float MaxValue;

    private void OnEnable() {
        Debug.Log("HealthBar OnEnable");
        SetMaxValue(MaxValue);
        SetValue(MaxValue);
    }

    private void Start() {
        
    }

    public void SetMaxValue(float maxValue) {
        Debug.Log("HealthBar SetMaxValue, "+maxValue);
        m_slider.maxValue = maxValue;
        //m_slider.value = maxHealth;
        
    }

    public void SetValue(float value) {
        Debug.Log("HealthBar SetValue, "+value);
        m_slider.value = value;
        if(m_slider.value >= m_slider.maxValue) m_slider.value = m_slider.maxValue;

        UpdateFillColor(m_slider.normalizedValue);
    }

    public void UpdateFillColor(float normalizedValue) {
        m_fill.color = m_gradient.Evaluate(normalizedValue);
    }

    public void ShowHealthBarUI() {
        gameObject.SetActive(true);
    }

    public void HideHealthBarUI() {
        gameObject.SetActive(false);
    }

}
