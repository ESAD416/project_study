using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicJump_Lamniat : MonoBehaviour
{
    [Header("DynamicJump_Lamniat 基本物件")]
    [SerializeField] protected Avatar_Lamniat m_Lamniat;

    [Header("DynamicJump_Lamniat 基本參數")]
    public bool IsJumping;
    public bool CanJump;
    public bool OnHeightObjCollisionExit;

    public int JumpCounter = 0;
    public int MaximumJumpCounter = 10;

    void Start() {

    }

    void Update() {
        
    }

    #region 碰撞偵測

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰到了Collider，执行相应的逻辑
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 碰到了Collider，执行相应的逻辑

        if (collision.gameObject.CompareTag("Map_Collision") && IsJumping)
        {
            OnHeightObjCollisionExit = true;
        } 

        
    }

    #endregion

}