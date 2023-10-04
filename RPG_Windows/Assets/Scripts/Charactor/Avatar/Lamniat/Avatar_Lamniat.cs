using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static JumpMechanismUtils;

public class Avatar_Lamniat : Avatar
{
    protected override void OnEnable() {
        base.OnEnable();
        m_inputControls.Lamniat_Land.Enable();
    }

}
