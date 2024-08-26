using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObj : MonoBehaviour
{
    [SerializeField] protected Transform m_Target;
    
    [SerializeField] protected Vector3 followOffset = new Vector3(0, 0, -10);
    public float smoothOffsetY = 100f; // Y軸平滑速度
    public float smoothOffsetX = 100f; // X軸平滑速度（保持不變）
    protected float originalSmoothOffsetY;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        originalSmoothOffsetY = smoothOffsetY;
        Debug.Log("originalSmoothOffsetY: "+originalSmoothOffsetY);
    }

    protected virtual void Update() {
        Debug.Log("CameraFollowObj transform.position: "+transform.position);
    }

    protected virtual void LateUpdate()
    {
        Vector3 desiredPositionX = new Vector3(m_Target.position.x + followOffset.x, transform.position.y, transform.position.z);
        Vector3 desiredPositionY = new Vector3(transform.position.x, m_Target.position.y + followOffset.y, transform.position.z);

        // 分別為X軸和Y軸計算平滑位置
        Vector3 resultPositionX = Vector3.Lerp(transform.position, desiredPositionX, smoothOffsetX * Time.deltaTime);
        Vector3 resultPositionY = Vector3.Lerp(transform.position, desiredPositionY, smoothOffsetY * Time.deltaTime);

        Vector3 resultOffset = new Vector3(resultPositionX.x, resultPositionY.y, m_Target.position.z + followOffset.z);

        transform.position = resultOffset;
    }
}
