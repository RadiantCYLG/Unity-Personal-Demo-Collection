using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家坦克和敌方坦克的炮管
/// </summary>
public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bullet; //炮口发射的子弹，用于实例化
    public Transform[] shootPos; //外部决定有几个发射位置
    [SerializeField] private TankBaseObj weaponOwner; //武器拥有者

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 武器的开火行为
    /// </summary>
    public void Fire()
    {
        //根据位置创建子弹
        for (int i = 0; i < shootPos.Length; i++)
        {
            //实例化创建预设体子弹
            GameObject obj = Instantiate(bullet, shootPos[i].position, shootPos[i].rotation);

            Bullet bulletObj = obj.GetComponent<Bullet>();
            bulletObj.SetOwner(weaponOwner); //设置发射子弹的坦克是谁
        }
    }

    /// <summary>
    /// 设置武器的拥有者
    /// </summary>
    /// <param name="tankObj"></param>
    public void SetOwner(TankBaseObj tankObj)
    {
        weaponOwner = tankObj;
    }
}
