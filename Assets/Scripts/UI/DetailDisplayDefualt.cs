using UnityEngine;

public class DetailDisplayDefualt : MonoBehaviour
{
    public void Awake()
    {
        // ������ �����Ͽ� ù��° �������� ������ �������� �ƹ��� �������� ������ �ʰ� ������
        for (int i = 1; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
}
