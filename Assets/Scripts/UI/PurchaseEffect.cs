using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseEffect : MonoBehaviour
{
    public GameObject coin;                         // 코인 이펙트를 실행할 오브젝트
    public Text displayCost;                        // 구매 비용을 표시할 Text
    public Text displayMoney;                       // 남은 금액을 표시할 Text
    public float moveSpeed;                         // 코인의 이동 속도
    public float fadeSpeed;                         // 사라질 때 투명해지는 속도
    public static bool isPlayCoin = false;          // 코인 이펙트 재생 여부
#pragma warning disable CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.
    private Renderer renderer;                      // 코인의 랜더러
    private AudioSource audio;                      // 재생할 플레이어


    private float currentAlpha = 1.0f;              // 현재 코인의 투명도

    public GameObject itemSlotPanel;
    private Dictionary<string, GameObject> itemSlot;// 아이템 이름에 해당하는 슬롯을 저장할 배열


    public void Start()
    {
        audio = GetComponent<AudioSource>();

        Debug.Assert(coin != null, "Error (GameObject is Null) : 코인 오브젝트가 존재하지 않습니다.");
        Debug.Assert(audio != null, "Error (AudioSource is Null) : 오디오가 존재하지 않습니다.");
        Debug.Assert(itemSlotPanel != null, "Error (GameObject is Null) : 아이템 슬롯 패널이 존재하지 않습니다.");

        renderer = coin.GetComponent<Renderer>();
        coin.SetActive(false);

        itemSlot = new Dictionary<string, GameObject>();
        for (int i = 0; i < itemSlotPanel.transform.childCount; i++)
        {
            GameObject slot = itemSlotPanel.transform.GetChild(i).gameObject;
            string name = slot.GetComponent<Slot>().item.itemName;

            if (name == "CatPaw")
                name = "CatPunch";

            itemSlot.Add(name, slot);
        }
    }

    public void Update()
    {
        if (displayMoney.text != GameManager.gameManager.currency.ToString() + "$")
            displayMoney.text = GameManager.gameManager.currency.ToString() + "$";

        // 코인으로 구매중인 경우
        if (isPlayCoin)
            CoinRise();
    }

    public void BuyItem()
    {
        int getCurrency = GameManager.gameManager.currency;
        int getCost = Int32.Parse(displayCost.text.Substring(0, displayCost.text.Length - 1));

        // 재화가 부족할 경우
        if (getCurrency < getCost)
        {
            // 구매 실패 소리 재생
            AudioClip clip = audio.clip;
            audio.PlayOneShot(clip);

            return;
        }

        // 구입한 가격을 설정
        ShopButton.BuyItem(getCost);
        
        UiManager.uiManager.UpdateCurrencyText(GameManager.gameManager.currency);

        // 비용이 적절한지 판단하여 색깔을 바꿈
        if (getCost > GameManager.gameManager.currency)
            displayCost.color = Color.red;

        else
            displayCost.color = Color.green;

        itemSlot[ItemSelect.nowOn].GetComponent<Slot>().AddItem();
        isPlayCoin = true;
    }

    public void CoinRise()
    {
        // 코인이 올라갈 때 Start
        if (!coin.activeSelf)
        {
            coin.SetActive(true);
            coin.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            // 코인을 다시 보이게 설정
            currentAlpha = 1.0f;
            Color getColor = renderer.material.color;
            getColor.a = currentAlpha;
            renderer.material.color = getColor;
        }

        // 오브젝트를 위로 이동
        coin.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 투명도를 감소시키기
        currentAlpha -= fadeSpeed * Time.deltaTime;

        // 오브젝트 렌더러의 투명도 설정
        Color currentColor = renderer.material.color;
        currentColor.a = Mathf.Max(currentAlpha, 0);
        renderer.material.color = currentColor;

        // 투명도가 최소값 이하로 떨어지면 오브젝트 비활성화
        if (currentAlpha <= 0)
        {
            coin.SetActive(false);
            isPlayCoin = false;
        }
    }
}
