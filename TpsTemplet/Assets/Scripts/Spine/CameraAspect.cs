using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code by https://drive.google.com/drive/folders/1INs0qIhRiWztfCs06NcDXISV-Md2WvTf

public class CameraAspect : MonoBehaviour
{
    //Aspect�� �� ũ�⿡ �µ��� ���� �ʿ� -> ���� ���� ���� �ٲٸ� �ɵ�
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
