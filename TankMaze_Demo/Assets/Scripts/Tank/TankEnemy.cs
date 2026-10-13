using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可以移动的敌方坦克
/// </summary>
public class TankEnemy : TankBaseObj
{
    [Header("敌方坦克移动")]
    //要让敌方坦克在目标点之间来回移动
    [SerializeField] private Transform[] randomNewPos; //要移动到的新的随机位置
    private Transform start; //开始所在点位
    private Transform target; //要移动的目标点
    private Vector3 lookDir; //看向目标点的方向
    private float moveT; //插值系数
    [Header("敌方坦克攻击")]
    [SerializeField] private Transform lookAtPlayer; //看向的玩家
    [SerializeField] private float fireDistanceMin = 15; //最小距离（小于就开火）
    [SerializeField] private float fireIntervalTime = 0.8f; //最小距离（小于就开火）
    private float timer; //计时器
    [SerializeField] private Transform[] shootPos;
    [SerializeField] private GameObject bullet;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        RandomPos();
    }

    // Update is called once per frame
    void Update()
    {
        MoveFixed();
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        if (lookAtPlayer == null)
            return;
        //当玩家和自身距离小于一定距离时，就看向玩家
        if (Mathf.Abs(Vector3.Distance(this.transform.position, lookAtPlayer.position)) <= 25)
        {
            tankHead.transform.LookAt(lookAtPlayer);
        }
        
        if (lookAtPlayer != null)
        {
            //当玩家和自身距离小于一定距离时，就看向玩家并开火
            if (Mathf.Abs( Vector3.Distance(this.transform.position, lookAtPlayer.position) )<= fireDistanceMin)
            {        
                CheckFire();
            }
        }
    }

    /// <summary>
    /// 在指定随机点之间固定移动
    /// </summary>
    private void MoveFixed()
    {
        if (randomNewPos.Length == 0)
            return;
        #region 插值运算移动
        if (transform.position != target.position)
        {
            moveT = 0;
            start = transform;
        }
        moveT += Time.deltaTime;

        lookDir = Quaternion.AngleAxis(0, transform.up) * (target.position - transform.position).normalized;
        //平滑地看向目标点
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 0.5f);
        //朝目标点匀速移动
        transform.position = Vector3.Lerp(start.position, target.position, moveT);
        #endregion

        #region 普通移动
        //transform.LookAt(target);
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        #endregion

        //两点距离小于0.05时，重新随机一个目标点
        if (Vector3.Distance(this.transform.position, target.position) < 0.05f)
        {
            RandomPos();
        }
    }

    /// <summary>
    /// 随机一个新的要移动到的位置
    /// </summary>
    private void RandomPos()
    {
        if (randomNewPos.Length == 0)
            return;
        target = randomNewPos[Random.Range(0, randomNewPos.Length)];
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
        for (int i = 0; i < shootPos.Length; i++)
        {
            GameObject obj = Instantiate(bullet, shootPos[i].position, shootPos[i].rotation);
            Bullet bulletObj = obj.GetComponent<Bullet>();
            //设置自己为子弹的拥有者（发射者）
            bulletObj.SetOwner(this);
        }
    }

    protected override void Die()
    {
        base.Die();
        //游戏面板更新分数显示
        GamePanel.Instance.AddScore(10);
    }
}
