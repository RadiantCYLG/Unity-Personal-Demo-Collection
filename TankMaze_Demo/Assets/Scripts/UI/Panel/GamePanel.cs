using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    //单例模式
    private static GamePanel instance;
    public static GamePanel Instance => instance;

    [SerializeField] private RectTransform imgHp;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnExit;
    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtTime;
    //玩家血量显示Image尺寸
    [SerializeField] private float imgHpMaxWidth = 500;
    [SerializeField] private float imgHpMaxHeight = 40;
    //游戏上方的时间计时
    [HideInInspector]
    public float currentTime;
    //得分
    [HideInInspector]
    public int score;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        btnSetting.onClick.AddListener(() =>
        {
            UI_Manager.Instance.ShowPanel<SettingPanel>();
            //暂停游戏运行时间，时间系数为0，时间停止，系数为1，时间计算继续
            Time.timeScale = 0;
        });

        btnExit.onClick.AddListener(() =>
        {
            UI_Manager.Instance.ShowPanel<ExitPanel>();
            Time.timeScale = 0;
        });
    }


    //必须override并调用base.Update()，否则会隐藏基类的Update（淡入淡出与移除逻辑都在那里执行）
    protected override void Update()
    {
        base.Update();
        UpdateCurrentTime();
    }

    /// <summary>
    /// 更新游戏时间的显示
    /// </summary>
    private void UpdateCurrentTime()
    {
        currentTime += Time.deltaTime;
        //时间存储单位是 秒
        //把秒数转换为 时 分 秒
        int time = (int)currentTime;
        txtTime.text = ""; //先清空通关时间上的文字
        if (time / 3600 > 0)
        {
            txtTime.text += time / 3600 + "时";
        }
        if (time % 3600 / 60 > 0 || txtTime.text != "")//不足一小时的除以分钟, 0分钟也记录上去
        {
            txtTime.text += time % 3600 / 60 + "分";
        }
        txtTime.text += time % 60 + "秒";
    }

    /// <summary>
    /// 加分
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        this.score += score;
        txtScore.text = this.score.ToString();
    }

    /// <summary>
    /// 更新玩家血量
    /// </summary>
    /// <param name="currentHp">当前血量</param>
    public void UpdatePlayerHp(int currentHp, int maxHp)
    {
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); //钳制函数，currentHp在最大值和最小值之间，取currentHp
        imgHp.sizeDelta = new Vector2((float)currentHp / maxHp * imgHpMaxWidth, imgHpMaxHeight);
    }

}
