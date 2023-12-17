using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTest_Item : MonoBehaviour
{
    public GameObject itemPrefab; // ������ ������ ������
    public float dropProbability = 0.5f; // �������� ������ Ȯ�� (0.0 ~ 1.0)

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
            // ������ ���� Ȯ���� ����Ͽ� ������ ���
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // �ش� ������Ʈ �ı�
    }
}
