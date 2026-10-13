using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitPanel : BasePanel
{
    [SerializeField] private Button btnSure;
    [SerializeField] private Button btnContinue;

    void Start()
    {
        btnSure.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("BeginScene");
            UI_Manager.Instance.ShowPanel<BeginPanel>();
            UI_Manager.Instance.HideAndRemovePanel<GamePanel>();
            UI_Manager.Instance.HideAndRemovePanel<ExitPanel>();
        });

        btnContinue.onClick.AddListener(() =>
        {
            UI_Manager.Instance.HideAndRemovePanel<ExitPanel>();
        });
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        //Ê±¼ä¼ÌÐø
        Time.timeScale = 1;
    }
}
