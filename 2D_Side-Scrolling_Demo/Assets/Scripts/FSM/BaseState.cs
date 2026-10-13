using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家和敌人的状态基类，定义所有状态必须具备的生命周期
//设计思想————每个状态都新建一个类，每个状态的逻辑都在自己的类文件里
public abstract class BaseState
{
    protected FSM stateMachine;
    protected string animBoolName; //作用于Animator的参数
    protected float stateTimer; //状态计时器，给需要的状态用
    protected bool isTriggerCalled; //攻击动画结束触发器

    protected Animator anim;

    protected BaseState(FSM stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }


    //规定该状态怎么开始
    public virtual void OnEnter()
    {
        //播放当前进入的状态的动画
        anim.SetBool(animBoolName, true);
        isTriggerCalled = false; //每次更新状态时，都将重置触发器为false
    }

    //规定该状态怎么持续
    public virtual void OnUpdate()
    {
        //Debug.Log($"运行{animBoolName}状态");
        stateTimer -= Time.deltaTime;//stateTimer逐渐减少
        UpdateAnimatorParameters();
    }

    //规定该状态怎么退出
    public virtual void OnExit()
    {
        //结束当前状态的动画
        anim.SetBool(animBoolName, false);
    }

    /// <summary>
    /// 攻击动画结束触发方法
    /// </summary>
    public void AnimTrigger()
    {
        isTriggerCalled = true;
    }

    /// <summary>
    /// 更新Animator参数
    /// </summary>
    protected virtual void UpdateAnimatorParameters()
    {

    }
}
