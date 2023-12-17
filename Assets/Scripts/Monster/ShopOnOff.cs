using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOnOff : MonoBehaviour
{

    public GameObject waveSystemObj;
    WaveSystem waveSystem;
    private void Start()
    {
        waveSystem = waveSystemObj.GetComponent<WaveSystem>();
    }
    void Update()
    {
        if(waveSystem.isStarted == true && gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
    }
}
