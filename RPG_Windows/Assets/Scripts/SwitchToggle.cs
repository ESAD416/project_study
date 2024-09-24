using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    public Image HandleImage;
    public bool IsOn = false;
    
    private RectTransform m_handleRect;
    private Vector2 m_OffPivot = new Vector2(0.5f, 0.5f);
    private Vector2 m_OnPivot = new Vector2(-0.5f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        m_handleRect = HandleImage.GetComponent<RectTransform>();
        
        m_handleRect.pivot = IsOn ? m_OnPivot : m_OffPivot;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHandlePivot();
    }

    public void Toggle() {
        IsOn = !IsOn;
    }

    private void UpdateHandlePivot() {
        m_handleRect.pivot = IsOn ? m_OnPivot : m_OffPivot;
    }
}
