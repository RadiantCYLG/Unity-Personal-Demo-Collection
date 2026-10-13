using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    //按钮
    [SerializeField] private Button btnBegin;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnExitGame;
    [SerializeField] private Button btnRank;//排行榜

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        btnBegin.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
            Cursor.visible = false; //隐藏鼠标
            Cursor.lockState = CursorLockMode.Confined; //将鼠标限制在窗口大小内
            UI_Manager.Instance.ShowPanel<GamePanel>();
            UI_Manager.Instance.HideAndRemovePanel<BeginPanel>(false);
        });

        btnSetting.onClick.AddListener(() =>
        {
            //显示设置界面
            UI_Manager.Instance.ShowPanel<SettingPanel>();
            UI_Manager.Instance.HideAndRemovePanel<BeginPanel>();
        });

        btnExitGame.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        btnRank.onClick.AddListener(() =>
        {
            //排行榜界面
            UI_Manager.Instance.ShowPanel<RankPanel>();
            UI_Manager.Instance.HideAndRemovePanel<BeginPanel>();
        });
    }
}
