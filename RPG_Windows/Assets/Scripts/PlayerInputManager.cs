using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    private PlayerInput input;

    /// <summary>
    /// 可操作角色的使用者輸入
    /// </summary>
    protected AvatarInputActionsControls m_inputControls;
    public AvatarInputActionsControls InputCtrl => this.m_inputControls;

    [SerializeField] protected Constant.ControlDevice controlDevice;
    public Constant.ControlDevice ControlDevice => this.controlDevice;

    private void Awake() {
        if(instance == null) 
        {
            instance = this;
        }

        input = GetComponent<PlayerInput>();
        m_inputControls = new AvatarInputActionsControls();
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
        
    }

    // Update is called once per frame
    void Update()
    {
        OnDeviceChange(input);
    }

    public void OnDeviceChange(PlayerInput input) {
        //Debug.Log("OnDeviceChange: "+input.currentControlScheme);
        switch(input.currentControlScheme) {
            case "Keyboard & Mouse":
                controlDevice = Constant.ControlDevice.KeyboardMouse;
                break;
            // case "Mobile":
            //     controlDevice = Constant.ControlDevice.Mobile;
            //     break;
            default:
                controlDevice = Constant.ControlDevice.Gamepad;
                break;
        }
    }
}
