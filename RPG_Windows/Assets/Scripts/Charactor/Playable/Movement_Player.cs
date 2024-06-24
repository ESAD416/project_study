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

    [Header("Movement_Player 物件")]
    [SerializeField] protected Player<T> m_player;
    protected Rigidbody2D m_playerRdbd;
    protected SpriteRenderer m_playerSprtRenderer;
    protected Animator m_playerAnimator;

    #endregion

    protected virtual void Awake() 
    {
        m_playerRdbd = m_player.Rigidbody;
        m_playerSprtRenderer = m_player.SprtRenderer;
        m_playerAnimator = m_player.Animator;
    }

    protected virtual void OnEnable() {

    }

    protected virtual void OnDisable() {

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

    protected virtual void FixedUpdate() 
    {
        m_playerRdbd.velocity = m_movement.normalized * MoveSpeed;
    }

    protected void SetAnimateMovementPara() 
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add("movementX", m_movement.x);
        dict.Add("movementY", m_movement.y);
        dict.Add("facingDirX", m_facingDir.x);
        dict.Add("facingDirY", m_facingDir.y);

        if(m_playerAnimator != null) AnimeUtils.SetAnimateFloatPara(m_playerAnimator, dict);
    }
}
