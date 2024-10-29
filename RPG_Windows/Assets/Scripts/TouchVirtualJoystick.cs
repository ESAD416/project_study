using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class TouchVirtualJoystick : MonoBehaviour
{

    [SerializeField] protected Vector2 joystickSize = new Vector2(150, 150);
    public RectTransform Background;  // 摇杆背景
    public RectTransform Handle;      // 摇杆手柄

    private Finger movementFinger;  // 用于移动摇杆的手指
    [SerializeField] private Vector2 movementAmount;              // 输入向量
    [SerializeField] private Movement_Lamniat targetMovement;

    [SerializeField] private float joystickRadius;             // 摇杆背景的半径

    // private Vector2 startTouchPosition;       // 初始点击位置
    // private bool isDragging = false;          // 是否正在拖动

    // public Vector2 InputDirection => inputVector;  // 外部访问方向

    // Start is called before the first frame update
    void Start()
    {
        // 获取摇杆背景的初始位置和半径
        joystickRadius = gameObject.GetComponent<RectTransform>().sizeDelta.x / 2f;
        Background.gameObject.SetActive(false); // 初始时隐藏摇杆
    }

    private void OnEnable() {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerUp;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable() {
        Debug.Log("OnDisable EnhancedTouchSupport");
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerUp;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (IsPointerOverUI(touchedFinger)) {
            // 如果是在UI上點擊，則不處理
            Debug.Log("Touch on UI");
            EnhancedTouchSupport.Disable();
            return;
        }

        if(movementFinger == null) {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            Background.gameObject.SetActive(true);
            gameObject.GetComponent<RectTransform>().sizeDelta = joystickSize;

            //Vector3 position = Camera.main.ScreenToWorldPoint(ClampStartPosition(touchedFinger.screenPosition));
            gameObject.GetComponent<RectTransform>().anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
        }
        
    }

    private void HandleFingerUp(Finger looseFinger) 
    {
        if(looseFinger == movementFinger) {
            movementFinger = null;
            Handle.anchoredPosition = Vector2.zero;
            movementAmount = Vector2.zero; 
            targetMovement.CancelMovement();

            Background.gameObject.SetActive(false);         // 隐藏摇杆
        }
    }

    private void HandleFingerMove(Finger moveFinger) {
        if(moveFinger == movementFinger) {
            ETouch.Touch currentTouch = moveFinger.currentTouch;

            Vector2 dragDirection = currentTouch.screenPosition - gameObject.GetComponent<RectTransform>().anchoredPosition;

            Handle.anchoredPosition = Vector2.ClampMagnitude(dragDirection, joystickRadius);
            movementAmount = dragDirection.normalized;

            targetMovement.PerformMovement(movementAmount);
        }
    }

    private bool IsPointerOverUI(Finger touchedFinger) {
        PointerEventData eventData = new PointerEventData(EventSystem.current) {
            position = touchedFinger.screenPosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    private Vector2 ClampStartPosition(Vector2 startPosition) {
        Debug.Log("Start Position: " + startPosition);

        Vector3 viewportPoint = Camera.main.ScreenToViewportPoint(startPosition);
        Debug.Log("viewportPoint Position: " + startPosition);

        // 限制X和Y使其保持在[0, 1]範圍內
        viewportPoint.x = Mathf.Clamp(viewportPoint.x, joystickSize.x / (2 * Screen.width), 1 - joystickSize.x / (2 * Screen.width));
        viewportPoint.y = Mathf.Clamp(viewportPoint.y, joystickSize.y / (2 * Screen.height), 1 - joystickSize.y / (2 * Screen.height));

        // 將Viewport座標轉換回螢幕座標
        Vector2 clampedPosition = Camera.main.ViewportToScreenPoint(viewportPoint);

        Debug.Log("Clamped Position: " + clampedPosition);
        return clampedPosition;
    }

}
