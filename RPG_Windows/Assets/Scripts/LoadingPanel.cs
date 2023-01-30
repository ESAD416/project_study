using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private Slider progessBar;
    [SerializeField] private Text progessPctText;
    [SerializeField] private Text descrpText;
    [SerializeField] private Button transBtn;

    private void Awake() {
        transBtn.gameObject.SetActive(false);
    }

    public void UpdateProgessBar(AsyncOperation asyncLoad) {
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
    }
}
