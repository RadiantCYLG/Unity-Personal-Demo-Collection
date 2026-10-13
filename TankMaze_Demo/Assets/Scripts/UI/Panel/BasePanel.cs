using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 面板基类，不实例化
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    //控制面板淡入淡出的组件
    private CanvasGroup canvasGroup;
    //alpha值变化速度
    private float alphaSpeed = 5f;
    //面板是否显示
    private bool isCanShow;
    //委托，隐藏面板后做什么
    private UnityAction hideCallBack;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    //必须声明为virtual，否则子类声明自己的Update时会把它隐藏掉，导致淡入淡出和移除逻辑失效
    protected virtual void Update()
    {
        CheckShowOrHide();
    }

    public virtual void ShowMe()
    {
        isCanShow = true;
        canvasGroup.alpha = 1;
    }

    public virtual void HideMe(UnityAction callBack)
    {
        isCanShow = false;
        canvasGroup.alpha = 0;
        //完全隐藏后，用委托执行其他逻辑，如在场景中删除面板
        hideCallBack = callBack;
    }

    /// <summary>
    /// 判断面板显示还是隐藏
    /// </summary>
    private void CheckShowOrHide()
    {
        //显示
        if(isCanShow && canvasGroup.alpha!=1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha>=1)
                canvasGroup.alpha = 1;
        }

        //隐藏
        else if(!isCanShow)
        {
            canvasGroup.alpha-= alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha<=0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
            }
        }
    }
}
