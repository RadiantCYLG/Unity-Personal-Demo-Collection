using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图摄像机
/// </summary>
public class CameraMap : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private int cameraHeight; //摄像机高度

    private Vector3 pos; //摄像机位置
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraFollow();
    }

    private void CameraFollow()
    {
        if (targetPlayer == null)
        {
            return;
        }
        pos.x = targetPlayer.position.x;
        pos.z = targetPlayer.position.z;
        //摄像机高度
        pos.y = cameraHeight;
        transform.position = pos;
    }
}
