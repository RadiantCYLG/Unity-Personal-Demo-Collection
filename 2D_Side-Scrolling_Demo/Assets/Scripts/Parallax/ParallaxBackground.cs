using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//视差背景
public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPositionX; //记录上一帧相机在 X 轴上的位置。
    private float cameraHalfWidth; //屏幕半宽度

    [SerializeField] private ParallaxLayer[] backgroundLayers; //存取背景图层

    private void Awake()
    {
        mainCamera = Camera.main; //自动找到标签为 "MainCamera" 的摄像机（当前场景中起作用的主相机）

        //mainCamera.orthographicSize（正交大小）：这是摄像机从中心点到屏幕顶部（或底部）的距离（世界单位），换言之，它是屏幕高度的一半。
        //mainCamera.aspect（宽高比）：这是屏幕宽度与高度的比例
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect; //得到屏幕半宽度

        InitialImageLength();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x; //当前相机所在位置
        float moveDistance = currentCameraPositionX - lastCameraPositionX; //计算背景需要移动的距离
        lastCameraPositionX = currentCameraPositionX; //更新lastCameraPositionX

        //计算摄像机的左右边缘
        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(moveDistance);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }

        ////换种写法
        //for (int i = 0; i < backgroundLayers.Length; i++)
        //{
        //    backgroundLayers[i].Move(moveDistance);
        //    backgroundLayers[i].LoopBackground(cameraLeftEdge, cameraRightEdge);
        //}
    }

    /// <summary>
    /// 初始化图像的长度（图宽）
    /// </summary>
    private void InitialImageLength()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.CalculateImageWidth();
        }
    }
}
