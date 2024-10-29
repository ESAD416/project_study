using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    private PlayerInput input;
    
    protected AvatarInputActionsControls m_inputControls;
    /// <summary>
    /// 預設的可操作角色InputActionAsset
    /// </summary>
    public AvatarInputActionsControls InputCtrl => this.m_inputControls;
    

    [SerializeField] private Constant.ControlDevice controlDevice;
    public Constant.ControlDevice ControlDevice => this.controlDevice;

    [Header("Touch")]
    [SerializeField] private Canvas touchCanvas;

    private void Awake() {
        if(instance == null) 
        {
            instance = this;
        }

        input = GetComponent<PlayerInput>();
        m_inputControls = new AvatarInputActionsControls();

        StartCoroutine(ControlDeviceCheck());
    }

    private void OnEnable() {
        m_inputControls.Enable();
    }

    private void OnDisable() {
        m_inputControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("actionMaps counts: "+m_inputControls.asset.actionMaps.Count);

        m_inputControls.Lamniat_Land.Menu.performed += content => {
            MenuManager.instance.OnMenuCallback.AddListener(EnableUIAction);
            MenuManager.instance.OnActionCallback.AddListener(EnableLamniatLandAction);
            MenuManager.instance.OnMenu();
        };

        m_inputControls.UI.Menu.performed += content => {
            // if(MenuManager.instance.GameIsPaused) MenuManager.instance.OnAction();
            if(MenuManager.instance.OnMenuPanel) MenuManager.instance.OnAction();
        };
        m_inputControls.UI.Cancel.performed += content => {
            // if(MenuManager.instance.GameIsPaused && MenuManager.instance.OnMenuPanel) MenuManager.instance.OnAction();
            if(MenuManager.instance.OnMenuPanel) MenuManager.instance.OnAction();
        };
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("currentControlScheme: "+input.currentControlScheme);

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = touchPosition
            };

            // 檢查是否點擊到 UI 元素
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                Debug.Log("results.Count "+results.Count);
                // 如果有 UI 元素，進行點擊觸發
                foreach (var result in results)
                {
                    ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
                }
            }
        }

        if(controlDevice.Equals(Constant.ControlDevice.TouchScreen)) {
            //touchCanvas.gameObject.SetActive(true);
        } else {
            //touchCanvas.gameObject.SetActive(false);
        }
    }

    IEnumerator ControlDeviceCheck() {
        while(true) {
            // 檢查是否啟用了模擬觸控
            bool isTouchable = CheckAvaliableToTouch();
            if(isTouchable) {
                controlDevice = Constant.ControlDevice.TouchScreen;
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            


            //Debug.Log("currentControlScheme: "+input.currentControlScheme);
            switch(input.currentControlScheme) {
                case "Keyboard&Mouse":
                    controlDevice = Constant.ControlDevice.KeyboardMouse;
                    break;
                case "Touch":
                    controlDevice = Constant.ControlDevice.TouchScreen;
                    break;
                default:
                    controlDevice = Constant.ControlDevice.Gamepad;
                    break;
            }

            yield return new WaitForSeconds(0.5f);
        }
        //Debug.Log("OnDeviceChange: "+input.currentControlScheme);
    }

    public void EnableLamniatLandAction() {
        foreach (var actionMap in m_inputControls.asset.actionMaps) {
            actionMap.Disable();
        }
        m_inputControls.Lamniat_Land.Enable();
    }    

    public void EnableUIAction() {
        foreach (var actionMap in m_inputControls.asset.actionMaps) {
            actionMap.Disable();
        }
        m_inputControls.UI.Enable();

    }

    private bool CheckAvaliableToTouch() {
        // 檢測是否為模擬觸控或真實觸控環境
        bool isSimulatedTouchEnabled = false;
        foreach (var device in InputSystem.devices)
        {
            if (device.description.interfaceName == "SimulatedTouchscreen")
            {
                isSimulatedTouchEnabled = true;
                break;
            }
        }

        bool isTouchDevice = Touchscreen.current != null;
        if (isSimulatedTouchEnabled || isTouchDevice)
        {
            return true;
        }

        return false;
    }
}
