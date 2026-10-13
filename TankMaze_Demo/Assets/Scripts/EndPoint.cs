using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 终点
/// </summary>
public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //游戏运行时间暂停
            Time.timeScale = 0;
            //显示鼠标
            Cursor.visible = true;

            UI_Manager.Instance.ShowPanel<WinPanel>();
            UI_Manager.Instance.HideAndRemovePanel<GamePanel>();
        }
    }
}
