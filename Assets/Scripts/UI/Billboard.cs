using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // ī�޶� �ٶ󺸰� �ְ� �ϴ� ��� ��𼭵� �� �� �ְ�

    public Transform cam;


    void Update()
    {
        transform.LookAt(cam);
    }
}
