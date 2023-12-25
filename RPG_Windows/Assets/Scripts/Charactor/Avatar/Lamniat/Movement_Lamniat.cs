using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Movement_Lamniat : Movement_Avatar
{
    private bool isHoldInteraction = false;

    protected override void OnEnable() {
        Debug.Log("Movement_Lamniat: OnEnable");
    }

    // Start is called before the first frame update
    protected override void Start() 
    {
         #region InputSystem事件設定
        m_inputControls = m_avatar.InputCtrl;

        m_inputControls.Lamniat_Land.Move.performed += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            SetFacingDir(inputVecter2);
            SetMovement(new Vector3(inputVecter2.x, inputVecter2.y));
            //m_avatar.SetStatus(Charactor.CharactorStatus.Move);
            m_avatar.SetCurrentBaseState(m_avatar.Move);

            var faceLeft = m_avatarSprtRenderer.flipX;
            if(m_avatarSprtRenderer!= null) m_avatarSprtRenderer.flipX = AnimeUtils.isLeftForHorizontalAnimation(Movement, faceLeft);
        };

        m_inputControls.Lamniat_Land.Move.canceled += content => {
            SetMovement(Vector3.zero);
            m_avatar.SetCurrentBaseState(m_avatar.Idle);
        };

        m_inputControls.Lamniat_Land.Hold.performed += content => {
            if(content.interaction is HoldInteraction) {
                isHoldInteraction = true;
            }
        };

        m_inputControls.Lamniat_Land.Hold.canceled += content => {
            if(content.interaction is HoldInteraction) {
                isHoldInteraction = false;
            }
        };

        #endregion
        
    }
    
    protected override void OnDisable() {
        Debug.Log("Movement_Lamniat: OnDisable");
    }
}
