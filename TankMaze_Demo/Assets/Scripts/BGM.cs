using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private static BGM instance;
    public static BGM Instance => instance;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        //回到BeginScene等已摆有BGM的场景时，常驻BGM已存在，销毁场景中多余的自己，避免覆盖单例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        audioSource = GetComponent<AudioSource>();
        //在保存的数据中获取音乐的设置
        SwitchAudio(GameDataMgr.Instance.musicData.isOpenMusic);
        ChangeValue(GameDataMgr.Instance.musicData.musicValue);
        audioSource.Play();
    }


    // Start is called before the first frame update
    void Start()
    {
        //过场景时，不删除BGM对象
        GameObject.DontDestroyOnLoad(instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 开关音频
    /// </summary>
    public void SwitchAudio(bool isOpen)
    {
        //mute表示静音
        audioSource.mute = !isOpen;
    }

    /// <summary>
    /// 改变音频大小
    /// </summary>
    public void ChangeValue(float value)
    {
        audioSource.volume = value;
    }
}
