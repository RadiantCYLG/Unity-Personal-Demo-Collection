using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敌方坦克小血条，挂载到HpBarRoot（血条根节点）上
/// </summary>
public class TankEnemyHpBar : MonoBehaviour
{
    [SerializeField] private TankBaseObj tank;
    [SerializeField] private RectTransform imgHp;
    [SerializeField] private float imgHpMaxWidth = 180;
    [SerializeField] private float imgHpMaxHeight = 15;

    private void Awake()
    {
        // 没手动拖的话，往上找父物体里的坦克基类
        if (tank == null)
        {
            tank = GetComponentInParent<TankBaseObj>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (tank != null)
        {
            tank.onHpChanged += UpdateHpBar;
            // 立刻刷新一次初始血量
            UpdateHpBar(tank.currentHp, tank.maxHp);
        }
    }

    private void LateUpdate()
    {
        // 始终面向相机，避免坦克转身时血条也歪过去
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    private void OnDestroy()
    {
        if (tank != null)
        {
            //取消事件订阅，防止敌人销毁后事件还挂着引用
            tank.onHpChanged -= UpdateHpBar;
        }
    }

    /// <summary>
    /// 更新小血条的显示
    /// </summary>
    private void UpdateHpBar(int currentHp, int maxHp)
    {
        if (imgHp == null || maxHp <= 0)
        {
            return;
        }

        currentHp = Mathf.Clamp(currentHp, 0, maxHp); //钳制函数，currentHp在最大值和最小值之间，取currentHp
        imgHp.sizeDelta = new Vector2((float)currentHp / maxHp * imgHpMaxWidth, imgHpMaxHeight);
    }
}
