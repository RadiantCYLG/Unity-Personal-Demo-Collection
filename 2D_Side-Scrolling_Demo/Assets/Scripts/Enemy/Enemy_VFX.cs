using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VFX : ObjBase_VFX
{
    [Header("反击相关")]
    [SerializeField] private GameObject attackAlert;

    /// <summary>
    /// 出现攻击警告时，玩家能反击
    /// </summary>
    public void EnableCounterOnAttack(bool enable)
    {
        if (attackAlert == null)
        {
            return;
        }
        attackAlert.SetActive(enable);
    }

}
