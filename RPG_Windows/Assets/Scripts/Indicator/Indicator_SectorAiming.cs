using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Indicator_SectorAiming : MonoBehaviour
{
    [SerializeField] private LineRenderer m_BisectorLineRenderer;
    [SerializeField] private LineRenderer m_Sector1LineRenderer;
    [SerializeField] private LineRenderer m_Sector2LineRenderer;
    [SerializeField] private Avatar_Lamniat m_targetAvatar;
    [SerializeField] private Combat_Lamniat m_targetCombat;
    [SerializeField] private PolygonCollider2D m_mapRange;
    private Vector3 m_StartPos;
    private Vector3 m_EndPos;
    [SerializeField] private Vector3 m_endPosOffset = new Vector3(1, 1, 0);
    [SerializeField] private float m_SectorAngle = 60f;
    [SerializeField] private float m_sectorClosureDuration = 1.0f;
    [SerializeField] private float m_sectorClosureTimeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = m_targetAvatar.Center;
        m_EndPos = m_targetAvatar.Center;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_targetCombat.IsShooting) {
            SetLine();

            m_sectorClosureTimeElapsed += Time.deltaTime;
            if (m_sectorClosureTimeElapsed >= m_sectorClosureDuration)
            {
                m_sectorClosureTimeElapsed = m_sectorClosureDuration;
            } 
        }
        else ResetLine();

    }

    public void SetLine()
    {
        m_StartPos = m_targetAvatar.Center;
        m_EndPos = m_targetAvatar.Center;
        while (m_mapRange.bounds.Contains(m_EndPos)) {
            m_EndPos =  m_EndPos + new Vector3(m_targetCombat.ShootDir.x * m_endPosOffset.x, m_targetCombat.ShootDir.y * m_endPosOffset.y, 0);
        }

        UpdateSectorClosureLine();
    }

    public void UpdateSectorClosureLine() {
        if(m_sectorClosureTimeElapsed == m_sectorClosureDuration) {
            m_BisectorLineRenderer.SetPosition(0, m_StartPos);
            m_BisectorLineRenderer.SetPosition(1, m_EndPos);

            m_Sector1LineRenderer.SetPosition(0, Vector3.zero);
            m_Sector1LineRenderer.SetPosition(1, Vector3.zero);

            m_Sector2LineRenderer.SetPosition(0, Vector3.zero);
            m_Sector2LineRenderer.SetPosition(1, Vector3.zero);
        } 
        else {
            var angle = m_SectorAngle * ((m_sectorClosureDuration - m_sectorClosureTimeElapsed) / m_sectorClosureDuration) / 2;
            Vector3 direction1 = Quaternion.Euler(0, 0, angle) * m_targetCombat.ShootDir;
            Vector3 direction2 = Quaternion.Euler(0, 0, -angle) * m_targetCombat.ShootDir;

            var endPos1 = m_targetAvatar.Center;
            while (m_mapRange.bounds.Contains(endPos1)) {
                endPos1 =  endPos1 + new Vector3(direction1.x * m_endPosOffset.x, direction1.y * m_endPosOffset.y, 0);
            }

            var endPos2 = m_targetAvatar.Center;
            while (m_mapRange.bounds.Contains(endPos2)) {
                endPos2 =  endPos2 + new Vector3(direction2.x * m_endPosOffset.x, direction2.y * m_endPosOffset.y, 0);
            }

            m_Sector1LineRenderer.SetPosition(0, m_StartPos);
            m_Sector1LineRenderer.SetPosition(1, endPos1);

            m_Sector2LineRenderer.SetPosition(0, m_StartPos);
            m_Sector2LineRenderer.SetPosition(1, endPos2);
        }
    }

    public void ResetLine() {
        m_StartPos = Vector3.zero;
        m_EndPos = Vector3.zero;

        m_BisectorLineRenderer.SetPosition(0, m_StartPos);
        m_BisectorLineRenderer.SetPosition(1, m_EndPos);

        m_Sector1LineRenderer.SetPosition(0, m_StartPos);
        m_Sector1LineRenderer.SetPosition(1, m_EndPos);

        m_Sector2LineRenderer.SetPosition(0, m_StartPos);
        m_Sector2LineRenderer.SetPosition(1, m_EndPos);

        m_sectorClosureTimeElapsed = 0f;
    }

}
