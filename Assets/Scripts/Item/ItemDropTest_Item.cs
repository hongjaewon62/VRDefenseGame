using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTest_Item : MonoBehaviour
{
    public GameObject itemPrefab; // 생성될 아이템 프리팹
    public float dropProbability = 0.5f; // 아이템이 생성될 확률 (0.0 ~ 1.0)

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            DestroyAndDrop();
        }    
    }

    public void DestroyAndDrop()
    {
        if (Random.value <= dropProbability)
        {
            // 아이템 생성 확률을 계산하여 생성할 경우
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // 해당 오브젝트 파괴
    }
}
