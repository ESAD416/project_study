using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
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
    }

    IEnumerator ControlDeviceCheck() {
        while(true) {
            switch(input.currentControlScheme) {
                case "Keyboard&Mouse":
                    controlDevice = Constant.ControlDevice.KeyboardMouse;
                    break;
                // case "Mobile":
                //     controlDevice = Constant.ControlDevice.Mobile;
                //     break;
                default:
                    controlDevice = Constant.ControlDevice.Gamepad;
                    break;
            }

            yield return new WaitForSeconds(1.5f);
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
}
