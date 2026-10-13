using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UI_Manager.Instance.ShowPanel<LoginBackgroundPanel>();
        UI_Manager.Instance.ShowPanel<LoginPanel>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
