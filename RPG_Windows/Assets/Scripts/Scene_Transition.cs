using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Transition : MonoBehaviour
{
    public int sceneIndexToLoad;
    public string sceneNameToLoad;
    public TransitionStorage infoStorage;
    public MsgDialogCtrl dialogCtrl;
    protected AsyncOperation asyncLoad;

    [Header("Player Controls")]
    public Vector2 playerPos ;
    public string jumpCollidersName;
    public PlayerStorage playerStorage;

    [Header("Animation Controls")]
    public Animator transitionAnimaCtrl;
    public float transitionDelay = 1f;

    public virtual void LoadScene() {
        try {
            StartCoroutine(LoadSceneProcess());
        } catch(Exception ex) {
            Debug.Log("Exception: "+ex);
            string errorTitle = "發生錯誤";
            string errorContent = ex.Message;
            string confirmBtnStr = "重新讀取";
            string cancelBtnStr = "離開遊戲";
            dialogCtrl.InitialDialogBox(errorTitle, errorContent, confirmBtnStr, cancelBtnStr);
        }
    }

    public virtual void ReloadScene() {
        StopAllCoroutines();
        LoadScene();
    }

    public virtual void LoadSceneAsync(bool allowSceneActive) {
        try {
            StartCoroutine(LoadSceneAsyncProcess(allowSceneActive));
        } catch(Exception ex) {
            Debug.Log("Exception: "+ex);
            string errorTitle = "發生錯誤";
            string errorContent = ex.Message;
            string confirmBtnStr = "重新讀取";
            string cancelBtnStr = "離開遊戲";
            dialogCtrl.InitialDialogBox(errorTitle, errorContent, confirmBtnStr, cancelBtnStr);
        }
    }

    public virtual void ReloadSceneAsynce(bool allowSceneActive) {
        StopAllCoroutines();
        LoadSceneAsync(allowSceneActive);
    }

    protected virtual IEnumerator LoadSceneProcess() {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndexToLoad);
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
}
