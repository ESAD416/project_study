using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading_Transition : Scene_Transition
{
    protected override IEnumerator LoadSceneAsyncProcess(bool allowSceneActive) {
        UIController.instance.LoadingPanel.gameObject.SetActive(true);

        asyncLoad = SceneManager.LoadSceneAsync(sceneIndexToLoad);
        asyncLoad.allowSceneActivation = allowSceneActive;

        while(!asyncLoad.isDone) {
            UIController.instance.LoadingPanel.UpdateProgessBar(asyncLoad);
            yield return new WaitForEndOfFrame();
        }
    }

    public void AllowSceneTransition() {
        asyncLoad.allowSceneActivation = true;
    }

    
}
