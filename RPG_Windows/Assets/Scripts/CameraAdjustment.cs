using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjustment : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Movement_Lamniat m_LamniatMovement;
    [SerializeField] private DynamicJump_Lamniat m_LamiatJump;
    [SerializeField] private Vector3 followOffset = new Vector3(0, 0, -10);
    private Vector3 originalFollowOffset;
    public float smoothOffsetY = 100f; // Y軸平滑速度
    public float smoothOffsetX = 100f; // X軸平滑速度（保持不變）
    private float originalSmoothOffsetY = 10f; 

    // Start is called before the first frame update
    void Start()
    {
        originalSmoothOffsetY = smoothOffsetY;
    }

    void Update() {
        //Debug.Log("transform.position: "+transform.position);
    }

    void LateUpdate()
    {
        
        if (m_LamiatJump != null && m_LamniatMovement != null)
        {

            Vector3 desiredPositionX = new Vector3(followTarget.position.x + followOffset.x, transform.position.y, transform.position.z);
            Vector3 desiredPositionY = new Vector3(transform.position.x, followTarget.position.y + followOffset.y, transform.position.z);

            if (m_LamiatJump.JumpCounter != 0)
            {
                smoothOffsetY = 10f;
            }
            else if (smoothOffsetY < originalSmoothOffsetY)
            {
                smoothOffsetY += 5f/Time.deltaTime;
            }
            else
            {
                smoothOffsetY = originalSmoothOffsetY;
            }

            if (m_LamiatJump.JumpCounter < m_LamiatJump.JumpFallingCount)
            {
                desiredPositionY.y = desiredPositionY.y - m_LamiatJump.JumpCounter/m_LamiatJump.JumpFallingCount*2; // 在跳躍時使用新的Y軸偏移量
            }
            else if (m_LamiatJump.JumpCounter < m_LamiatJump.HeightDecreaseCount)
            {
                desiredPositionY.y = desiredPositionY.y - 2 + (m_LamiatJump.JumpCounter-m_LamiatJump.JumpFallingCount)/(m_LamiatJump.HeightDecreaseCount - m_LamiatJump.JumpFallingCount)*2; // 在跳躍時使用新的Y軸偏移量
            }
            else if (m_LamiatJump.JumpCounter < m_LamiatJump.HeightDecreaseCount + m_LamiatJump.HeightDecreaseCountExponential)
            {
                desiredPositionY.y = desiredPositionY.y + (m_LamiatJump.JumpCounter-m_LamiatJump.HeightDecreaseCount)/m_LamiatJump.HeightDecreaseCountExponential*2; // 在跳躍時使用新的Y軸偏移量
            }
            else if (m_LamiatJump.JumpCounter >= m_LamiatJump.HeightDecreaseCount + m_LamiatJump.HeightDecreaseCountExponential)
            {
                desiredPositionY.y = desiredPositionY.y + 2; // 在跳躍時使用新的Y軸偏移量
            }
            else
            {
                smoothOffsetY = originalSmoothOffsetY;
            }

            if (!m_LamiatJump.OnHeightObjCollisionExit && m_LamiatJump.JumpCounter != 0)
            {
                desiredPositionY.y += m_LamniatMovement.Movement.normalized.y*2;
            }

            // 分別為X軸和Y軸計算平滑位置
            Vector3 resultPositionX = Vector3.Lerp(transform.position, desiredPositionX, smoothOffsetX * Time.deltaTime);
            Vector3 resultPositionY = Vector3.Lerp(transform.position, desiredPositionY, smoothOffsetY * Time.deltaTime);

            Vector3 resultOffset = new Vector3(resultPositionX.x, resultPositionY.y, followTarget.position.z + followOffset.z);

            transform.position = resultOffset;
        }
    
    }
}
