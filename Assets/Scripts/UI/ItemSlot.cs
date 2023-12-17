using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public static bool watchUiActivated = false;

    [SerializeField]
    private GameObject itemSlotPanel;
    [SerializeField]
    private GameObject itemSlotParent;

    private Slot[] slots;

    private void Start()
    {
        slots = itemSlotParent.GetComponentsInChildren<Slot>();
        itemSlotPanel.SetActive(false);
    }

    public void CloseWatchUI()
    {
        watchUiActivated = false;
    }

    public void AcquireItem(Item item, int count = 1)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item != null)
            {
                if (slots[i].item.itemName == item.itemName)
                {
                    slots[i].SetSlotCount(count);
                    return;
                }
            }
        }
    }
}
