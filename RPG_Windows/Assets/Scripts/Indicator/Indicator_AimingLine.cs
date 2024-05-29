using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator_AimingLine : MonoBehaviour
{
    private LineRenderer m_lineRenderer;

    [SerializeField] private Avatar_Lamniat m_targetAvatar;
    [SerializeField] private Combat_Lamniat m_targetCombat;
    [SerializeField] private PolygonCollider2D m_mapRange;

    private Vector3 m_StartPos;
    private Vector3 m_EndPos;
    [SerializeField] private float m_maxLength = 10f;


    // Start is called before the first frame update
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_StartPos = m_targetAvatar.Center;
        m_EndPos = m_targetAvatar.Center;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_targetCombat.IsShooting) SetLine();
        else ResetLine();
    }

    public void SetLine()
    {
        m_StartPos = m_targetAvatar.Center;
        m_EndPos = m_targetAvatar.Center + new Vector3(m_targetCombat.ShootDir.x * m_maxLength, m_targetCombat.ShootDir.y * m_maxLength, 0);;
        

        m_lineRenderer.SetPosition(0, m_StartPos);
        m_lineRenderer.SetPosition(1, m_EndPos);
    }

    public void ResetLine() {
        m_StartPos = Vector3.zero;
        m_EndPos = Vector3.zero;

        m_lineRenderer.SetPosition(0, m_StartPos);
        m_lineRenderer.SetPosition(1, m_EndPos);
    }
}
