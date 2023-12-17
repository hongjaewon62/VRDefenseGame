using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public int price; // ��ǰ ����
    public GameObject ShopUi;
    public GameObject grab;
    [SerializeField] private int[] upgradeCosts = new int[] { 0, 100, 200 }; // �� ���� ������ ���׷��̵� ��� �迭

    public static void BuyItem(int price)
    {
        if (GameManager.gameManager.SpendCurrency(price)) // ���� �Ŵ��� �ڵ� �� ������ true �� �� �Ҹ� ������ false
        {
            // ��ǰ ���ſ� �������� �� ��� �ۼ��ϱ�
            Debug.Log("���ż���");
        }
        else
        {
            // ��ǰ ���ſ� ������ ��� �޽����� �� ������ �˷��ִ� ǥ�� �����ֱ�
            Debug.Log("���Ž���");
        }
    }

    public void UpgradeWeapon()
    {
        if (GameManager.gameManager.SpendCurrency(upgradeCosts[GameManager.gameManager.weaponLevel])) // ���� �Ŵ��� �ڵ� �� ������ true �� �� �Ҹ� ������ false
        {
            if (GameManager.gameManager.weaponLevel >= 3)
            {
                Debug.Log("Ǯ���Դϴ�");
            }
            else
            {
                Destroy(gameObject);
                GameManager.gameManager.weaponLevel += 1;
            }
            Debug.Log("���ż���");
        }
        else
        {
            // ��ǰ ���ſ� ������ ��� �޽����� �� ������ �˷��ִ� ǥ�� �����ֱ�
            Debug.Log("���Ž���");
        }
    }
}
