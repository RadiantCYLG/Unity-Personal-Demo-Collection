using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private RewardProp[] rewardProps;
    [SerializeField] private GameObject cubeBreakEff; //Cube销毁特效

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            //销毁子弹（Bullet脚本中已实现）


            //创建Cube销毁特效
            GameObject breakEff = Instantiate(cubeBreakEff, transform.position, transform.rotation);
            //控制音效
            AudioSource audioSource = breakEff.GetComponent<AudioSource>();
            audioSource.volume = GameDataMgr.Instance.musicData.soundValue;
            audioSource.mute = !GameDataMgr.Instance.musicData.isOpenSound;
            audioSource.Play();

            //如果有奖励
            if (rewardProps.Length != 0)
            {
                //销毁Cube，50%概率生成随机的属性奖励
                int rangeInt = Random.Range(0, 101);
                if (rangeInt < 90)
                {
                    //将rangeInt作为索引
                    rangeInt = Random.Range(0, rewardProps.Length);
                    Instantiate(rewardProps[rangeInt], transform.position, transform.rotation);
                }
            }

            //延时销毁特效
            Destroy(breakEff.gameObject, 1f);
        }

        //销毁Cube
        Destroy(this.gameObject);
    }
}
