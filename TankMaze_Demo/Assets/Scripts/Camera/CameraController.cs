using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 控制相机在玩家后上方看向并跟随玩家
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; //相机看向玩家目标
    [SerializeField] private Transform playerHead; //坦克头，炮管
    [SerializeField] private float offsetH = 3f; //偏差高度
    [SerializeField] private float horziontalAngle = 20; //水平夹角
    [SerializeField] private float distance = 7f; //距离观测点

    private Vector3 reverseLookDir; //相机看向方向的反方向
    private Vector3 startPos; //相机开始位置
    private Vector3 finalPos; //相机最终位置

    //限制鼠标滚轮控制相机离玩家的距离在以下范围
    private float offsetH_Min = 1;
    private float offsetH_Max = 6;
    private float lookDisMin = 5;
    private float lookDisMax = 20;

    private float mouseScrollInput; //鼠标滚轮输入
    private float scrollSpeed = 4f;
    private float moveT; //跟随物体的线性插值系数

    #region AI方案——平滑跟随玩家
    //[Header("平滑跟随参数")]
    //[SerializeField] private float positionSmoothTime = 0.15f; // 位置平滑时间，越小跟随越快
    //[SerializeField] private float rotationSpeed = 12f;        // 旋转平滑速度
    ////给 SmoothDamp 使用的速度缓存
    //private Vector3 positionVelocity;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MouseScrollControl();
        
    }
    private void LateUpdate()
    {
        CalcuCameraPos();
    }

    /// <summary>
    /// 计算相机位置
    /// </summary>
    private void CalcuCameraPos()
    {
        if(player == null) return;
        if (transform.position != finalPos)
        {
            moveT = 0;
            startPos = transform.position;
        }
        moveT += Time.deltaTime * 3f;

        //确定相机最终位置
        finalPos = player.position + player.transform.up * offsetH;
        reverseLookDir = Quaternion.AngleAxis(horziontalAngle, playerHead.transform.right) * -playerHead.transform.forward;
        finalPos += reverseLookDir * distance;

        #region AI方案——平滑跟随玩家
        // 平滑跟随位置（指数缓动，不受帧率影响）
        //transform.position = Vector3.SmoothDamp(
        //    transform.position,
        //    finalPos,
        //    ref positionVelocity,
        //    positionSmoothTime
        //);
        // 平滑看向目标
        //Quaternion targetRot = Quaternion.LookRotation(-reverseLookDir);
        // 用 1 - exp(-k * dt) 保证帧率无关
        //float rotT = 1 - Mathf.Exp(-rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotT);
        #endregion

        //缓动跟随目标(匀速跟随)
        //transform.position = Vector3.Lerp(startPos, finalPos, moveT);
        //直接跟随
        transform.position = Vector3.Lerp(startPos, finalPos, 1);
        //缓慢看向目标
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-reverseLookDir), 0.1f);
        //直接看向
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-reverseLookDir), 1f);
    }

    /// <summary>
    /// 鼠标滚轮输入和控制
    /// </summary>
    private void MouseScrollControl()
    {
        mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");
        distance += -mouseScrollInput * scrollSpeed;
        distance = Mathf.Clamp(distance, lookDisMin, lookDisMax);
        offsetH += -mouseScrollInput * scrollSpeed;
        offsetH = Mathf.Clamp(offsetH, offsetH_Min, offsetH_Max);
    }
}
