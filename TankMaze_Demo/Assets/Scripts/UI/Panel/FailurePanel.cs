using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 失败面板
/// </summary>
public class FailurePanel : BasePanel
{
    [SerializeField] private Button btnReturnMain; //返回主界面
    [SerializeField] private Button btnAgain; //重新挑战

    // Start is called before the first frame update
    void Start()
    {
        btnReturnMain.onClick.AddListener(() =>
        {
            //取消游戏运行时间暂停
            Time.timeScale = 1;

            SceneManager.LoadScene("BeginScene");
            UI_Manager.Instance.ShowPanel<BeginPanel>();
            UI_Manager.Instance.HideAndRemovePanel<FailurePanel>();
        });

        btnAgain.onClick.AddListener(() =>
        {
            //取消游戏运行时间暂停
            Time.timeScale = 1;

            SceneManager.LoadScene("GameScene");
            UI_Manager.Instance.ShowPanel<GamePanel>();
            UI_Manager.Instance.HideAndRemovePanel<FailurePanel>();
        });
    }


}
