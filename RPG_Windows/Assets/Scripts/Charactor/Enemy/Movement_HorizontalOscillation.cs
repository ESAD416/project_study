using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_HorizontalOscillation : Movement_Enemy
{
    [Header("Movement_HorizontalOscillation 基本參數")]
    public bool moveRight;

    [SerializeField] protected Detector_EnemyPatrol m_leftDetector_Patrol;
    [SerializeField] protected Detector_EnemyPatrol m_rightDetector_Patrol;
    
    [SerializeField] protected List<Transform> pathTurn = new List<Transform>();
    private Vector3 defaultPos;
    private Dictionary<string, Vector3> turnPara = new Dictionary<string, Vector3>();

    protected override void Start()
    {
        base.Start();
        defaultPos = m_enemy.transform.position;

        foreach (Transform trun in pathTurn) 
        {
            Vector3 v = defaultPos - trun.position;
            turnPara.Add(trun.gameObject.name, v);
        }
    }

    protected override void Update() 
    {  
        base.Update();
        if(m_detector_chaser != null && m_detector_chaser.gameObject.activeSelf) {
            if(m_detector_chaser.TargetModel != null && m_detector_chaser.targetVisable) 
            {
                UpdatePathParaForHorizontal();
            } 
            else 
            {
                StartCoroutine(UpdatePathParaEndingDelay());
            }
        }
    }

    protected override void FixedUpdate() {
        if(!m_enemy.CurrentBaseState.Equals(BaseStateMachine_Enemy.BaseState.Dead)) {
            if(m_enemy.CurrentEnemyState != null && m_enemy.CurrentEnemyState.State.Equals(EnemyStateMachine.EnemyState.Chase)) {
                // 如果有套用Patrol與Chase索敵模組
                if(m_movement.x > 0) moveRight = true;
                else if(m_movement.x < 0) moveRight = false;
            } else {
                if(moveRight) 
                {
                    SetMovement(Vector3.right);
                    m_leftDetector_Patrol?.gameObject.SetActive(false);
                    m_rightDetector_Patrol?.gameObject.SetActive(true);
                }
                else 
                {
                    SetMovement(Vector3.left);
                    m_leftDetector_Patrol?.gameObject.SetActive(true);
                    m_rightDetector_Patrol?.gameObject.SetActive(false);
                } 
            }

            // if(m_enemy.CurrentEnemyState.State.Equals(EnemyStateMachine.EnemyState.Patrol)) 
            // {
            //     if(moveRight) 
            //     {
            //         SetMovement(Vector3.right);
            //         m_leftDetector_Patrol?.gameObject.SetActive(false);
            //         m_rightDetector_Patrol?.gameObject.SetActive(true);
            //     }
            //     else 
            //     {
            //         SetMovement(Vector3.left);
            //         m_leftDetector_Patrol?.gameObject.SetActive(true);
            //         m_rightDetector_Patrol?.gameObject.SetActive(false);
            //     } 
            // } else if(m_enemy.CurrentEnemyState != null && m_enemy.CurrentEnemyState.State.Equals(EnemyStateMachine.EnemyState.Chase)) {
            //     if(m_movement.x > 0) moveRight = true;
            //     else if(m_movement.x < 0) moveRight = false;
            // }
            if(m_enemySprtRenderer!= null) m_enemySprtRenderer.flipX = moveRight;
        }

        base.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D trig)
	{
		if (trig.gameObject.tag == "Enemy_Turn"){
			if (moveRight){
				moveRight = false;
			}
			else{
				moveRight = true;
			}	
		}
	}

    private void UpdatePathParaForHorizontal() {
        var enemyPos = m_enemy.transform.position;
        foreach (Transform turn in pathTurn) {
                var v = turnPara[turn.gameObject.name];
                turn.position = enemyPos - v;
            
        }
    }

    private IEnumerator UpdatePathParaEndingDelay() {
        UpdatePathParaForHorizontal();
        yield return new WaitForSeconds(m_detector_chaser.chaseModeDelay);  // hardcasted casted time for debugged
    }
}
