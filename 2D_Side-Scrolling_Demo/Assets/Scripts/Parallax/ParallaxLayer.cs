using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//创建一个新的变量类型，记录视差背景图层
public class ParallaxLayer
{
    [SerializeField] private Transform background; //存取背景对象
    [SerializeField] private float parallaxMultiplier; //与背景移动距离相乘的系数，决定背景在一帧内的移动量相当于相机移动量的百分比
    [SerializeField] private float imageWidthOffset = 5f; //宽度偏差值

    private float imageFullWidth; //图像全宽
    private float imageHalfWidth; //屏幕半宽

    /// <summary>
    /// 背景移动
    /// </summary>
    public void Move(float moveDistance)
    {
        background.position += Vector3.right * (moveDistance * parallaxMultiplier);
    }

    /// <summary>
    /// 计算背景图片的宽度
    /// </summary>
    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x; //.bounds.size.x: Sprite（精灵图片）在 Unity 世界坐标系下的实际宽度。
        imageHalfWidth = imageFullWidth / 2;
    }

    /// <summary>
    /// 循环背景
    /// </summary>
    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        //计算图片的左右边缘
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;
        float imageLeftEdge = (background.position.x - imageHalfWidth) + imageWidthOffset;

        if (imageRightEdge < cameraLeftEdge)
        {
            background.position += Vector3.right * imageFullWidth;
        }
        else if (imageLeftEdge > cameraRightEdge)
        {
            background.position += Vector3.right * -imageFullWidth;
        }
    }
}
