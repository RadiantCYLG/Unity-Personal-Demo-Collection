using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//面板基类
public abstract class BasePanel : MonoBehaviour
{
    //整体控制淡入淡出的组件
    private CanvasGroup canvasGroup;
    //alpha值变化速度
    private float alphaSpeed = 10;
    //是否能显示
    private bool isCanShow;
    //定义委托
    private UnityAction hideCallBack;

    protected AudioClip clickAudio;
    protected AudioClip clickAudio_GoBack;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        clickAudio = Resources.Load<AudioClip>("Sounds/Arknights_ButtonClickAudio1");
        clickAudio_GoBack = Resources.Load<AudioClip>("Sounds/Arknights_ButtonClickAudio2_Back");
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckShowOrHide();
    }

    /// <summary>
    /// 初始化面板
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// 显示自己
    /// </summary>
    public virtual void ShowMe()
    {
        isCanShow = true;
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// 隐藏自己
    /// </summary>
    public void HideMe(UnityAction callBack)
    {
        isCanShow = false;
        canvasGroup.alpha = 0;
        //完全隐藏后，用委托执行其他逻辑
        hideCallBack = callBack;
    }

    /// <summary>
    /// 判断面板显示还是隐藏
    /// </summary>
    private void CheckShowOrHide()
    {
        //显示
        if (isCanShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        //隐藏
        else if (!isCanShow)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();
            }
        }
    }

}
