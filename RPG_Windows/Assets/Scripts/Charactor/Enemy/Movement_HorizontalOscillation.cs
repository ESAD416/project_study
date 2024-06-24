using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_HorizontalOscillation : Movement_Base
{
    [Header("Movement_HorizontalOscillation 參數")]
    public bool MoveRight;
    [SerializeField] protected Vector3 defaultMovement = Vector3.left;

    [Header("Movement_HorizontalOscillation 物件")]
    [SerializeField] protected MonoBehaviour m_target;
    private ICharactor target;

    protected Rigidbody2D m_targetRdbd;
    protected SpriteRenderer m_targetSprtRenderer;
    protected Animator m_targetAnimator;

    [SerializeField] protected List<Transform> pathTurn = new List<Transform>();
    private Vector3 defaultPos;
    private Dictionary<string, Vector3> turnPos = new Dictionary<string, Vector3>();

    // [SerializeField] protected Detector_EnemyPatrol m_leftDetector_Patrol;
    // [SerializeField] protected Detector_EnemyPatrol m_rightDetector_Patrol;

    protected virtual void Awake() {
        target = m_target as ICharactor;

        m_targetRdbd = target.Rigidbody;
        m_targetSprtRenderer = target.SprtRenderer;
        m_targetAnimator = target.Animator;
    }
    

    protected virtual void Start()
    {
        defaultPos = m_target.transform.position;

        foreach (Transform trun in pathTurn) 
        {
            Vector3 v = defaultPos - trun.position;
            turnPos.Add(trun.gameObject.name, v);
        }

        SetDefaultMovement();
    }

    protected virtual void Update() 
    {  
        // if(m_detector_chaser != null && m_detector_chaser.gameObject.activeSelf) {
        //     if(m_detector_chaser.TargetModel != null && m_detector_chaser.targetVisable) 
        //     {
                
        //     } 
        //     else 
        //     {
        //         StartCoroutine(UpdatePathParaEndingDelay());
        //     }
        // }

        if(MoveRight) SetMovement(Vector3.right);
        else SetMovement(Vector3.left);
        
        if(m_targetSprtRenderer!= null) m_targetSprtRenderer.flipX = MoveRight;


        if(target.CurrentBaseStateName.Equals(Constant.CharactorState.Dead) || 
           target.CurrentBaseStateName.Equals(Constant.CharactorState.Attack)) {
            SetMovement(Vector3.zero);
        }


        // ToDo 脫離路徑後需持續更新路標位置
        //UpdateTurnPosForHorizontalPath();
    }

    protected virtual void FixedUpdate() {
        // if(!m_enemy.CurrentBaseState.Equals(Constant.CharactorState.Dead)) {
        //     if(m_enemy.CurrentEnemyState != null && m_enemy.CurrentEnemyState.State.Equals(Constant.EnemyState.Chase)) {
        //         // 如果有套用Patrol與Chase索敵模組
        //         if(m_movement.x > 0) MoveRight = true;
        //         else if(m_movement.x < 0) MoveRight = false;
        //     } else {
        //         if(MoveRight) 
        //         {
        //             SetMovement(Vector3.right);
        //             // m_leftDetector_Patrol?.gameObject.SetActive(false);
        //             // m_rightDetector_Patrol?.gameObject.SetActive(true);
        //         }
        //         else 
        //         {
        //             SetMovement(Vector3.left);
        //             // m_leftDetector_Patrol?.gameObject.SetActive(true);
        //             // m_rightDetector_Patrol?.gameObject.SetActive(false);
        //         } 
        //     }

        //     
        // }

        m_targetRdbd.velocity = m_movement.normalized * MoveSpeed;
    }

    public void TurnDir()
	{
		if (MoveRight){
            MoveRight = false;
        }
        else{
            MoveRight = true;
        }	
	}

    public void SetDefaultMovement() {
        SetMovement(defaultMovement);
    }

    private void UpdateTurnPosForHorizontalPath() {
        var enemyPos = m_target.transform.position;
        foreach (Transform turn in pathTurn) {
                var v = turnPos[turn.gameObject.name];
                turn.position = enemyPos - v;
            
        }
    }

    // private IEnumerator UpdatePathParaEndingDelay() {
    //     UpdatePathParaForHorizontal();
    //     yield return new WaitForSeconds(m_detector_chaser.chaseModeDelay);  // hardcasted casted time for debugged
    // }
}
