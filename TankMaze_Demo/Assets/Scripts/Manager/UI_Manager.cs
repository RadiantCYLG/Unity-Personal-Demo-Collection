using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_Manager
{
    //设置UI_Manager为单例模式
    private static UI_Manager instance = new UI_Manager();
    public static UI_Manager Instance => instance;
    //canvas组件
    private Transform canvas;
    //存储panel的字典
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
   

    private UI_Manager()
    {
        //得到场景上的Canvas对象
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas")?.transform;
        }
        //!!! 过场景时不删除Canvas
        GameObject.DontDestroyOnLoad(canvas.gameObject);
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T ShowPanel<T>() where T : BasePanel
    {
        //得到要显示的面板的名字
        string panelName = typeof(T).Name;
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
    /// 隐藏并移除面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isFade">是否淡出</param>
    public void HideAndRemovePanel<T>(bool isFade = true) where T : BasePanel
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
            //不需要淡出
            else
            {
                RemovePanel(panelName);
            }
        }
    }

    private void RemovePanel(string name)
    {
        //销毁面板对象
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
