using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 属性奖励的类型
/// </summary>
public enum E_PropType
{
    Atk, //加攻击
    Def, //加防御
    Hp, //加血量
    MoveSpeed, //加移速
}

/// <summary>
/// 属性奖励，增加玩家的属性
/// </summary>
public class RewardProp : MonoBehaviour
{
    public E_PropType propType = E_PropType.Atk;
    [SerializeField] private int propChangeValue = 2; //属性增加的值
    [SerializeField] private GameObject getEffect; //奖励获取的特效

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TankPlayer player = other.GetComponent<TankPlayer>();
            //根据属性的类型来增加对应的属性
            switch (propType)
            {
                case E_PropType.Atk:
                    player.atk += propChangeValue;
                    break;
                case E_PropType.Def:
                    player.def += propChangeValue;
                    break;
                case E_PropType.Hp:
                    player.currentHp += propChangeValue;
                    if (player.currentHp > player.maxHp)
                    {
                        player.currentHp = player.maxHp;
                    }
                    GamePanel.Instance.UpdatePlayerHp(player.currentHp, player.maxHp);
                    break;
                case E_PropType.MoveSpeed:
                    player.moveSpeed += propChangeValue;
                    break;
            }

            //创建特效，控制音效
            GameObject getEff = Instantiate(getEffect, this.transform.position, this.transform.rotation);
            AudioSource audioSource = getEff.GetComponent<AudioSource>();
            audioSource.volume = GameDataMgr.Instance.musicData.soundValue;
            audioSource.mute = !GameDataMgr.Instance.musicData.isOpenSound;
            audioSource.Play();

            //面板分数+5
            GamePanel.Instance.AddScore(5);

            Destroy(getEff.gameObject, 1f);
            Destroy(this.gameObject);
        }
    }
}
