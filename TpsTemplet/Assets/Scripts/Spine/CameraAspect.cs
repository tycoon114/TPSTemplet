using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code by https://drive.google.com/drive/folders/1INs0qIhRiWztfCs06NcDXISV-Md2WvTf

public class CameraAspect : MonoBehaviour
{
    //Aspect는 내 크기에 맞도록 수정 필요 -> 설정 값에 따라 바꾸면 될듯
    float targetAspect = 4f / 3f;
    float initOrthographicSize;

    // Start is called before the first frame update
    void Start()
    {
        initOrthographicSize = Camera.main.orthographicSize;
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        if (targetAspect < screenAspect)
        {
            Camera.main.orthographicSize = initOrthographicSize * (targetAspect / Camera.main.aspect);
        }
        else
        {
            Camera.main.orthographicSize = initOrthographicSize;
        }
    }
}
