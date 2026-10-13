using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Controller : MonoBehaviour
{
    [SerializeField] private bool isAutoDestroy = true;
    [SerializeField] private float destroyAfterTime = 1;
    [Space]
    [SerializeField] private bool isRandomOffset = true;
    [SerializeField] private bool isRandomRotate = true;

    [Header("特效位置随机")]
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.3f;
    [Space]
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        ApplyRandomOffset();
        ApplyRandomRotate();

        if (isAutoDestroy)
        {
            //1s后销毁本身
            Destroy(gameObject, destroyAfterTime);
        }
    }

    private void ApplyRandomOffset()
    {
        if (!isRandomOffset)
        {
            return;
        }

        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position += new Vector3(xOffset, yOffset);
    }

    private void ApplyRandomRotate()
    {
        if (!isRandomRotate)
        {
            return;
        }

        float zRotation = Random.Range(0, 361);
        transform.Rotate(0, 0, zRotation);
    }
}
