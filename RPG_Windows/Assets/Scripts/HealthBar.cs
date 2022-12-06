using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] Color high = Color.green;

    [SerializeField] Color low = Color.yellow;

    [SerializeField] Color danger = Color.red;

    public void SetHealth(int health, int maxHealth) {
        Debug.Log("HealthBar SetHealth: health, "+health+", maxHealth, "+maxHealth);
        slider.gameObject.SetActive((health < maxHealth) && (health > 0));
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void UpdateFillColor() {
        // Debug.Log("fillRect: "+slider.fillRect);
        // Debug.Log("Img: "+slider.fillRect.GetComponent<Image>());
        // Debug.Log("color a: "+slider.fillRect.GetComponent<Image>().color);
        // Debug.Log("Lerpcolor: "+Color.Lerp(low, high, slider.value / 40) );
        Color fillcolor = Color.Lerp(low, high, slider.value / 40);
        if(slider.value >= slider.maxValue / 2) {
            fillcolor = high;
        } else if(slider.value > slider.maxValue / 4 && slider.value < slider.maxValue / 2) {
            fillcolor = Color.Lerp(danger, low, slider.value / 40);
        } else if(slider.value <= slider.maxValue / 4) {
            fillcolor = danger;
        }

        slider.fillRect.GetComponent<Image>().color = fillcolor; 
    }
}
