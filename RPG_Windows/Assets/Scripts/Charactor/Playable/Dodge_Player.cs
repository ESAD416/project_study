using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Player<T> : MonoBehaviour where T : Collider2D
{
    #region 基本物件

    [Header("基本物件")]
    [SerializeField] private Player<T> m_player;

    protected Animator m_playerAnimator;

    #endregion

    #region 基本參數

    [Header("基本參數")]
    public bool IsDodging;
    [SerializeField] protected float m_dodgeClipTime;
    public float DodgeClipTime => this.m_dodgeClipTime;

    #endregion

    protected virtual void Awake() 
    {
        m_playerAnimator = m_player.Animator;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }


}
