using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//选服面板左侧的按钮
public class BtnServerLeft : MonoBehaviour
{
    [SerializeField] private Button btnSelf; //按钮自身
    [SerializeField] private Text txtName; //显示区服范围的名

    //区服范围
    private int beginIndex;
    private int endIndex;

    private AudioClip clickAudio;

    // Start is called before the first frame update
    void Start()
    {
        clickAudio = Resources.Load<AudioClip>("Sounds/Arknights_ButtonClickAudio1");

        btnSelf.onClick.AddListener(() =>
        {
            UI_Manager.Instance.PlayBtnSound(clickAudio);

            //通知选服面板，改变右侧的显示内容
            SelectServerPanel selectServerPanel = UI_Manager.Instance.GetPanel<SelectServerPanel>();
            selectServerPanel.UpdateServerTheRightShow(beginIndex, endIndex);
        });
    }

    /// <summary>
    /// 通过外部传入起始值，确定服务器范围
    /// </summary>
    public void InitInfo(int beginIndex, int endIndex)
    {
        this.beginIndex = beginIndex;
        this.endIndex = endIndex;
        //确定按钮显示的区服范围
        txtName.text = beginIndex + " - " + endIndex + " 区";
    }
}
