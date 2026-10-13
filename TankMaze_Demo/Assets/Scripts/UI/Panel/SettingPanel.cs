using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    [SerializeField] private Button btnExit;
    [SerializeField] private Toggle togMusic;
    [SerializeField] private Toggle togSound;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;

    // Start is called before the first frame update
    void Start()
    {
        #region UI控件事件监听
        btnExit.onClick.AddListener(() =>
        {
            //判断当前所处场景
            if (SceneManager.GetActiveScene().name == "BeginScene")
            {
                UI_Manager.Instance.ShowPanel<BeginPanel>();
            }
            else if (SceneManager.GetActiveScene().name == "GameScene")
            {
                UI_Manager.Instance.GetPanel<GamePanel>().ShowMe();
            }
            UI_Manager.Instance.HideAndRemovePanel<SettingPanel>();
        });

        togMusic.onValueChanged.AddListener((isOpen) =>
        {
            GameDataMgr.Instance.SwitchMusic(isOpen);
        });
        togSound.onValueChanged.AddListener((isOpen) =>
        {
            GameDataMgr.Instance.SwitchSound(isOpen);
        });

        sliderMusic.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.ChangeMusicValue(value);
            if (sliderMusic.value > 0)
            {
                togMusic.isOn = true;
            }
        });
        sliderSound.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.ChangeSoundValue(value);
            if (sliderSound.value > 0)
            {
                togSound.isOn = true;
            }
        });
        #endregion
    }

    public override void ShowMe()
    {
        base.ShowMe();
        UpdatePanelInfo();
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        //游戏运行时间计算继续
        Time.timeScale = 1;
    }

    /// <summary>
    /// 更新面板信息
    /// </summary>
    private void UpdatePanelInfo()
    {
        MusicData date = GameDataMgr.Instance.musicData;
        togMusic.isOn = date.isOpenMusic;
        togSound.isOn = date.isOpenSound;
        sliderMusic.value = date.musicValue;
        sliderSound.value = date.soundValue;
    }
}
