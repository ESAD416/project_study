using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem m_targetHealthSystem;
    [SerializeField] private Slider m_slider;
    [SerializeField] private Image m_fill;
    [SerializeField] private Gradient m_gradient;

    private void Start() {
        SetMaxHealth(m_targetHealthSystem.MaxHealth);
        SetHealth(m_targetHealthSystem.MaxHealth);
    }

    public void SetMaxHealth(float maxHealth) {
        Debug.Log("HealthBar SetMaxHealth, "+maxHealth);
        m_slider.maxValue = maxHealth;
        //m_slider.value = maxHealth;
        
    }

    public void SetHealth(float health) {
        Debug.Log("HealthBar SetHealth, "+health);
        m_slider.value = health;
        if(m_slider.value >= m_slider.maxValue) m_slider.value = m_slider.maxValue;

        UpdateFillColor(m_slider.normalizedValue);
    }

    public void UpdateFillColor(float normalizedValue) {
        m_fill.color = m_gradient.Evaluate(normalizedValue);
    }

    public void UpdateFillColor(float normalizedValue, Color low, Color high, Color danger) {
        m_fill.color = Color.Lerp(low, high, normalizedValue);
        // // Debug.Log("fillRect: "+slider.fillRect);
        // // Debug.Log("Img: "+slider.fillRect.GetComponent<Image>());
        // // Debug.Log("color a: "+slider.fillRect.GetComponent<Image>().color);
        // // Debug.Log("Lerpcolor: "+Color.Lerp(low, high, slider.value / 40) );
        // Color fillcolor = Color.Lerp(low, high, slider.value / 40);
        // if(slider.value >= slider.maxValue / 2) {
        //     fillcolor = high;
        // } else if(slider.value > slider.maxValue / 4 && slider.value < slider.maxValue / 2) {
        //     fillcolor = Color.Lerp(danger, low, slider.value / 40);
        // } else if(slider.value <= slider.maxValue / 4) {
        //     fillcolor = danger;
        // }

        // slider.fillRect.GetComponent<Image>().color = fillcolor; 
    }
}
