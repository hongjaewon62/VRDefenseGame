using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    public string objectID;

    private void Awake()
    {
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }

    private void Start()
    {
        for(int i = 0; i< Object.FindObjectsOfType<DontDestroyObject>().Length; i++)
        {
            if(Object.FindObjectsOfType<DontDestroyObject>()[i] != this)
            {
                if(Object.FindObjectsOfType<DontDestroyObject>()[i].objectID == objectID)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
