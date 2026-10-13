using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ObjBase
{
    public static event Action onPlayerDeath; //玩家Death的事件
    //状态相关
    public Player_IdleState idleState { get; private set; } //待机状态
    public Player_MoveState moveState { get; private set; } //移动状态
    public Player_JumpState jumpState { get; private set; } //跳跃状态
    public Player_FallState fallState { get; private set; } //下落状态
    public Player_WallSlideState wallSlideState { get; private set; } //贴墙滑行状态
    public Player_WallJumpState wallJumpState { get; private set; } //贴墙跳跃状态
    public Player_DashState dashState { get; private set; } //冲刺状态
    public Player_BasicAttackState basicAttackState { get; private set; } //普通攻击状态
    public Player_JumpAttackState jumpAttackState { get; private set; } //跳跃攻击状态
    public Player_DeadState deadState { get; private set; } //死亡状态
    public Player_CounterAttackState counterAttackState { get; private set; } //反击状态


    public float xInput { get; private set; }
    public float yInput { get; private set; }
    private bool isInputEnabled = true; //是否启用输入

    public SpriteRenderer sr;

    [Header("移动相关")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f; //向上跳跃的力大小
    public Vector2 wallJumpForce; //决定玩家在滑墙跳跃力
    [Range(0, 1)] //在inspector窗口上将出现一个0~1范围的控制条
    public float inAirMoveMultiplier = 0.7f; //玩家在空中移动时的乘数,降低空中移速
    [Range(0, 1)]
    public float inAirFallMultiplier = 0.7f; //在空中下落速度的乘数
    [Space]
    public float dashDuration = 0.25f; //冲刺持续时间
    public float dashSpeed = 10; //冲刺速度
    public float dashCoolTime = 1f; //冲刺冷却时间
    [HideInInspector] public float lastDashTimer;

    [Header("攻击相关")]
    public Vector2[] attackVelocity; //攻击产生的矢量，不同连击段数，矢量大小不同
    public float attackVelocityDuration = 0.1f; //攻击产生的惯性速度的持续时间
    public float resetAttackAfterTime = 2; //玩家攻击一次后，多久重置连击
    private Coroutine queuedAttackCo; //协程类型变量，存储协程

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponentInChildren<SpriteRenderer>();

        idleState = new Player_IdleState(this, fSM, "idle");
        moveState = new Player_MoveState(this, fSM, "move");
        jumpState = new Player_JumpState(this, fSM, "jumpFall");//跳跃和下落动画混合树的bool参数
        fallState = new Player_FallState(this, fSM, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, fSM, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, fSM, "wallJump");
        dashState = new Player_DashState(this, fSM, "dash");
        basicAttackState = new Player_BasicAttackState(this, fSM, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, fSM, "jumpAttack");
        deadState = new Player_DeadState(this, fSM, "dead");
        counterAttackState = new Player_CounterAttackState(this, fSM, "counterAttack");
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        fSM.InitialState(idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        GetInput();
    }


    public override void ObjectBaseDeath()
    {
        base.ObjectBaseDeath();
        onPlayerDeath?.Invoke(); //调用事件，通知敌人转为待机状态
        fSM.ChangeState(deadState);
    }


    public bool CanDash()
    {
        if (Time.time <= lastDashTimer + dashCoolTime)
        {
            return false;
        }

        // 触墙时不能冲刺
        if (isTouchWall)
        {
            return false;
        }

        // 已处于冲刺状态时不能再冲刺,完成一次冲刺后才可冲刺（避免连按）
        if (fSM.currentState == dashState)
        {
            return false;
        }

        return true;
    }

    private void GetInput()
    {
        if (!isInputEnabled)
        {
            xInput = 0;
            yInput = 0;
            return;
        }

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    /// <summary>
    /// 禁用输入
    /// </summary>
    public void DisableInput()
    {
        isInputEnabled = false;
    }

    /// <summary>
    /// 启用输入
    /// </summary>
    public void EnableInput()
    {
        isInputEnabled = true;
    }

    /// <summary>
    ///定义一个协程，延迟进入攻击状态
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayEnterAttackStateCo()
    {
        yield return new WaitForEndOfFrame(); //暂停到当前帧结束
        fSM.ChangeState(basicAttackState);
    }
    /// <summary>
    /// 封装协程方法
    /// </summary>
    public void DelayEnterAttackState()
    {
        //避免触发多个同类型协程：
        //如果当前有一个协程
        if (queuedAttackCo != null)
        {
            //停止这个协程
            StopCoroutine(queuedAttackCo);
        }

        //启动一个协程
        queuedAttackCo = StartCoroutine(DelayEnterAttackStateCo());
    }
}
