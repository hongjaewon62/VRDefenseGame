using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse : MonoBehaviour
{
    public GameObject leftGrabPositon;
    [SerializeField]
    private InstallObject installObject;

    private bool isCooldown = false;

    public void Use()
    {
        if(!isCooldown)
        {
            if (leftGrabPositon.GetComponentInChildren<ItemPickUp>() != null)                                               // 손에 아이템이 있는지 체크
            {
                Item.ItemType item = leftGrabPositon.transform.GetChild(1).GetComponent<ItemPickUp>().item.itemType;        //  itemType을 할당
                if (item == Item.ItemType.CatPunch)
                {
                    Transform itemObject = leftGrabPositon.transform.GetChild(1);                                           // 손의 1번째 자식 오브젝트 할당(위치가 바뀌면 수정)
                    itemObject.GetComponent<ItemCatPunch>().StartCatPunchCoroutine();
                }
                //else if (item == Item.ItemType.SportsDrink)
                //{
                //    Transform itemObject = leftGrabPositon.transform.GetChild(1);                                     // 손의 1번째 자식 오브젝트 할당(위치가 바뀌면 수정)
                //    itemObject.GetComponent<ItemSportsDrink>().SpeedIncrease();
                //}
                else if(item == Item.ItemType.Lullaby)
                {
                    if (installObject.isPreviewActivated == false)
                    {
                        installObject.Installation(2);
                    }
                    else if (installObject.clickNum > 0)
                    {
                        Transform itemObject = leftGrabPositon.transform.GetChild(1);
                        installObject.Build();
                        Destroy(itemObject.gameObject);
                    }
                    installObject.clickNum++;
                }
                else if (item == Item.ItemType.Kitten)
                {
                    if (installObject.isPreviewActivated == false)
                    {
                        installObject.Installation(0);
                    }
                    else if (installObject.clickNum > 0)
                    {
                        Transform itemObject = leftGrabPositon.transform.GetChild(1);
                        installObject.Build();
                        Destroy(itemObject.gameObject);
                    }
                    installObject.clickNum++;
                }
                else if (item == Item.ItemType.Trap)
                {
                    if (installObject.isPreviewActivated == false)
                    {
                        installObject.Installation(1);
                    }
                    else if (installObject.clickNum > 0)
                    {
                        Transform itemObject = leftGrabPositon.transform.GetChild(1);
                        installObject.Build();
                        Destroy(itemObject.gameObject);
                    }
                    installObject.clickNum++;
                }

                StartCoroutine(Cooldown());
            }
        }
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(0.2f);
        isCooldown = false;
    }
}
