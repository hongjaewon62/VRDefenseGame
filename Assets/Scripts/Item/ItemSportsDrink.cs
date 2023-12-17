using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSportsDrink : MonoBehaviour
{
    public float defaultSpeed = 0.1f;                       // 기본 속도 0.1f
    public float increaseSpeed = 0.3f;                      // 증가 속도
    public float duration = 5.0f;

    public void SpeedIncrease()
    {
        Transform rootTransform = gameObject.transform.root;
        rootTransform.GetComponent<PlayerMovement>().StartCoroutine(rootTransform.GetComponent<PlayerMovement>().SpeedIncreaseCoroutine(duration, defaultSpeed, increaseSpeed));
        Destroy(gameObject);
    }
}
