using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : ObjBase_Combat
{
    [Header("反击详情")]
    [SerializeField] private float counterDuration = 0.25f;

    /// <summary>
    /// 是否已完成反击
    /// </summary>
    /// <returns></returns>
    public bool IsCounterAttackExecute()
    {
        bool hasCounterTarget = false; //定义返回值

        foreach (var target in GetDetectedColliders())
        {
            //获取的继承了ICounter的target
            ICounter counter = target.GetComponent<ICounter>();
            if (counter == null)
            {
                continue;
            }

            if ((counter as Enemy_Skeleton).canBeCounter)
            {
                //敌人陷入眩晕状态
                counter.CheckBeCounter();
                //说明已经反击到目标了
                hasCounterTarget = true;
            }
        }

        return hasCounterTarget;
    }

    public float GetCounterRecoveryTime()
    {
        return counterDuration;
    }
}
