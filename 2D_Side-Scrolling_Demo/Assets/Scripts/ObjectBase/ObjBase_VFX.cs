using System.Collections;
using UnityEngine;

public class ObjBase_VFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("受伤特效")]
    [SerializeField] private Material onDamageMaterial; //受伤时的Material
    [SerializeField] private float onDamageVfxDuration = 0.2f; //受伤视觉效果持续时间
    private Material originalMaterial; //最初的Material
    private Coroutine onDamageVfxCoroutine;

    [Header("击中特效")]
    [SerializeField] private GameObject onHitVfx;
    [SerializeField] private Color onHitVfxColor = Color.white;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>(); //获取脚本挂载对象的子对象上的SpriteRenderer组件
        originalMaterial = sr.material; //初始化originalMaterial
    }

    /// <summary>
    /// 在攻击目标身上创建攻击特效
    /// </summary>
    /// <param name="target"></param>
    public void CreateVfxOnHit(Transform target)
    {
        GameObject vfxPrefab = Instantiate(onHitVfx, target.transform.position, Quaternion.identity);
        vfxPrefab.GetComponentInChildren<SpriteRenderer>().color = onHitVfxColor;
    }

    /// <summary>
    /// 播放受伤的视觉效果
    /// </summary>
    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
        {
            StopCoroutine(onDamageVfxCoroutine);
        }
        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }
    /// <summary>
    /// 处理控制视觉效果的Material的协程
    /// </summary>
    /// <returns>Coroutine（协程）类型</returns>
    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration); //等待设定的时间后
        sr.material = originalMaterial;
    }
}
