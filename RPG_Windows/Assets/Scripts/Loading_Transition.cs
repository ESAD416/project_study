using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading_Transition : MonoBehaviour
{
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image progessBar;

    public void LoadScene(int sceneIndex) {
        StartCoroutine(LoadSceneAsyncProcess(sceneIndex));
    }

    IEnumerator LoadSceneAsyncProcess(int sceneIndex) {
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneIndex);
        loadingCanvas.SetActive(true);
        while(!oper.isDone) {
            float progessValue = Mathf.Clamp01(oper.progress / 0.9f);
            progessBar.fillAmount = progessValue;
            yield return null;
        }
    }
}
