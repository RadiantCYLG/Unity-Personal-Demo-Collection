using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private TankBaseObj owner; //发射子弹的坦克是谁
    [SerializeField] private float speed = 50;
    [SerializeField] private GameObject effects; //子弹销毁特效

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //碰到了Tag为Cube的对象
        //子弹射击到不同阵营单位（玩家或敌方）时，也爆炸
        if (other.CompareTag("Cube") ||
            other.CompareTag("Player")&& owner.CompareTag("Enemy")||
            other.CompareTag("Enemy") && owner.CompareTag("Player"))
        {
            //判断子弹撞到的对象是否受伤
            TankBaseObj tankObj =other.GetComponent<TankBaseObj>();
            if(tankObj != null)
            {
                tankObj.BeHurt(owner);
            }

            if(effects!=null)
            {
                //子弹销毁时，可以创建一个 爆炸特效
                GameObject eff = Instantiate(effects, transform.position, transform.rotation);
                //控制特效的音效
                AudioSource effSound = eff.GetComponent<AudioSource>();
                effSound.volume = GameDataMgr.Instance.musicData.soundValue; //设置音效大小
                effSound.mute = !GameDataMgr.Instance.musicData.isOpenSound; //是否静音
                effSound.Play();
                //销毁特效
                Destroy(eff.gameObject, 0.9f);
            }
            Destroy(this.gameObject);
        }
    }

    public void SetOwner(TankBaseObj tankObj)
    {
        owner = tankObj;
    }

}
