using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Manager
{
    //设置UI_Manager为单例模式
    private static UI_Manager instance = new UI_Manager();
    public static UI_Manager Instance => instance;

    //存储panel的字典
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    //Canvas组件
    private Transform canvas;

    //音效资源
    private AudioSource btnAudioSource;

    private UI_Manager()
    {
        //得到场景上的Canvas对象
        canvas = GameObject.Find("Canvas").transform;

        //!!!
        //过场景时不删除Canvas
        GameObject.DontDestroyOnLoad(canvas.gameObject);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clip"></param>
    public void PlayBtnSound(AudioClip clip)
    {
        btnAudioSource = canvas.GetComponent<AudioSource>();
        if (btnAudioSource == null)
        {
            btnAudioSource = canvas.AddComponent<AudioSource>();
            //可选优化：设置音效不随距离衰减（2D音效）
            btnAudioSource.spatialBlend = 0f;
        }

        if (btnAudioSource!=null&& clip != null)
        {
            btnAudioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <returns></returns>
    public T ShowPanel<T>() where T : BasePanel
    {
        //得到要显示面板的名字
        string panelName = typeof(T).Name;
        //字典中已有该面板
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }

        //实例化（创建）面板
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObj.transform.SetParent(canvas, false);
        //获取面板上的脚本并存入字典
        T panel = panelObj.GetComponent<T>();
        panelDic.Add(panelName, panel);
        //显示面板、返回面板
        panel.ShowMe();
        return panel;
    }

    /// <summary>
    /// 隐藏面板(隐藏后会删除面板)
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <param name="isFade">是否淡出</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            //需要淡出
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>
                {
                    RemovePanel(panelName);
                });
            }
            else
            {
                RemovePanel(panelName);
            }
        }
    }

    private void RemovePanel(string name)
    {
        //删除面板
        GameObject.Destroy(panelDic[name].gameObject);
        //字典中移除对应面板
        panelDic.Remove(name);
    }

    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }

        return null;
    }
}
