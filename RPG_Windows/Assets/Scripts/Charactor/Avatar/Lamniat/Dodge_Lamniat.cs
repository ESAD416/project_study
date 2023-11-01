using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Lamniat : Dodge_Avatar
{
    private bool isHoldInteraction = false;

    protected override void Awake() {
        base.Awake();

        m_dodgeClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_avatarAnimator, "Lamniat_dodge");
        Debug.Log("m_dodgeClipTime: "+m_dodgeClipTime);
    }

    protected override void Start() {
        m_inputControls = m_avatar.InputCtrl;

        m_inputControls.Lamniat_Land.Dodge.performed += content => {
            Debug.Log("Lamniat_Land.Dodge.performed");
            IsDodging = true;
        };

        m_inputControls.Lamniat_Land.Dodge.canceled += content => {
            IsDodging = false;
        };
    }

}
