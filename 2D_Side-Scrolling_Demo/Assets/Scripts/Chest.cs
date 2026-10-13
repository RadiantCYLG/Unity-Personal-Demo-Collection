using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IDamage
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private ObjBase_VFX vfx => GetComponent<ObjBase_VFX>();

    [Header("打开相关")]
    [SerializeField] private Vector2 knockbackAtOpen = new Vector2(1, 1);

    public void TakeDamage(float damage, Transform damageDealer)
    {
        vfx.PlayOnDamageVfx();
        anim.SetBool("chestOpen", true);
        rb.velocity = knockbackAtOpen;
        rb.angularVelocity = Random.Range(-200f, 201f);
    }

}
