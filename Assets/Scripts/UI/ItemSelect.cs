using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour
{
    public GameObject itemDisplay;                                  // �������� ������ ���
    public static string nowOn = "";                                // ���� ���� ���׿� ǥ�õǴ� ������ �̸�
    
    public static List<KeyValuePair<string, GameObject>> items;     // ������ ����� ����Ʈ�� ����

    public string content;                                          // �� ��ũ��Ʈ�� ���� ��ü�� ����
    public int cost;                                                // ���� ���
    public Text displayContent;                                     // ������ ǥ���� Text
    public Text displayCost;                                        // ���� ����� ǥ���� Text
    public Text displayMoney;                                       // ���� ����� ǥ���� Text

    public void Awake()
    {
        items = new List<KeyValuePair<string, GameObject>>();

        Debug.Assert(itemDisplay != null, "Error (GameObject is Null) : ������ ��ġ�� �������� �ʽ��ϴ�.");

        // �������� ����� ������ �̸��� ������Ʈ�� ����Ǵ� ����Ʈ�� ����
        for (int i = 0; i < itemDisplay.transform.childCount; i++)
        {
            GameObject inObject = itemDisplay.transform.GetChild(i).gameObject;

            items.Add(new KeyValuePair<string, GameObject>(inObject.name, inObject));
        }

        // ù��° �������� ���λ��׿� ǥ��
        nowOn = items[0].Key;
    }

    public void Start()
    {
        // ���� �ݾ��� ������
        int currency = GameManager.gameManager.currency;

        if (displayContent.text.CompareTo("������ �߹ٴ��̴�.") != 0)
            return;

        // ������ ���ݰ� ���� ������ �����
        displayContent.text = content;
        displayCost.text = cost.ToString() + "$";
        displayMoney.text = currency.ToString() + "$";

        // ����� �������� �Ǵ��Ͽ� ������ �ٲ�
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
        // ���� ���� Ŭ���� ��� ����
        if (nowOn == gameObject.name)
            return;

        // ����� �������� �Ǵ��Ͽ� ������ �ٲ�
        if (cost > GameManager.gameManager.currency)
            displayCost.color = Color.red;

        else
            displayCost.color = Color.green;

        // ������ ��Ͽ��� ���� �����ִ� �������� ��
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Key == nowOn)
            {
                items[i].Value.gameObject.SetActive(false);
                break;
            }
        }
        
        // ������ ��Ͽ��� Ŭ���� �������� Ŵ
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Key == gameObject.name)
            {
                items[i].Value.gameObject.SetActive(true);
                nowOn = items[i].Key;                       // ������ ������ ����

                // ������ �����۸� ���� ���λ��׿� ǥ��
                displayContent.text = content;
                displayCost.text = cost.ToString() + "$";
                break;
            }
        }
    }
}