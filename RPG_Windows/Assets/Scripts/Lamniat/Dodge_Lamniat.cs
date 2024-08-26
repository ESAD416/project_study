using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Lamniat : MonoBehaviour
{
    #region 基本物件

    [Header("基本物件")]
    [SerializeField] private Lamniat m_lamniatPlayer;

    protected Animator m_playerAnimator;

    #endregion

    #region 基本參數

    [Header("基本參數")]
    public bool IsDodging;
    [SerializeField] protected float m_dodgeClipTime;
    public float DodgeClipTime => this.m_dodgeClipTime;

    #endregion

    protected void Awake() {
        m_playerAnimator = m_lamniatPlayer.Animator;

        m_dodgeClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_playerAnimator, "Lamniat_dodge");
    }

    protected void Start() {
        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Dodge.performed += content => {
            IsDodging = true;
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Dodge.canceled += content => {
            IsDodging = false;
        };
    }

}
