using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObj_Lamniat : CameraFollowObj
{
    [SerializeField] private Movement_Lamniat m_targetMovement;
    [SerializeField] private DynamicJump_Lamniat m_targetJump;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void LateUpdate()
    {
        Vector3 desiredPositionX = new Vector3(m_Target.position.x + followOffset.x, transform.position.y, transform.position.z);
        Vector3 desiredPositionY = new Vector3(transform.position.x, m_Target.position.y + followOffset.y, transform.position.z);

        if (m_targetJump != null && m_targetMovement != null)
        {

            if (m_targetJump.JumpCounter != 0)
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

            if (m_targetJump.JumpCounter < m_targetJump.JumpFallingCount)
            {
                desiredPositionY.y = desiredPositionY.y - m_targetJump.JumpCounter/m_targetJump.JumpFallingCount*2; // 在跳躍時使用新的Y軸偏移量
            }
            else if (m_targetJump.JumpCounter < m_targetJump.HeightDecreaseCount)
            {
                desiredPositionY.y = desiredPositionY.y - 2 + (m_targetJump.JumpCounter-m_targetJump.JumpFallingCount)/(m_targetJump.HeightDecreaseCount - m_targetJump.JumpFallingCount)*2; // 在跳躍時使用新的Y軸偏移量
            }
            else if (m_targetJump.JumpCounter < m_targetJump.HeightDecreaseCount + m_targetJump.HeightDecreaseCountExponential)
            {
                desiredPositionY.y = desiredPositionY.y + (m_targetJump.JumpCounter-m_targetJump.HeightDecreaseCount)/m_targetJump.HeightDecreaseCountExponential*2; // 在跳躍時使用新的Y軸偏移量
            }
            else if (m_targetJump.JumpCounter >= m_targetJump.HeightDecreaseCount + m_targetJump.HeightDecreaseCountExponential)
            {
                desiredPositionY.y = desiredPositionY.y + 2; // 在跳躍時使用新的Y軸偏移量
            }
            else
            {
                smoothOffsetY = originalSmoothOffsetY;
            }

            if (!m_targetJump.OnHeightObjCollisionExit && m_targetJump.JumpCounter != 0)
            {
                desiredPositionY.y += m_targetMovement.Movement.normalized.y*2;
            }

        }
    
        // 分別為X軸和Y軸計算平滑位置
        Vector3 resultPositionX = Vector3.Lerp(transform.position, desiredPositionX, smoothOffsetX * Time.deltaTime);
        Vector3 resultPositionY = Vector3.Lerp(transform.position, desiredPositionY, smoothOffsetY * Time.deltaTime);

        Vector3 resultOffset = new Vector3(resultPositionX.x, resultPositionY.y, m_Target.position.z + followOffset.z);

        transform.position = resultOffset;
    }
}
