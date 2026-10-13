using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 坦克对象（玩家坦克和敌人坦克）基类
/// </summary>
public abstract class TankBaseObj : MonoBehaviour
{
    //血量变化事件（有参无返），参数(当前血量，最大血量)
    public event UnityAction<int, int> onHpChanged;

    [Header("基本属性")]
    public int atk; //攻击
    public int def; //防御
    public int maxHp = 100; //最大血量
    public int currentHp; //当前血量
    public float moveSpeed = 10; //移动速度
    [SerializeField] protected float rotateSpeed = 100; //旋转速度
    [SerializeField] protected float headRotateSpeed = 100;  //头部旋转速度
    [Header("部件")]
    [SerializeField] protected Transform tankHead; //坦克头部（连着炮管）
    [SerializeField] protected Weapon currentWeapon; //当前武器（炮管）
    [Header("特效")]
    [SerializeField] protected GameObject dieEffects; //死亡特效


    private void Awake()
    {
        currentHp = maxHp;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 开火
    /// </summary>
    public abstract void Fire();

    /// <summary>
    /// 被对方攻击导致受伤
    /// </summary>
    public virtual void BeHurt(TankBaseObj other)
    {
        int dmg = other.atk - this.def;
        if (dmg <= 0)
        {
            //5%的保底伤害
            dmg = (int)(0.05f * other.atk);
        }
        this.currentHp -= dmg;
        //血量减少至小于等于0，则死亡
        if (currentHp <= 0)
        {
            this.currentHp = 0;
            //if (transform.CompareTag("Player"))
            //{
            //    GamePanel.Instance.UpdatePlayerHp(currentHp, maxHp);
            //}

        }
        //玩家受伤后，更新玩家的血条
        if (transform.CompareTag("Player"))
        {
            GamePanel.Instance.UpdatePlayerHp(currentHp, maxHp);
        }
        //更新Enemy的血条
        onHpChanged?.Invoke(currentHp, maxHp); //调用血量变化事件

        if (currentHp <= 0)
        {
            this.Die();
        }
    }

    /// <summary>
    /// 死亡行为
    /// </summary>
    protected virtual void Die()
    {
        //销毁对象
        Destroy(gameObject);
        //死亡时，给可能的坦克对象，播放对应特效
        if (this.dieEffects != null)
        {
            //自身死亡时，从自身位置创建特效
            GameObject dEf = GameObject.Instantiate(dieEffects, this.transform.position, this.transform.rotation);

            //每一个特效身上会关联一个AudioSource组件
            //控制特效的音效播放
            AudioSource audioSource = dEf.GetComponent<AudioSource>();
            audioSource.volume = GameDataMgr.Instance.musicData.soundValue; //音量大小
            audioSource.mute = !GameDataMgr.Instance.musicData.isOpenSound; //是否静音
            audioSource.Play(); //避免没有勾选Playe On Awake
        }
    }
}
