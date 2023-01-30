using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalPanel : MonoBehaviour
{
    [SerializeField] private Transform windowBox;

    [Header("Modal Header")]
    [SerializeField] private Transform headerArea;
    [SerializeField] private Text titleField;

    [Header("Modal Content")]
    [SerializeField] private Transform contentArea;
    [Space()]
    [SerializeField] private Transform verticalLayoutArea;
    [SerializeField] private Image verticalLayoutImage;
    [SerializeField] private Text verticalLayoutText;

    [Space()]
    [SerializeField] private Transform horizontalLayoutArea;
    [SerializeField] private Transform horizontalImageContainer;
    [SerializeField] private Image horizontalLayoutImage;
    [SerializeField] private Text horizontalLayoutText;

    [Header("Modal Footer")]
    [SerializeField] private Transform footerArea;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Text confirmBtnMsg;
    [SerializeField] private Button declineBtn;
    [SerializeField] private Text declineBtnMsg;
    [SerializeField] private Button alternateBtn;
    [SerializeField] private Text alternateBtnMsg;

    private Action onConfirmAction;
    private Action onDeclineAction;
    private Action onAlternateAction;

    #region General

    public void Confirm() {
        onConfirmAction?.Invoke();
        //Close();
    }

    public void Decline() {
        onDeclineAction?.Invoke();
        //Close();
    }

    public void Alternate() {
        onAlternateAction?.Invoke();
        //Close();
    }

    public void DeactivateModelPanel() {
        Debug.Log("DeactivateModelPanel");
        gameObject.SetActive(false);
    }

    public void ActivateModelPanel() {
        gameObject.SetActive(true);
    }

    #endregion

    

    #region Horizontal
    private void ShowModalAsHorizontalLayout(string title, Sprite imageToShow, string message, string confirmMsg, Action confirmAction, string declineMsg = "", Action declineAction = null, string alternateMsg = "", Action alternateAction = null) {
        gameObject.SetActive(true);
        verticalLayoutArea.gameObject.SetActive(false);

        // hide the header if there's no title
        bool hasTitle = !string.IsNullOrEmpty(title);
        Debug.Log("hasTitle: "+hasTitle);
        titleField.text = title;
        headerArea.gameObject.SetActive(hasTitle);

        // hide the image if there's no image
        bool hasImage = imageToShow != null;
        Debug.Log("hasImage: "+hasImage);
        horizontalLayoutImage.sprite = imageToShow;
        horizontalImageContainer.gameObject.SetActive(hasImage);

        horizontalLayoutText.text = message;

        onConfirmAction = confirmAction;
        confirmBtnMsg.text = confirmMsg;

        bool hasDeclineOption = declineAction != null;
        declineBtn.gameObject.SetActive(hasDeclineOption);
        onDeclineAction = declineAction;
        declineBtnMsg.text = declineMsg;

        bool hasAlternateOption = alternateAction != null;
        alternateBtn.gameObject.SetActive(hasAlternateOption);
        onAlternateAction = alternateAction;
        alternateBtnMsg.text = alternateMsg;

        horizontalLayoutArea.gameObject.SetActive(true);
    }

    public void ShowModalHorizontally(string title, Sprite imageToShow, string message, string confirmMsg, Action confirmAction) {
        ShowModalAsHorizontalLayout(title, imageToShow, message, confirmMsg, confirmAction);
    }

    public void ShowModalHorizontally(string title, Sprite imageToShow, string message, string confirmMsg, Action confirmAction, string declineMsg, Action declineAction) {
        ShowModalAsHorizontalLayout(title, imageToShow, message, confirmMsg, confirmAction);
    }

    public void ShowModalHorizontally(string title, Sprite imageToShow, string message, string confirmMsg, Action confirmAction, string declineMsg, Action declineAction, string alternateMsg, Action alternateAction) {
        ShowModalAsHorizontalLayout(title, imageToShow, message, confirmMsg, confirmAction);
    }

    #endregion


    #region Vertical

    private void ShowModalAsVerticalLayout(string title, Sprite imageToShow, string message, string confirmMsg, Action confirmAction, string declineMsg = "", Action declineAction = null, string alternateMsg = "", Action alternateAction = null) {
        gameObject.SetActive(true);
        horizontalLayoutArea.gameObject.SetActive(false);

        // hide the header if there's no title
        bool hasTitle = !string.IsNullOrEmpty(title);
        Debug.Log("hasTitle: "+hasTitle);
        titleField.text = title;
        headerArea.gameObject.SetActive(hasTitle);

        // hide the image if there's no image
        bool hasImage = imageToShow != null;
        Debug.Log("hasImage: "+hasImage);
        verticalLayoutImage.sprite = imageToShow;
        verticalLayoutImage.gameObject.SetActive(hasImage);

        verticalLayoutText.text = message;

        onConfirmAction = confirmAction;
        confirmBtnMsg.text = confirmMsg;

        bool hasDeclineOption = declineAction != null;
        declineBtn.gameObject.SetActive(hasDeclineOption);
        onDeclineAction = declineAction;
        declineBtnMsg.text = declineMsg;

        bool hasAlternateOption = alternateAction != null;
        alternateBtn.gameObject.SetActive(hasAlternateOption);
        onAlternateAction = alternateAction;
        alternateBtnMsg.text = alternateMsg;

        verticalLayoutArea.gameObject.SetActive(true);
    }

    public void ShowModalVertically(string title, Sprite imageToShow, string message, string confirmMsg, Action confirmAction) {
        ShowModalAsVerticalLayout(title, imageToShow, message, confirmMsg, confirmAction);
    }

    public void ShowModalVertically(string title, Sprite imageToShow, string message, string confirmMsg, Action confirmAction, string declineMsg, Action declineAction) {
        ShowModalAsVerticalLayout(title, imageToShow, message, confirmMsg, confirmAction, declineMsg, declineAction);
    }

    public void ShowModalVertically(string title, Sprite imageToShow, string message, string confirmMsg, Action confirmAction, string declineMsg, Action declineAction, string alternateMsg, Action alternateAction) {
        ShowModalAsVerticalLayout(title, imageToShow, message, confirmMsg, confirmAction, declineMsg, declineAction, alternateMsg, alternateAction);
    }

    #endregion


}
