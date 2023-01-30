using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private GameObject loginUI;
    [SerializeField] private GameObject registerUI;
    [SerializeField] private GameObject verifyEmailUI;
    [SerializeField] private GameObject verifyCodeUI;

    
    public void ShowLoginBox() {
        UIController.instance.ClearUI();
        loginUI.SetActive(true);
    }

    public void ShowRegisterBox() {
        UIController.instance.ClearUI();
        registerUI.SetActive(true);
    }

    public void ShowVerifyEmailBox() {
        UIController.instance.ClearUI();
        verifyEmailUI.SetActive(true);
    }

    public void ShowVerifyCodeBox() {
        UIController.instance.ClearUI();
        verifyCodeUI.SetActive(true);
    }
}
