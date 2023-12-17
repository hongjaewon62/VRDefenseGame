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
            if (leftGrabPositon.GetComponentInChildren<ItemPickUp>() != null)                                               // �տ� �������� �ִ��� üũ
            {
                Item.ItemType item = leftGrabPositon.transform.GetChild(1).GetComponent<ItemPickUp>().item.itemType;        //  itemType�� �Ҵ�
                if (item == Item.ItemType.CatPunch)
                {
                    Transform itemObject = leftGrabPositon.transform.GetChild(1);                                           // ���� 1��° �ڽ� ������Ʈ �Ҵ�(��ġ�� �ٲ�� ����)
                    itemObject.GetComponent<ItemCatPunch>().StartCatPunchCoroutine();
                }
                //else if (item == Item.ItemType.SportsDrink)
                //{
                //    Transform itemObject = leftGrabPositon.transform.GetChild(1);                                     // ���� 1��° �ڽ� ������Ʈ �Ҵ�(��ġ�� �ٲ�� ����)
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
