using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Scene_Transition : MonoBehaviour
{
    public int sceneIndexToLoad;
    public string sceneNameToLoad;
    public TransitionStorage transInfoStorage;
    protected AsyncOperation asyncLoad;

    [Header("Destination Player Controls")]
    public Vector2 destinationPlayerPos;
    public float destinationPlayerHeight;
    public string destinationJumpCollidersName;
    public PlayerStorage destinationPlayerStorage;

    [Header("Transition Animation Controls")]
    public Animator transitionAnimaCtrl;
    public float transitionDelay = 1f;

    #region Load Scene

    public virtual void LoadScene() {
        try {
            StartCoroutine(LoadSceneProcess());
        } catch(Exception ex) {
            Debug.Log("Exception: "+ex);
            string errorTitle = "發生錯誤";
            string errorContent = ex.Message;
            string confirmBtnStr = "重新讀取";
            string cancelBtnStr = "離開遊戲";

            UnityEvent confirmAction = new UnityEvent();
            confirmAction.AddListener(ReloadScene);
            confirmAction.AddListener(UIController.instance.ModalPanel.DeactivateModelPanel);

            UnityEvent declineAction = new UnityEvent();
            declineAction.AddListener(UIController.instance.ModalPanel.DeactivateModelPanel);

            UIController.instance.ModalPanel.ShowModalVertically(errorTitle, null, errorContent, confirmBtnStr, confirmAction.Invoke, cancelBtnStr, declineAction.Invoke);
            //dialogCtrl.InitialDialogBox(errorTitle, errorContent, confirmBtnStr, cancelBtnStr);
        }
    }

    public virtual void ReloadScene() {
        Debug.Log("ReloadScene");
        StopAllCoroutines();
        LoadScene();
    }

    
    protected virtual IEnumerator LoadSceneProcess() {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndexToLoad);
    }

    #endregion

    #region Async Load Scene

    public virtual void LoadSceneAsync(bool allowSceneActive) {
        try {
            StartCoroutine(LoadSceneAsyncProcess(allowSceneActive));
        } catch(Exception ex) {
            Debug.Log("Exception: "+ex);
            string errorTitle = "發生錯誤";
            string errorContent = ex.Message;
            string confirmBtnStr = "重新讀取";
            string cancelBtnStr = "離開遊戲";

            UnityEvent confirmAction = new UnityEvent();
            confirmAction.AddListener(delegate { ReloadSceneAsync(false); });
            confirmAction.AddListener(UIController.instance.ModalPanel.DeactivateModelPanel);

            UnityEvent declineAction = new UnityEvent();
            declineAction.AddListener(UIController.instance.ModalPanel.DeactivateModelPanel);

            UIController.instance.ModalPanel.ShowModalVertically(errorTitle, null, errorContent, confirmBtnStr, confirmAction.Invoke, cancelBtnStr, declineAction.Invoke);
            //dialogCtrl.InitialDialogBox(errorTitle, errorContent, confirmBtnStr, cancelBtnStr);
        }
    }

    public virtual void ReloadSceneAsync(bool allowSceneActive) {
        Debug.Log("ReloadSceneAsync");
        StopAllCoroutines();
        LoadSceneAsync(allowSceneActive);
    }

    protected virtual IEnumerator LoadSceneAsyncProcess(bool allowSceneActive) {
        yield return new WaitForSeconds(transitionDelay);
        asyncLoad = SceneManager.LoadSceneAsync(sceneIndexToLoad);
        asyncLoad.allowSceneActivation = allowSceneActive;
        while (!asyncLoad.isDone)
        {
            // Check if the load has finished
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }

    #endregion

    public void TextError() {
        Debug.Log("Exception ");
        string errorTitle = "發生錯誤";
        string errorContent = "Exception";
        string confirmBtnStr = "重新讀取";
        string cancelBtnStr = "離開遊戲";

        UnityEvent confirmAction = new UnityEvent();
        confirmAction.AddListener(delegate { ReloadSceneAsync(false); });
        confirmAction.AddListener(UIController.instance.ModalPanel.DeactivateModelPanel);
        
        UnityEvent declineAction = new UnityEvent();
        declineAction.AddListener(UIController.instance.ModalPanel.DeactivateModelPanel);

        UIController.instance.ModalPanel.ShowModalVertically(errorTitle, null, errorContent, confirmBtnStr, confirmAction.Invoke, cancelBtnStr, declineAction.Invoke);
    }

}
