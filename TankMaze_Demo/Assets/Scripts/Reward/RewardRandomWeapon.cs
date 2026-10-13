using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 随机武器奖励
/// </summary>
public class RewardRandomWeapon : MonoBehaviour
{
    [SerializeField] private Weapon[] weaponObj;
    [SerializeField] private GameObject getEffect; //奖励获取的特效

    private void OnTriggerEnter(Collider other)
    {
        //碰撞到的对象Tag为Player
        if (other.CompareTag("Player"))
        {
            //数组随机索引，让玩家获得随机武器
            int index = Random.Range(0, weaponObj.Length);
            //得到碰到玩家身上所挂载TankPlayer脚本
            TankPlayer player = other.GetComponent<TankPlayer>();
            player.ChangeWeapon(weaponObj[index]);
            
            //创建特效，控制音效
            GameObject getEff = Instantiate(getEffect, this.transform.position, this.transform.rotation);
            AudioSource audioSource = getEff.GetComponent<AudioSource>();
            audioSource.volume = GameDataMgr.Instance.musicData.soundValue;
            audioSource.mute = !GameDataMgr.Instance.musicData.isOpenSound;
            audioSource.Play();


            Destroy(getEff.gameObject, 1f);
            Destroy(this.gameObject);
        }
    }
}
