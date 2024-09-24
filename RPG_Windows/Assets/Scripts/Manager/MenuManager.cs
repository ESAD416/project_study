using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] protected List<GameObject> panelCanvas = new List<GameObject>();
    public bool GameIsPaused = false;
    public bool OnMenuPanel = false;
    public UnityEvent OnMenuCallback, OnActionCallback;

    private void Awake() {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable() {
        DeactivateAllCanvas();
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
    
    public void OnMenu() {
        OnMenuCallback.Invoke();
        
    }

    public void OnAction() {
        OnActionCallback.Invoke();
    }

    #region Canvas Actions 

    public void ActivateCanvas(GameObject canvas) {
        DeactivateAllCanvas();
        canvas.SetActive(true);
    }

    public void DeactivateAllCanvas() {
        foreach(GameObject canvas in panelCanvas)
            canvas.SetActive(false);

        // cancel select button
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetSelectedGameObject(GameObject gameObject) {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    #endregion
    
    #region Event Callbacks Remove

    public void RemoveAllEventCallbacks() {
        // TODO 預計還會有更多的事件
        OnMenuCallback.RemoveAllListeners();
        OnActionCallback.RemoveAllListeners();
    }


    #endregion

    public void QuitGame() {

    }
}
