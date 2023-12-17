using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSportsDrink : MonoBehaviour
{
    public float defaultSpeed = 0.1f;                       // �⺻ �ӵ� 0.1f
    public float increaseSpeed = 0.3f;                      // ���� �ӵ�
    public float duration = 5.0f;

    public void SpeedIncrease()
    {
        Transform rootTransform = gameObject.transform.root;
        rootTransform.GetComponent<PlayerMovement>().StartCoroutine(rootTransform.GetComponent<PlayerMovement>().SpeedIncreaseCoroutine(duration, defaultSpeed, increaseSpeed));
        Destroy(gameObject);
    }
}
