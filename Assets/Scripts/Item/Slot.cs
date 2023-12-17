using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour
{
    public Item item;
    public int itemCount;
    public Transform summonPosition;
    //public bool holding = false;            // 아이템이 손에 있는지 체크

    [SerializeField]
    private TextMeshProUGUI textCount;
    [SerializeField]
    private GameObject watchUi;

    public void Start()
    {
        if (item.name == "CatPunch")
            itemCount = GameManager.gameManager.itemNum[0];
            textCount.text = itemCount.ToString();
        if (item.name == "Lullaby")
            itemCount = GameManager.gameManager.itemNum[1];
            textCount.text = itemCount.ToString();
        if (item.name == "Kitten")
            itemCount = GameManager.gameManager.itemNum[2];
            textCount.text = itemCount.ToString();
        if (item.name == "Manhole")
            itemCount = GameManager.gameManager.itemNum[3];
            textCount.text = itemCount.ToString();
    }

    // 아이템 획득
    public void AddItem(int count = 1)
    {
        if (item.name == "CatPunch")
            GameManager.gameManager.itemNum[0]++;
        if (item.name == "Lullaby")
            GameManager.gameManager.itemNum[1]++;
        if (item.name == "Kitten")
            GameManager.gameManager.itemNum[2]++;
        if (item.name == "Manhole")
            GameManager.gameManager.itemNum[3]++;
        itemCount += count;
        textCount.text = itemCount.ToString();
    }

    // 아이템 개수 조정
    public void SetSlotCount(int count)
    {
        itemCount += count;
        textCount.text = itemCount.ToString();
    }

    public void UseItem()
    {
        if(itemCount > 0)
        {
            if (item.name == "CatPunch")
                GameManager.gameManager.itemNum[0]--;
            if (item.name == "Lullaby")
                GameManager.gameManager.itemNum[1]--;
            if (item.name == "Kitten")
                GameManager.gameManager.itemNum[2]--;
            if (item.name == "Manhole")
                GameManager.gameManager.itemNum[3]--;
            itemCount--;
            textCount.text = itemCount.ToString();
            SummonItem();

            watchUi.SetActive(false);
        }
    }

   public void SummonItem()
    {
        GameObject summonitemObject = Instantiate(item.itemPrefab, summonPosition.transform.position, summonPosition.transform.rotation);
        HoverItem2 hoverItem = summonitemObject.GetComponent<HoverItem2>();
        if (hoverItem != null)
        {
            hoverItem.itemRotation = false;
        }
        summonitemObject.transform.SetParent(summonPosition);
        //summonitemObject.transform.GetComponent<HoverItem2>().itemRotation = false;
        //Instantiate(summonitemObject, summonPosition.transform.position, summonPosition.transform.rotation).transform.SetParent(summonPosition);
        summonitemObject.transform.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Update()
    {
        if (item.name == "CatPunch")
            GameManager.gameManager.itemNum[0] = itemCount;
        if (item.name == "Lullaby")
            GameManager.gameManager.itemNum[1] = itemCount;
        if (item.name == "Kitten")
            GameManager.gameManager.itemNum[2] = itemCount;
        if (item.name == "Manhole")
            GameManager.gameManager.itemNum[3] = itemCount;
    }
 
}
