using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : ObjBase_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override void TakeDamage(float damage, Transform damageDealer /*攻击造成者*/)
    {
        base.TakeDamage(damage, damageDealer);

        if (isDead)
            return;

        //有玩家对敌人攻击了
        if (damageDealer.GetComponent<Player>() != null)
        {
            //敌人进入战斗状态
            enemy.EnterBattleState(damageDealer);
        }
    }
}
