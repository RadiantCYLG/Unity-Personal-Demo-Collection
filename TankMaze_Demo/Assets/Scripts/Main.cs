using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UI_Manager.Instance.ShowPanel<BeginPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
