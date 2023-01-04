using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Threading.Tasks;

public class Loading_Transition : Scene_Transition
{
    [Header("Loading Controls")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progessBar;
    [SerializeField] private TMP_Text progessText;


    protected override IEnumerator LoadSceneAsyncProcess() {
        loadingScreen.SetActive(true);

        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneIndexToLoad);
        oper.allowSceneActivation = false;
        while(!oper.isDone) {
            float progessValue = Mathf.Clamp01(oper.progress / 0.9f);

            progessBar.value = progessValue;
            progessText.text = progessValue * 100f + "%";

            yield return null;
        }
    }
}
