using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
    public UnityEvent OnPlayClicked, OnOptionClicked, OnQuitClicked;

    public void QuitGame() {
        Debug.Log("QUIT GAME");
        Application.Quit();
    }
}
