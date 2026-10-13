using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTriggerEvent : ObjBase_AnimTriggerEvent
{
    private Enemy enemy;
    private Enemy_VFX enemy_vfx;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemy_vfx = GetComponentInParent<Enemy_VFX>();
    }

    private void EnableBeCounter()
    {
        enemy_vfx.EnableCounterOnAttack(true);
        enemy.EnableEnemyCanbeCounter(true);
    }

    private void DisableBeCounter()
    {
        enemy_vfx.EnableCounterOnAttack(false);
        enemy.EnableEnemyCanbeCounter(false);
    }
}
