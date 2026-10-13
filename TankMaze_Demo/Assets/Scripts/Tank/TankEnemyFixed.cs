using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 固定不动的敌方坦克
/// </summary>
public class TankEnemyFixed : TankBaseObj
{
    [Header("敌方坦克攻击")]
    [SerializeField] private float fireIntervalTime = 0.7f; //开火间隔时间
    private float timer = 0;
    [SerializeField] private Transform[] shootPos; //一个或多个发射位置
    [SerializeField] private GameObject bullet; //关联发射的弹预设体

    private void Update()
    {
        CheckFire();
    }

    private void CheckFire()
    {
        //间隔开火
        timer += Time.deltaTime;
        if (timer > fireIntervalTime)
        {
            timer = 0;
            Fire();
        }
    }

    public override void Fire()
    {
        //遍历发射位置，从发射位置发射子弹
        for(int i=0; i<shootPos.Length; i++)
        {
            GameObject obj = Instantiate(bullet, shootPos[i].position, shootPos[i].rotation);
            Bullet bulletObj =obj.GetComponent<Bullet>();
            //设置自己为子弹的拥有者（发射者）
            bulletObj.SetOwner(this);
        }
    }

    public override void BeHurt(TankBaseObj other)
    {
       //固定不动的敌方坦克不受伤害
    }
}
