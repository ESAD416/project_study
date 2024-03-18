using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DynamicJump_Lamniat : MonoBehaviour
{
    [Header("DynamicJump_Lamniat 基本物件")]
    [SerializeField] protected Avatar_Lamniat m_Lamniat;
    [SerializeField] protected HeightManager m_HeightManager;
    [SerializeField] protected Movement_Lamniat m_LamniatMovement;

    [Header("DynamicJump_Lamniat 基本參數")]
    public bool IsJumping;
    public bool CanJump;
    private bool HasUpStep = false;
    private bool HasDownStep = false;
    public bool OnHeightObjCollisionExit;


    /// <summary>
    /// 角色跳躍歷經時間
    /// </summary>
    public float m_jumpingTimeElapsed = 0f;
    /// <summary>
    /// 角色跳躍歷經禎數
    /// </summary>
    public int JumpCounter = 0;
    public int HeightIncreaseCount = 10;
    public int JumpFallingCount = 25;
    public int HeightDecreaseCount = 40;
    public int HeightDecreaseCountExponential = 10;


    public int HeightChangeCount;
    private float[] jumpOffset = new float[] 
    {
        0.0f, 0.1f,     //5
        0.2f,      //10
        0.3f,     //15
        0.4f,     //20
        0.5f, 0.6f,     //25
        0.7f,     //30
        0.8f,     //35
        0.9f,     //40
        
        1.0f, 1.067f, 1.133f, 1.2f, 1.267f,     //45
        1.333f, 1.4f, 1.467f, 1.533f, 1.6f,     //50
        1.667f, 1.733f, 1.8f, 1.867f, 1.933f,     //55
        2.0f, 1.933f, 1.867f, 1.8f, 1.733f,     //60
        1.667f, 1.6f, 1.533f, 1.467f, 1.4f,     //65
        1.333f, 1.267f, 1.2f, 1.133f, 1.067f,     //70

        1.0f, 0.897f,     //75
        0.793f, 0.69f,     //80
        0.586f,     //85
        0.483f, 0.379f,     //90
        0.276f,     //95
        0.138f, 0.0f     //100
    };

    void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 100; // FPS鎖幀
        Debug.Log("targetFrameRate: "+Application.targetFrameRate);
        Debug.Log("vSyncCount: "+QualitySettings.vSyncCount);
    }
    void Start() {
        InvokeRepeating("SetParameterByFPS", 1, 1);
    }

    void Update() {
        // 不在跳躍狀態就不執行
        if (!IsJumping) {
            return;
        }

        // TODO 移動到Animator實作
        // float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        // if (normalizedTime > 0.8f) // 当动画播放到0.8秒时
        // {
        //     animator.Play("Jumping State", 0, 0.2f); // 从0.2秒的位置重新开始播放
        // }

        // 調整跳躍level
        UpdateLamniatJumping();
        //UpdateLamniatJumpingVer2();
    }

    private void SetParameterByFPS() {
        var fps = 1f / Time.unscaledDeltaTime;
        
        // if(fps < 89f) {
        //     // 減半
        //     HeightIncreaseCount = 5; 
        //     JumpFallingCount = 13;
        //     HeightDecreaseCount = 20;
        //     HeightDecreaseCountExponential = 5;

        //     jumpOffset = new float[] 
        //     {
        //         0.0f,     //5
        //         0.2f,      //10
        //         0.4f,     //20
        //         0.6f,     //25
        //         0.8f,     //35
                
        //         1.0f, 1.1f, 1.2335f,
        //         1.3665f, 1.5f, 1.6335f, 
        //         1.7665f, 1.9f,
        //         2.0f, 
        //         1.9f, 1.7665f, 1.6335f, 1.5f,
        //         1.3665f, 1.2335f, 1.1f,     

        //         1.0f,     //75
        //         0.793f,     //80
        //         0.586f,     //85
        //         0.379f,     //90
        //         0.276f,     //95
        //         0.0f     //100
        //     };
        // } else {
        //     HeightIncreaseCount = 10;
        //     JumpFallingCount = 25;
        //     HeightDecreaseCount = 40;
        //     HeightDecreaseCountExponential = 10;

        //     jumpOffset = new float[] 
        //     {
        //         0.0f, 0.1f,     //5
        //         0.2f,      //10
        //         0.3f,     //15
        //         0.4f,     //20
        //         0.5f, 0.6f,     //25
        //         0.7f,     //30
        //         0.8f,     //35
        //         0.9f,     //40
                
        //         1.0f, 
        //         1.067f, 1.133f, 1.2f, 1.267f,     //45
        //         1.333f, 1.4f, 1.467f, 1.533f, 1.6f,     //50
        //         1.667f, 1.733f, 1.8f, 1.867f, 1.933f,     //55
        //         2.0f, 
        //         1.933f, 1.867f, 1.8f, 1.733f,     //60
        //         1.667f, 1.6f, 1.533f, 1.467f, 1.4f,     //65
        //         1.333f, 1.267f, 1.2f, 1.133f, 1.067f,     //70

        //         1.0f, 0.897f,     //75
        //         0.793f, 0.69f,     //80
        //         0.586f,     //85
        //         0.483f, 0.379f,     //90
        //         0.276f,     //95
        //         0.138f, 0.0f     //100
        //     };
        // }

    }

    

    #region 碰撞偵測

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰到了Collider，执行相应的逻辑
        //Debug.Log("collision Enter: "+collision.gameObject.name);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("collision : "+collision.gameObject.name);
        // 碰到了Collider，执行相应的逻辑
        if (collision.gameObject.CompareTag("Map_Collision") && IsJumping)
        {
            OnHeightObjCollisionExit = true;
        } 

    }

    #endregion

    #region 觸發事件

    void OnTriggerEnter2D(Collider2D trigger)
    {
        Debug.Log("trigger Enter: "+trigger.gameObject.name);
        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        int mapRangeLayerMask = 1 << LayerMask.NameToLayer("MapRange");
        int layerMask = ~(playerLayerMask | mapRangeLayerMask);

        Vector2 start = (Vector2)transform.position + m_Lamniat.BodyCollider.offset ;  // 射线的起点
        RaycastHit2D hit = Physics2D.Raycast(start, m_Lamniat.AvatarMovement.Movement.normalized, 1f, layerMask);
        Color color = hit.collider != null ? Color.red : Color.green;
        Debug.DrawLine(start, start + m_Lamniat.AvatarMovement.Movement.normalized*1f, color);
        //print(hit.collider);
        //Debug.Log("hit.collider name: "+hit.collider.gameObject.name);
        if (hit.collider == null || hit.collider.gameObject.name != trigger.gameObject.name) {
            return;
        }

        //Debug.Log("trigger tag: "+trigger.gameObject.tag);
        // Debug.Log("CanJump: "+CanJump);
        if (trigger.gameObject.CompareTag("Map_Jump_Trigger") && CanJump)
        {
            // Debug.Log("trigger Enter jump start");
            // 触发跳跃逻辑
            m_Lamniat.SetCurrentBaseState(m_Lamniat.Jump);
            IsJumping = true;
            CanJump = false;
            //moveDuration = 0f;
            JumpCounter = 0;
            HeightChangeCount = 0;
            // 記錄最一開始的跳躍

            // 一旦跳躍，Trigger就關掉
            // for (int i = 0; i < tilemapTriggerArray.Length; i++)
            // {
            //     int triggerLevel = i / triggersPerLevel;
            //     tilemapTriggerArray[i].enabled = false;
            // } 
        }

    }

    void OnTriggerStay2D(Collider2D trigger)
    {
        Debug.Log("trigger stay: "+trigger.gameObject.name);
        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        int mapRangeLayerMask = 1 << LayerMask.NameToLayer("MapRange");
        int layerMask = ~(playerLayerMask | mapRangeLayerMask);

        Vector2 start = (Vector2)transform.position + m_Lamniat.BodyCollider.offset ;  // 射线的起点
        RaycastHit2D hit = Physics2D.Raycast(start, m_Lamniat.AvatarMovement.Movement.normalized, 1f, layerMask);
        Color color = hit.collider != null ? Color.red : Color.green;
        Debug.DrawLine(start, start + m_Lamniat.AvatarMovement.Movement.normalized*1f, color);
        // Debug.Log("hit.collider name: "+hit.collider.gameObject.name);
        if (hit.collider == null || hit.collider.gameObject.name != trigger.gameObject.name) {
            return;
        }

        //Debug.Log("trigger tag: "+trigger.gameObject.tag);
        // Debug.Log("CanJump: "+CanJump);
        if (trigger.gameObject.CompareTag("Map_Jump_Trigger") && CanJump)
        {
            // Debug.Log("trigger stay jump start");
            // 触发跳跃逻辑
            m_Lamniat.SetCurrentBaseState(m_Lamniat.Jump);
            IsJumping = true;
            CanJump = false;
            //moveDuration = 0f;
            JumpCounter = 0;
            HeightChangeCount = 0;
            // 記錄最一開始的跳躍

            // 一旦跳躍，Trigger就關掉
            // for (int i = 0; i < tilemapTriggerArray.Length; i++)
            // {
            //     int triggerLevel = i / triggersPerLevel;
            //     tilemapTriggerArray[i].enabled = false;
            // }
        }

    }
    #endregion

    private void UpdateLamniatJumping() {
        if (JumpCounter < HeightIncreaseCount)
        {
            // 跳跃上升阶段
            // level = 0; // 虚拟高度保持在0
            FixJumpCornersWhileDownStep();
            m_Lamniat.SprtRenderer.transform.localPosition = new Vector2(0, jumpOffset[JumpCounter]);
        }
        else if (JumpCounter < HeightDecreaseCount)
        {
            // 顶点暂停阶段
            if (JumpCounter >= HeightIncreaseCount && !HasUpStep)
            {            
                Debug.Log("level++");
                var level = m_Lamniat.CurrentHeight + 1;
                m_Lamniat.SetCurrentHeight(level);  // 虚拟高度提升到1
                HasUpStep = true;
            }
            
            m_Lamniat.SprtRenderer.transform.localPosition = new Vector2(0, jumpOffset[JumpCounter]-1);
        }
        else if (JumpCounter >= HeightDecreaseCount) 
        {
            Tilemap currentTilemap = m_HeightManager.GetCurrentTilemapByAvatarHeight(m_Lamniat.CurrentHeight);
            if (TileUtils.HasTileAtPlayerPosition(currentTilemap, m_Lamniat.BodyCollider.bounds))
            {
                Debug.Log("FinishJump");
                IsJumping = false; // 结束跳跃
                m_LamniatMovement.CanMove = true;
                HasUpStep = false;
                HasDownStep = false;

                m_LamniatMovement.SetMoveVelocity(Vector2.zero);
                m_LamniatMovement.SetFirstMoveWhileJumping = false;

                // 播完Landing動畫
                m_Lamniat.Animator.SetTrigger("land");
                // 切換状态
                float clipTime = AnimeUtils.GetAnimateClipTimeInRuntime(m_Lamniat.Animator, "Lamniat_jump_landing");
                Debug.Log("clipTime: "+clipTime);
                Invoke("EndJumpState", clipTime);

                // 一旦跳躍結束，便啟動對應level的Trigger
                // for (int i = 0; i < tilemapTriggerArray.Length; i++)
                // {
                //     // 通过计算确定当前Trigger属于哪个level
                //     int triggerLevel = i / triggersPerLevel;
                //     tilemapTriggerArray[i].enabled = (triggerLevel == level);
                // }

                m_Lamniat.SprtRenderer.transform.localPosition = new Vector2(0, 0);
                JumpCounter = 0;
            }
            else
            {
                int fixedJumpCounter = HeightDecreaseCount+(JumpCounter-HeightDecreaseCount)%HeightDecreaseCountExponential;
                if (JumpCounter >= HeightDecreaseCount && !HasDownStep)
                {
                    var level = m_Lamniat.CurrentHeight - 1;
                    if(level < m_HeightManager.MinimumLevel) level = 0;
                    m_Lamniat.SetCurrentHeight(level); // 否则回到level 0
                    HeightChangeCount = JumpCounter;
                    HasDownStep = true;
                    Debug.Log("level--");
                }
                else if (JumpCounter - HeightChangeCount >= HeightDecreaseCountExponential)
                {
                    var level = m_Lamniat.CurrentHeight - 1;
                    if(level < m_HeightManager.MinimumLevel) level = 0;
                    m_Lamniat.SetCurrentHeight(level); // 否则回到level 0
                    HeightChangeCount = JumpCounter;
                    Debug.Log("level--");
                }

                //Debug.Log("fixJumpCounter:" + fixJumpCounter);
                m_Lamniat.SprtRenderer.transform.localPosition = new Vector2(0, jumpOffset[fixedJumpCounter]);
            }
        }

        // JumpCounter++;
        JumpCounter = JumpCounter + (int)Mathf.Round(Time.deltaTime*100) ;
        if (JumpCounter > JumpFallingCount)
        {
            //Debug.Log("jumpCounter: " + jumpCounter + " x:" + transform.position.x + " y:" + transform.position.y );
            FixJumpCorners();
        }
        UpdateViewPosition();

    }

    private void FloatTimeElapsedConvertToFrameInt(float value) 
    {
        float threshold = 0.01f;
        m_jumpingTimeElapsed += value;
        JumpCounter = Mathf.FloorToInt(m_jumpingTimeElapsed / threshold);
    }

    private void EndJumpState() {
        if(m_LamniatMovement.IsMoving) m_Lamniat.SetCurrentBaseState(m_Lamniat.Move);
        else m_Lamniat.SetCurrentBaseState(m_Lamniat.Idle);
    }

    private void UpdateViewPosition() 
    {
        Vector3 viewPosition = transform.position;
        viewPosition.y -= m_Lamniat.LastHeight;
        viewPosition.y += m_Lamniat.CurrentHeight; // 根据虚拟高度调整Y坐标
        m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
        
        // 进一步根据jumpDuration调整spriteRenderer的Y坐标来模拟跳跃效果
        // 例如：使用Mathf.Lerp或其他函数来平滑过渡位置变化
        transform.position = viewPosition;
    }


    private void FixJumpCorners()
    {
        // 跳躍的時候將四個角的位置標記起來
        // 哪一個角有碰到，就讓給collider一個offset往該角移動
        // 只碰到一個角，就往斜向位移
        // 碰到兩個角，就往X或Y方向移動
        // 四個角都碰到，offset就會相加並互相抵銷

        Tilemap currentTilemap = m_HeightManager.GetCurrentTilemapByAvatarHeight(m_Lamniat.CurrentHeight);
        // 與BodyCollider範圍相比有縮小
        Vector3Int body_bottom_left = currentTilemap.WorldToCell(transform.position + new Vector3(-0.3f, -1.8f, 0));
        Vector3Int body_bottom_right = currentTilemap.WorldToCell(transform.position + new Vector3(0.3f, -1.8f, 0));
        Vector3Int body_top_left = currentTilemap.WorldToCell(transform.position + new Vector3(-0.3f, -1.2f, 0));
        Vector3Int body_top_right = currentTilemap.WorldToCell(transform.position + new Vector3(0.3f, -1.2f, 0));

        Vector3 offsetPosition = new Vector3(0, 0, 0);
        if (TileUtils.HasTileAtPosition(currentTilemap, body_bottom_left))
        {
            offsetPosition += new Vector3(-0.05f * m_Lamniat.BodyCollider.size.x, -0.05f * m_Lamniat.BodyCollider.size.y);
        }
        if (TileUtils.HasTileAtPosition(currentTilemap, body_bottom_right))
        {
            offsetPosition += new Vector3(0.05f * m_Lamniat.BodyCollider.size.x, -0.05f * m_Lamniat.BodyCollider.size.y);
        }
        if (TileUtils.HasTileAtPosition(currentTilemap, body_top_left))
        {
            offsetPosition += new Vector3(-0.05f * m_Lamniat.BodyCollider.size.x, 0.05f * m_Lamniat.BodyCollider.size.y);
        }
        if (TileUtils.HasTileAtPosition(currentTilemap, body_top_right))
        {
            offsetPosition += new Vector3(0.05f * m_Lamniat.BodyCollider.size.x, 0.05f * m_Lamniat.BodyCollider.size.y);
        }


        if (offsetPosition != Vector3.zero)
        {
            offsetPosition.x = Mathf.Clamp(offsetPosition.x, -0.25f * m_Lamniat.BodyCollider.size.x, 0.25f * m_Lamniat.BodyCollider.size.x);
            offsetPosition.y = Mathf.Clamp(offsetPosition.y, -0.25f * m_Lamniat.BodyCollider.size.y, 0.25f * m_Lamniat.BodyCollider.size.y);
            Vector3 fixCornersPosition = transform.position + offsetPosition;
            transform.position = fixCornersPosition;
            //Debug.Log("offsetPosition: x = " + offsetPosition.x + ", y = " + offsetPosition.y + ", level = " + level);
        }
    }

    private void FixJumpCornersWhileDownStep() {
        Debug.Log("FixJumpCornersWhileDownStep start");
        if (m_Lamniat.CurrentHeight-1 < 0 || !m_LamniatMovement.CanMove)
        {
            Debug.Log("FixJumpCornersWhileDownStep return");
            return;
        }
        Tilemap currentTilemap = m_HeightManager.GetCurrentTilemapByAvatarHeight(m_Lamniat.CurrentHeight-1);
        // 與BodyCollider範圍相比有縮小
        Vector3Int body_bottom_left = currentTilemap.WorldToCell(transform.position + new Vector3(-0.3f, -2.8f, 0));
        Vector3Int body_bottom_right = currentTilemap.WorldToCell(transform.position + new Vector3(0.3f, -2.8f, 0));
        Vector3Int body_top_left = currentTilemap.WorldToCell(transform.position + new Vector3(-0.3f, -2.2f, 0));
        Vector3Int body_top_right = currentTilemap.WorldToCell(transform.position + new Vector3(0.3f, -2.2f, 0));

        Vector3 offsetPosition = new Vector3(0, 0, 0);

        if (TileUtils.HasTileAtPosition(currentTilemap, body_bottom_left) && 
            TileUtils.HasTileAtPosition(currentTilemap, body_bottom_right) && 
            TileUtils.HasTileAtPosition(currentTilemap, body_top_left) && 
            TileUtils.HasTileAtPosition(currentTilemap, body_top_right) && 
            !TileUtils.HasTileAtPlayerPosition(currentTilemap, m_Lamniat.BodyCollider.bounds))
        {
            Debug.Log("FixJumpCornersWhileDownStep CanMove = false");
            m_LamniatMovement.CanMove = false;
        }
        Debug.Log("FixJumpCornersWhileDownStep end");
    }
}