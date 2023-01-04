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

    [Header("Player Controls")]
    public Vector2 playerPos ;
    public string jumpCollidersName;
    public PlayerStorage playerStorage;

    [Header("Animation Controls")]
    public Animator transitionAnimaCtrl;
    public float transitionDelay = 1f;

    public virtual void LoadScene() {
        StartCoroutine(LoadSceneProcess());
    }

    public virtual void LoadSceneAsync() {
        StartCoroutine(LoadSceneAsyncProcess());
    }

    protected virtual IEnumerator LoadSceneProcess() {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndexToLoad);
    }

    protected virtual IEnumerator LoadSceneAsyncProcess() {
        yield return new WaitForSeconds(transitionDelay);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndexToLoad);
        asyncLoad.allowSceneActivation = false;
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
