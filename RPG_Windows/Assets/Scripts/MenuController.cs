using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    public MsgConfirmDialog dialog;

    public void OnPlayClicked() {
        Debug.Log("OnPlayClicked");
    }

    public void OnOptionClicked() {
        Debug.Log("OnOptionClicked");
    }

    public void OnQuitClicked() {
        Debug.Log("OnQuitClicked");
        Application.Quit();
    }
}
