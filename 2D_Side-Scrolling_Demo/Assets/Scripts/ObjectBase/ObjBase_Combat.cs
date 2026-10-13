using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//处理玩家或敌人的攻击
public class ObjBase_Combat : MonoBehaviour
{
    private ObjBase_VFX vfx;

    public float damage = 10f;
    [Header("攻击详情")]
    [SerializeField] private Transform targetCheckPoint; //圆形检测起始点，圆心
    [SerializeField] private float targetCheckRadius = 0.7f; //检测半径
    [SerializeField] private LayerMask whatIsTarget; //检测目标

    private void Awake()
    {
        vfx = GetComponent<ObjBase_VFX>();
    }

    /// <summary>
    /// 执行攻击，对目标造成伤害
    /// </summary>
    public void ExecuteAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            //里氏替换原则（Chest类和Object_Health类都继承了IDamage接口）
            IDamage iDamage = target.GetComponent<IDamage>();
            if (iDamage == null)
                continue;

            iDamage.TakeDamage(damage, transform);
            vfx.CreateVfxOnHit(target.transform); 
        }
    }

    /// <summary>
    /// 获取检测到的Collider2D
    /// </summary>
    /// <returns>Collider2D型数组</returns>
    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheckPoint.position, targetCheckRadius, whatIsTarget);
    }

    /// <summary>
    /// Scene窗口绘制
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheckPoint.position, targetCheckRadius);
    }
}
