using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class OffScreenIndicatorCtrl : MonoBehaviour
{
    public CinemachineVirtualCamera virtualMainCam;
    public Player player;
    public GameObject indicatorPrefab;
    public float checkFrequency = 0.1f;
    public Vector2 offset;

    public List<IndicatorModel> targets;
    protected RectTransform canvasRect;
    protected Camera activeCam;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        canvasRect = GetComponent<RectTransform>();;
        targets = new List<IndicatorModel>();
        InstantiateIndicators();
        StartCoroutine(UpdateIndicators());
    }

    public void InstantiateIndicators(float duration = 0) {
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
        }
        if(duration > 0) {
            Invoke("DestoryIndicators", duration);
        }
    }

    protected virtual void UpdatePosition(IndicatorModel indicator) {
        var rect = indicator.rectTransform.rect;
        activeCam = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera;
        
        var targetScreenPos = activeCam.WorldToViewportPoint(indicator.target.position);
        Debug.Log("UpdatePosition before targetScreenPos: "+targetScreenPos);
        if(targetScreenPos.x > 0 && targetScreenPos.x < 1 && targetScreenPos.y > 0 && targetScreenPos.y < 1 )
        {
            // 目標物件在畫面內，隱藏指示器
        }

        var indicatorPos = activeCam.WorldToScreenPoint(indicator.target.position);
        //Debug.Log("UpdatePosition before indicatorPos: "+targetScreenPos);
        Vector3 dir = (indicator.target.position - player.Center).normalized;
        //Debug.Log("UpdatePosition dir: "+dir);

        indicatorPos.x = Mathf.Clamp(indicatorPos.x, rect.width / 2, Screen.width - rect.width / 2) + offset.x;
        indicatorPos.y = Mathf.Clamp(indicatorPos.y, rect.height / 2, Screen.height - rect.height / 2) + offset.y;
        indicatorPos.z = 0;

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, indicatorPos, null, out anchoredPos);


        // Vector3 screenEdge = activeCam.ViewportToWorldPoint(new Vector3(Mathf.Clamp(indicatorPos.x, 0.1f, 0.9f), Mathf.Clamp(indicatorPos.y, 0.1f, 0.9f), activeCam.nearClipPlane));
        // indicatorPos = new Vector3(screenEdge.x, screenEdge.y, 0f);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //var angle = Vector3.Angle(dir, indicator.referenceAxis);
        //var quaternion = dir.x > 0 ? Quaternion.Euler(0, 0, angle) : Quaternion.Euler(0, 0, -angle);

        // Debug.Log("UpdatePosition indicatorPos: "+indicatorPos);
        indicator.indicatorUI.rotation = Quaternion.Euler(0, 0, angle);
        indicator.rectTransform.anchoredPosition = anchoredPos;
    }

    protected virtual IEnumerator UpdateIndicators() {
        while(true) {
            foreach(var indicator in targets) {
                UpdatePosition(indicator);
            }

            yield return new WaitForSeconds(checkFrequency);  
        }
    }

    protected virtual void DestoryIndicators() {
        var indicators = GetComponentsInChildren<Transform>();
        foreach(var indicator in indicators) {
            if(indicator != transform) {
                Destroy(indicator.gameObject);
            }
        }

        targets = new List<IndicatorModel>();
    }
}
