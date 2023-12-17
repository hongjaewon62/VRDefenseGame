using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // 카메라 바라보고 있게 하는 기능 어디서든 볼 수 있게

    public Transform cam;


    void Update()
    {
        transform.LookAt(cam);
    }
}
