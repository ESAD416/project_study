using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Pursuing : Movement_Base
{
    [Header("Movement_Pursuing 物件")]
    [SerializeField] protected Detector_EnemyPursuing m_detector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetMovement(m_detector.TargetModel.position - m_detector.transform.position);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
    }
}
