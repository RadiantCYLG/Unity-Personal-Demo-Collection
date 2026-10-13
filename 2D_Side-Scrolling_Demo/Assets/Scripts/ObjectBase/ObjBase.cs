using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//实体对象类，作为玩家和敌人的基类
public abstract class ObjBase : MonoBehaviour
{
    public event Action onFlipUI; //处理UI的翻转

    protected FSM fSM; //状态机管理器
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    [Header("碰撞检测相关")]
    //地面检测
    [SerializeField] protected LayerMask whatIsGroundOrWall; //定义地面或墙面图层（墙面也使用该变量）
    [SerializeField] private Transform groundCheckPoint; //地面检测射线起点
    [SerializeField] private float groundCheckDistance; //射线到地面的距离
    //墙体检测
    [SerializeField] private Transform primaryWallCheckPoint; //第一条射线起点，墙体检测
    [SerializeField] private Transform secondWallCheckPoint; //第二条射线起点，墙体检测
    [SerializeField] private float wallCheckDistance; //射线到墙的距离

    public bool isOnGround { get; private set; } //是否触地
    public bool isTouchWall { get; private set; } //是否触墙
    public int rayFaceDir { get; private set; } = 1; //决定墙体检测射线方向

    protected bool faceRight = true; //玩家或敌人默认 面朝右

    //处理受到攻击的逻辑
    private bool isKnocked; //是否受击
    private Coroutine knockbackCo;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        fSM = new FSM();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckGroundCollision();
        fSM.UpdateCurState();
    }


    public virtual void ObjectBaseDeath()
    {

    }

    public void ExecuteKnockBack(Vector2 knockback, float duration)
    {
        if (knockbackCo != null)
        {
            StopCoroutine(knockbackCo);
        }
        knockbackCo = StartCoroutine(KnockBackCo(knockback, duration));
    }

    /// <summary>
    /// 受击协程方法
    /// </summary>
    /// <param name="knockback">受击向量（力）</param>
    /// <param name="duration">受击效果持续时间</param>
    /// <returns></returns>
    private IEnumerator KnockBackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.velocity = knockback;

        yield return new WaitForSeconds(duration);

        rb.velocity = Vector2.zero; //防止一直移动
        isKnocked = false;
    }

    /// <summary>
    /// 设置二维向量，决定移动或跳跃的大小或方向
    /// </summary>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        rb.velocity = new Vector2(xVelocity, yVelocity);
        CheckFlip(xVelocity);
    }

    /// <summary>
    /// 处理角色什么情况下改变朝向
    /// </summary>
    public void CheckFlip(float xVelocity)
    {
        //角色朝右时，开始向左
        if (faceRight == true && xVelocity < 0)
        {
            Flip();
        }
        //角色朝左时，开始向右
        else if (faceRight == false && xVelocity > 0)
        {
            Flip();
        }
    }

    /// <summary>
    /// 改变角色朝向
    /// </summary>
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        //翻转后，再改变朝向
        faceRight = !faceRight;
        rayFaceDir = rayFaceDir * -1;

        onFlipUI?.Invoke();
    }

    /// <summary>
    /// 处理玩家或敌人与地面的碰撞
    /// </summary>
    protected void CheckGroundCollision()
    {
        //射线检测玩家是否在地面         （射线发射点，往哪发射，发射多远的距离，撞到谁才结束）
        //撞到了任何带碰撞体的物体，它就返回 true
        //射线什么也没撞到，它就返回 false
        //Physics2D.Raycast的返回类型是RaycastHit2D，可以隐式转换为bool类型
        isOnGround = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, whatIsGroundOrWall);
        isTouchWall = Physics2D.Raycast(primaryWallCheckPoint.position, Vector2.right * rayFaceDir, wallCheckDistance, whatIsGroundOrWall)
                    && Physics2D.Raycast(secondWallCheckPoint.position, Vector2.right * rayFaceDir, wallCheckDistance, whatIsGroundOrWall);
    }

    /// <summary>
    /// Scene窗口辅助画图，射线可视化
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        //检测地面的射线
        Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + new Vector3(0, -groundCheckDistance));
        //检测墙壁的射线
        Gizmos.DrawLine(primaryWallCheckPoint.position, primaryWallCheckPoint.position + new Vector3(wallCheckDistance * rayFaceDir, 0));
        Gizmos.DrawLine(secondWallCheckPoint.position, secondWallCheckPoint.position + new Vector3(wallCheckDistance * rayFaceDir, 0));
    }

    /// <summary>
    /// 攻击动画结束触发方法
    /// </summary>
    public void CallAnimTrigger()
    {
        fSM.currentState.AnimTrigger();
    }
}
