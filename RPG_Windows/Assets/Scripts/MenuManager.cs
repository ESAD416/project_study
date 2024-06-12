using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    #region 暫停選單物件
    
    [SerializeField] protected GameObject m_pauseMenuPanel;
    [SerializeField] protected GameObject m_pauseFirstOption;
    public bool GameIsPaused = false;
    public UnityEvent OnPauseCallback, OnResumeCallback;
    
    #endregion

    private void Awake() {
        if(instance == null) 
        {
            instance = this;
        }
    }

    private void OnEnable() {
        m_pauseMenuPanel.SetActive(false);
    }

    private void OnDisable() {
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region Pause/Resume Method

    public void Pause() {
        OnPauseCallback.Invoke();

        Time.timeScale = 0f;
        GameIsPaused = true;

        ActivateCanvas(m_pauseMenuPanel);
        EventSystem.current.SetSelectedGameObject(m_pauseFirstOption);
    }

    public void Resume() {
        Debug.Log("Resume");
        OnResumeCallback.Invoke();

        Time.timeScale = 1f;
        GameIsPaused = false;

        DeactivateAllCanvas();
        EventSystem.current.SetSelectedGameObject(null);
        OnResumeCallback.RemoveAllListeners();
    }

    #endregion

    #region Canvas Activations/Deactivations

    public void ActivateCanvas(GameObject canvas) {
        DeactivateAllCanvas();
        canvas.SetActive(true);
    }

    public void DeactivateAllCanvas() {
        // TODO 預計還會有更多的選單
        m_pauseMenuPanel.SetActive(false);
    }

    #endregion
    
    #region 



    #endregion

    public void QuitGame() {

    }
}