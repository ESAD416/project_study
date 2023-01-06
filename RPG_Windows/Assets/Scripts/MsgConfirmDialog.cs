using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgConfirmDialog : MonoBehaviour
{
    public GameObject dialogBox;
    public Text titleTxt;
    public Text contentTxt;
    public Text confirmBtnTxt;
    public Text cancelBtnTxt;

    public void InitialDialogBox(string titleStr, string contentStr, string confirmBtnStr, string cancelBtnStr) {
        titleTxt.text = titleStr;
        contentTxt.text = contentStr;
        confirmBtnTxt.text = confirmBtnStr;
        cancelBtnTxt.text = cancelBtnStr;
        dialogBox.SetActive(true);
    }

}
