using UnityEngine;
using UnityEngine.EventSystems;

public class TankPlayer : TankBaseObj
{
    private float xInput;
    private float zInput;
    private float mouseX;

    [Header("玩家坦克头部旋转相关")]
    [SerializeField] private float sensitvity = 3; //炮管转向灵敏
    [SerializeField] private float maxHeadAngle = 60; // 炮塔左右最大角度
    private float headAngleY; // 炮塔当前相对初始朝向的 Y 轴偏移角
    private Quaternion orignialHeadLocalRot; // 炮塔初始本地旋转

    [Space]
    [SerializeField] private Transform weaponPos; //武器创建的位置
    [Space]
    [SerializeField] private float fireIntervalTime = 0.8f;
    private float timer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        atk = 20;
        // 记录炮塔初始本地旋转，避免直接赋值时丢失初始朝向
        orignialHeadLocalRot = tankHead.localRotation;
        headAngleY = 0f;
        //初始化显示玩家血条
        GamePanel.Instance.UpdatePlayerHp(currentHp, maxHp);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MoveAndRotate();
        CheckFire();
        //GamePanel.Instance.UpdatePlayerHp(currentHp, maxHp);
    }

    /// <summary>
    /// 移动和转向
    /// </summary>
    private void MoveAndRotate()
    {
        //移动
        transform.Translate(Vector3.forward * zInput * moveSpeed * Time.deltaTime);
        //旋转
        transform.Rotate(Vector3.up * xInput * rotateSpeed * Time.deltaTime);

        //头部和炮管朝向
        headAngleY += mouseX * sensitvity; // 鼠标水平输入累加到炮塔角度
        // 限制炮塔左右最大角度
        headAngleY = Mathf.Clamp(headAngleY, -maxHeadAngle, maxHeadAngle);
        // 用四元数构造绕 Y 轴旋转 offsetRot
        Quaternion offsetRot = Quaternion.AngleAxis(headAngleY, Vector3.up);
        // 左乘：在父级空间（车体空间）中绕 Y 轴旋转
        tankHead.localRotation = offsetRot * orignialHeadLocalRot;
    }

    /// <summary>
    /// 获取输入
    /// </summary>
    private void GetInput()
    {
        //游戏运行时间暂停，不响应输入控制玩家
        if (Time.timeScale == 0f)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        //Input.GetAxis表示-1~1之间平滑变化 
        // ws控制前后移动
        zInput = Input.GetAxis("Vertical");
        //ad控制转向
        xInput = Input.GetAxis("Horizontal");
        //鼠标横向变化控制坦克炮管朝向
        mouseX = Input.GetAxis("Mouse X");
    }

    /// <summary>
    /// 检测开火
    /// </summary>
    private void CheckFire()
    {
        //游戏运行时间暂停，不响应开火
        if (Time.timeScale == 0f)
        {
            return;
        }

        // 鼠标悬停在 UI 上时，不响应开火
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        timer += Time.deltaTime;
        //按下鼠标左键，玩家坦克开火
        if (Input.GetMouseButtonDown(0)&& timer > fireIntervalTime)
        {
            timer = 0;
            Fire();
        }
    }

    /// <summary>
    /// 更换武器
    /// </summary>
    public void ChangeWeapon(Weapon weapon)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
        }

        //实例化创建武器对象
        Weapon weaponObj = GameObject.Instantiate(weapon);
        //设置武器的直接父对象，让武器在其下创建
        weaponObj.transform.SetParent(weaponPos, false);
        //当前武器变更为已获取的武器
        currentWeapon = weaponObj;
        currentWeapon.SetOwner(this);
    }

    public override void Fire()
    {
        if (currentWeapon != null)
        {
            //设置炮管的拥有者
            currentWeapon.SetOwner(this);
            //炮管开火
            currentWeapon.Fire();
        }
    }

    public override void BeHurt(TankBaseObj other)
    {
        base.BeHurt(other);
    }

    protected override void Die()
    {
        base.Die();
        //暂停时间
        Time.timeScale = 0;
        Cursor.visible = true;
        UI_Manager.Instance.ShowPanel<FailurePanel>();
        UI_Manager.Instance.HideAndRemovePanel<GamePanel>();
    }
}
