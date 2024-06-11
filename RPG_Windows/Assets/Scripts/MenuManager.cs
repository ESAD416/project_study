using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] protected PlayerInputManager m_inputManager;

    #region 暫停選單物件
    
    [SerializeField] protected GameObject m_pauseMenuPanel;
    [SerializeField] protected GameObject m_pauseFirstOption;
    public bool GameIsPaused = false;
    
    #endregion

    private void Awake() {
        if(instance == null) 
        {
            instance = this;
        }
    }

    private void OnEnable() {
    }

    private void OnDisable() {
    }

    // Start is called before the first frame update
    void Start()
    {
        m_pauseMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region Pause/Resume Method

    public void Pause() {
        Time.timeScale = 0f;
        GameIsPaused = true;

        ActivateCanvas(m_pauseMenuPanel);
        EventSystem.current.SetSelectedGameObject(m_pauseFirstOption);
    }

    public void Resume() {
        Time.timeScale = 1f;
        GameIsPaused = false;

        DeactivateAllCanvas();
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
        
        EventSystem.current.SetSelectedGameObject(null);
    }

    #endregion
    
    #region 



    #endregion

    public void QuitGame() {

    }
}
