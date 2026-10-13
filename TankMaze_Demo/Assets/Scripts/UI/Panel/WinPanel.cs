using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 通关面板
/// </summary>
public class WinPanel : BasePanel
{
    [SerializeField] private InputField inputName;
    [SerializeField] private Button btnSure;
    // Start is called before the first frame update
    void Start()
    {
        btnSure.onClick.AddListener(() =>
        {
            //取消游戏运行时间暂停
            Time.timeScale = 1;
            //在排行榜中添加数据
            GameDataMgr.Instance.AddRankData(inputName.text,
                GamePanel.Instance.score,
                GamePanel.Instance.currentTime);

            SceneManager.LoadScene("BeginScene");
            UI_Manager.Instance.ShowPanel<BeginPanel>();
            UI_Manager.Instance.HideAndRemovePanel<WinPanel>();
        });
    }
}
