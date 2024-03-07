using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraAdjustmentWithAvatarJump : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private Transform followTarget;
    [SerializeField] private Movement_Lamniat m_LamniatMovement;
    [SerializeField] private DynamicJump_Lamniat m_LamiatJump;
    [SerializeField] private Vector3 defaultFollowOffset = new Vector3(0, 0, -10);
    private Vector3 originalFollowOffset;
    public float smoothOffsetY = 100f; // Y軸平滑速度
    //public float smoothOffsetX = 100f; // X軸平滑速度（保持不變）
    private float originalSmoothOffsetY = 10f; 

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            // 儲存攝影機與目標之間的偏移量
            virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = defaultFollowOffset;
            originalFollowOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            Debug.Log("originalFollowOffset: "+originalFollowOffset);
            followTarget = virtualCamera.Follow;
        }
        originalSmoothOffsetY = smoothOffsetY;
    }

    void Update() {
        //Debug.Log("transform.position: "+transform.position);
    }

    void LateUpdate()
    {
        
        if (virtualCamera != null && m_LamiatJump != null && m_LamniatMovement != null)
        {
            //float desiredOffsetX = followTarget.position.x + originalFollowOffset.x - transform.position.x;
            float desiredOffsetY = followTarget.position.y + originalFollowOffset.y - transform.position.y;

            if (m_LamiatJump.JumpCounter != 0)
            {
                smoothOffsetY = 10f;
            }
            else if (smoothOffsetY < originalSmoothOffsetY)
            {
                smoothOffsetY += 5f;
            }
            else
            {
                smoothOffsetY = originalSmoothOffsetY;
            }

            if (m_LamiatJump.JumpCounter < m_LamiatJump.JumpFallingCount)
            {
                desiredOffsetY = desiredOffsetY - m_LamiatJump.JumpCounter/m_LamiatJump.JumpFallingCount*2; // 在跳躍時使用新的Y軸偏移量
            }
            else if (m_LamiatJump.JumpCounter < m_LamiatJump.HeightDecreaseCount)
            {
                desiredOffsetY = desiredOffsetY - 2 + (m_LamiatJump.JumpCounter-m_LamiatJump.JumpFallingCount)/(m_LamiatJump.HeightDecreaseCount - m_LamiatJump.JumpFallingCount)*2; // 在跳躍時使用新的Y軸偏移量
            }
            else if (m_LamiatJump.JumpCounter < m_LamiatJump.HeightDecreaseCount + m_LamiatJump.HeightDecreaseCountExponential)
            {
                desiredOffsetY = desiredOffsetY + (m_LamiatJump.JumpCounter-m_LamiatJump.HeightDecreaseCount)/m_LamiatJump.HeightDecreaseCountExponential*2; // 在跳躍時使用新的Y軸偏移量
            }
            else if (m_LamiatJump.JumpCounter >= m_LamiatJump.HeightDecreaseCount + m_LamiatJump.HeightDecreaseCountExponential)
            {
                desiredOffsetY = desiredOffsetY + 2; // 在跳躍時使用新的Y軸偏移量
            }
            else
            {
                smoothOffsetY = originalSmoothOffsetY;
            }

            if (!m_LamiatJump.OnHeightObjCollisionExit && m_LamiatJump.JumpCounter != 0)
            {
                desiredOffsetY += m_LamniatMovement.Movement.normalized.y*2;
            }

            // 分別為X軸和Y軸計算平滑位置
            //float resultOffsetX = Mathf.Lerp(originalFollowOffset.x, desiredOffsetX, smoothOffsetX * Time.deltaTime);
            float resultOffsetY = Mathf.Lerp(originalFollowOffset.y, desiredOffsetY, smoothOffsetY * Time.deltaTime);

            Vector3 resultOffset = new Vector3(originalFollowOffset.x, resultOffsetY, originalFollowOffset.z);

            if(m_LamiatJump.IsJumping) {
                virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = resultOffset;
            } else {
                virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = originalFollowOffset;
            }
        }
    
    }
}
