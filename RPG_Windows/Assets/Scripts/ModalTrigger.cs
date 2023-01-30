using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModalTrigger : MonoBehaviour
{
    public bool triggerOnEnable = true;
    public string title;
    public Sprite imageSprite;
    [TextArea(4, 4)] public string message;

    [Space()]
    public UnityEvent onConfirmCallback;
    public UnityEvent onDeclineCallback;
    public UnityEvent onAlternateCallback;

    public void Click() {
        Debug.Log("ModalTrigger is Click: ");
    }

    

    public void Trigger() {
        if(!triggerOnEnable) return;
        Debug.Log("ModalPanel is null: "+UIController.instance.ModalPanel == null);

        SetButtonActions(out Action confirmAction, out Action declineAction, out Action alternateAction);
        UIController.instance.ModalPanel.ShowModalVertically(title, imageSprite, message, "Continue", confirmAction, "Cancel", declineAction);
    }

    private void SetButtonActions(out Action confirmAction, out Action declineAction, out Action alternateAction) {
        confirmAction = null;
        declineAction = null;
        alternateAction = null;

        if(onConfirmCallback.GetPersistentEventCount() > 0) {
            confirmAction = onConfirmCallback.Invoke;
        }
        if(onDeclineCallback.GetPersistentEventCount() > 0) {
            declineAction = onDeclineCallback.Invoke;
        }
        if(onAlternateCallback.GetPersistentEventCount() >0) {
            alternateAction = onAlternateCallback.Invoke;
        }
    }
}
