using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading_Transition : Scene_Transition
{
    [Header("Loading Controls")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progessBar;
    [SerializeField] private Text progessText;


    protected override IEnumerator LoadSceneAsyncProcess(bool allowSceneActive) {
        loadingScreen.SetActive(true);

        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneIndexToLoad);
        oper.allowSceneActivation = allowSceneActive;
        while(!oper.isDone) {
            float progessValue = Mathf.Clamp01(oper.progress / 0.9f);

            progessBar.value = progessValue;
            progessText.text = progessValue * 100f + "%";

            yield return null;
        }
    }
}
