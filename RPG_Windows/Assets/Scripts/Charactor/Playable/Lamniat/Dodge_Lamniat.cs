using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Lamniat : Dodge_Avatar
{
    protected override void Awake() {
        base.Awake();

        m_dodgeClipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_avatarAnimator, "Lamniat_dodge");
    }

    protected override void Start() {
        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Dodge.performed += content => {
            IsDodging = true;
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Dodge.canceled += content => {
            IsDodging = false;
        };
    }

}
