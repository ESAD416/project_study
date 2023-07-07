using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator_Slider : MonoBehaviour
{
    public Slider slider;
    public float duration;

    private float elapsedTime = 0f;
    private float currentValue;

    private void Update() {
        // 遞增時間
        elapsedTime += Time.deltaTime;

        // 計算當前值（根據時間的插值）
        float t = Mathf.Clamp01(elapsedTime / duration);
        currentValue = Mathf.Lerp(slider.minValue, slider.maxValue, t);

        // 更新 Slider 的值
        slider.value = currentValue;

        if(elapsedTime >= duration) {
            // 確保最終值正確
            elapsedTime = duration;
            currentValue = slider.maxValue;
            slider.value = currentValue;
        }

    }

}
