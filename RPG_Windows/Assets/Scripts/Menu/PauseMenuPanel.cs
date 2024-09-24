using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenuPanel : MonoBehaviour
{
    public bool OnPausePanel = false;
    public UnityEvent OnInitCallback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable() {
        OnInitCallback.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause() {
        Time.timeScale = 0f;
        MenuManager.instance.GameIsPaused = true;
        MenuManager.instance.OnMenuPanel = true;
        OnPausePanel = true;
    }

    public void Resume() {
        Time.timeScale = 1f;
        MenuManager.instance.GameIsPaused = false;
        MenuManager.instance.OnMenuPanel = false;
        OnPausePanel = false;

        // DeactivateAllCanvas();
        // RemoveAllEventCallbacks();
        // SetSelectedGameObject(null);
    }

}
