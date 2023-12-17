using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearIsActive : MonoBehaviour
{
    private bool isOne = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!isOne)
        {
            gameObject.SetActive(false);
            isOne = true;
        }
    }
}
