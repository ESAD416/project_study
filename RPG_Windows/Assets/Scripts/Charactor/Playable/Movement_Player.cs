using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IMovementPlayer {
    Vector2 Movement { get; }
}

public class Movement_Player<T> : Movement_Base, IMovementPlayer where T : Collider2D
{
    #region 物件

    protected new Player<T> movingTarget;
    public new bool IsMoving {
        get {
            // Player移動由使用者控制，故以接收輸入的移動向量為判斷基準;
            return this.m_movement != Vector2.zero;
        }
    }

    #endregion

    protected override void Awake() 
    {
        movingTarget = m_movingTarget as Player<T>;

        m_targetRdbd = movingTarget.Rigidbody;
        m_targetSprtRenderer = movingTarget.SprtRenderer;
        m_targetAnimator = movingTarget.Animator;
    }

    protected override void OnEnable() {

    }

    protected override void OnDisable() {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetAnimateMovementPara();
    }

    protected override void FixedUpdate() 
    {
        base.FixedUpdate();
    }

    protected void SetAnimateMovementPara() 
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add("movementX", m_movement.x);
        dict.Add("movementY", m_movement.y);
        dict.Add("facingDirX", m_facingDir.x);
        dict.Add("facingDirY", m_facingDir.y);

        if(m_targetAnimator != null) AnimeUtils.SetAnimateFloatPara(m_targetAnimator, dict);
    }
}
