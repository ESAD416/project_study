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
    [SerializeField] private Text progessPctText;
    [SerializeField] private Text descrpText;
    [SerializeField] private Button transBtn;

    private void Awake() {
        transBtn.gameObject.SetActive(false);
        loadingScreen.SetActive(false);
    }


    protected override IEnumerator LoadSceneAsyncProcess(bool allowSceneActive) {
        loadingScreen.SetActive(true);

        asyncLoad = SceneManager.LoadSceneAsync(sceneIndexToLoad);
        asyncLoad.allowSceneActivation = allowSceneActive;
        while(!asyncLoad.isDone) {
            float progessValue = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            progessBar.value = progessValue;
            progessPctText.text = progessValue * 100f + "%";

            // Check if the load has finished
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("asyncLoad process is finish");
                transBtn.gameObject.SetActive(true);
                descrpText.text = "點擊螢幕後繼續";
            }

            yield return new WaitForEndOfFrame();
        }

        
    }

    public void AllowSceneTransition() {
        asyncLoad.allowSceneActivation = true;
    }

    
}
