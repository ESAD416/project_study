using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicatorCtrl_Slider : OffScreenIndicatorCtrl
{
    public new void InstantiateIndicators(float duration = 0) {
        foreach(var indicator in targets) {
            if(indicator.indicatorUI == null) {
                indicator.indicatorUI = Instantiate(indicatorPrefab).transform;
                indicator.indicatorUI.SetParent(transform);
            }

            var rectTransform = indicator.indicatorUI.GetComponent<RectTransform>();
            if(rectTransform == null) {
                rectTransform = indicator.indicatorUI.gameObject.AddComponent<RectTransform>();
            }
            indicator.rectTransform = rectTransform;

            if(indicator.rectTransform.GetComponent<Indicator_Slider>() != null) {
                if(duration > 0) {
                    indicator.rectTransform.GetComponent<Indicator_Slider>().duration = duration;
                }
            }
        }
        if(duration > 0) {
            Invoke("DestoryIndicators", duration);
        }
    }

    protected override void UpdatePosition(IndicatorModel indicator) {
        var rect = indicator.rectTransform.rect;
        activeCam = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera;

        var targetScreenPos = activeCam.WorldToViewportPoint(indicator.target.position);
        // Debug.Log("UpdatePosition before targetScreenPos: "+targetScreenPos);
        if(targetScreenPos.x > 0 && targetScreenPos.x < 1 && targetScreenPos.y > 0 && targetScreenPos.y < 1 )
        {
            // 目標物件在畫面內，隱藏指示器
        }

        var attackOffSet = activeCam.WorldToScreenPoint(indicator.target.position);
        // Debug.Log("UpdatePosition attackOffSet: "+targetScreenPos);
        Vector3 dir = (indicator.target.position - player.m_Center).normalized;
        // Debug.Log("UpdatePosition dir: "+dir);

        var camCenter = activeCam.WorldToScreenPoint(player.m_Center);
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, camCenter, null, out anchoredPos);

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        indicator.indicatorUI.rotation = Quaternion.Euler(0, 0, angle);
        indicator.rectTransform.anchoredPosition = anchoredPos;
    }

    protected override IEnumerator UpdateIndicators() {
        while(true) {
            foreach(var indicator in targets) {
                UpdatePosition(indicator);
            }

            yield return new WaitForSeconds(checkFrequency);  
        }
    }
}
