using UnityEngine;

public class DetailDisplayDefualt : MonoBehaviour
{
    public void Awake()
    {
        // 상점의 디테일에 첫번째 아이템을 제외한 상세정보에 아무런 아이템이 나오지 않게 설정함
        for (int i = 1; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }
}
