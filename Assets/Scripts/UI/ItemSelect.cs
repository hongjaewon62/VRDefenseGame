using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour
{
    public GameObject itemDisplay;                                  // 아이템을 전시할 목록
    public static string nowOn = "";                                // 현재 세부 사항에 표시되는 아이템 이름
    
    public static List<KeyValuePair<string, GameObject>> items;     // 전시할 목록을 리스트로 저장

    public string content;                                          // 이 스크립트를 넣은 객체의 설명
    public int cost;                                                // 구매 비용
    public Text displayContent;                                     // 설명을 표시할 Text
    public Text displayCost;                                        // 구매 비용을 표시할 Text
    public Text displayMoney;                                       // 남은 비용을 표시할 Text

    public void Awake()
    {
        items = new List<KeyValuePair<string, GameObject>>();

        Debug.Assert(itemDisplay != null, "Error (GameObject is Null) : 전시할 위치가 존재하지 않습니다.");

        // 아이템할 목록을 가져와 이름과 오브젝트가 저장되는 리스트에 저장
        for (int i = 0; i < itemDisplay.transform.childCount; i++)
        {
            GameObject inObject = itemDisplay.transform.GetChild(i).gameObject;

            items.Add(new KeyValuePair<string, GameObject>(inObject.name, inObject));
        }

        // 첫번째 아이템을 세부사항에 표시
        nowOn = items[0].Key;
    }

    public void Start()
    {
        // 현재 금액을 가져옴
        int currency = GameManager.gameManager.currency;

        if (displayContent.text.CompareTo("기적의 발바닥이다.") != 0)
            return;

        // 전시할 가격과 현재 가격을 출력함
        displayContent.text = content;
        displayCost.text = cost.ToString() + "$";
        displayMoney.text = currency.ToString() + "$";

        // 비용이 적절한지 판단하여 색깔을 바꿈
        if (cost > currency)
            displayCost.color = Color.red;

        else
            displayCost.color = Color.green;
    }

    public void Update()
    {
        
    }

    public void ItemClick()
    {
        // 같은 것을 클릭할 경우 중지
        if (nowOn == gameObject.name)
            return;

        // 비용이 적절한지 판단하여 색깔을 바꿈
        if (cost > GameManager.gameManager.currency)
            displayCost.color = Color.red;

        else
            displayCost.color = Color.green;

        // 아이템 목록에서 현재 켜져있는 아이템을 끔
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Key == nowOn)
            {
                items[i].Value.gameObject.SetActive(false);
                break;
            }
        }
        
        // 아이템 목록에서 클릭한 아이템을 킴
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Key == gameObject.name)
            {
                items[i].Value.gameObject.SetActive(true);
                nowOn = items[i].Key;                       // 선택한 아이템 변경

                // 선택한 아이템를 현재 세부사항에 표시
                displayContent.text = content;
                displayCost.text = cost.ToString() + "$";
                break;
            }
        }
    }
}