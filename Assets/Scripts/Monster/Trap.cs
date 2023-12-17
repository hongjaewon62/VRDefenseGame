using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float trapDuration = 2f;  // ������ ���� �ð�

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster") // ���� ������Ʈ�� ������ ��쿡�� ����
        {
            Monster monster = other.GetComponent<Monster>();

            // ������ �ɸ� ���Ϳ� ���� ó�� �߰������� �ϼ��� ���� �� �����Ѵٸ� Ʈ�� �߾� ��ġ�� �̵���Ű�� �ڵ� �ֱ�
            monster.SetTrapped(trapDuration);
            Debug.Log("catch");
        }
    }
}

