using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public int price; // 상품 가격
    public GameObject ShopUi;
    public GameObject grab;
    [SerializeField] private int[] upgradeCosts = new int[] { 0, 100, 200 }; // 각 무기 레벨별 업그레이드 비용 배열

    public static void BuyItem(int price)
    {
        if (GameManager.gameManager.SpendCurrency(price)) // 게임 매니저 코드 돈 있으면 true 및 돈 소모 없으면 false
        {
            // 상품 구매에 성공했을 때 기능 작성하기
            Debug.Log("구매성공");
        }
        else
        {
            // 상품 구매에 실패한 경우 메시지나 돈 없음을 알려주는 표시 보여주기
            Debug.Log("구매실패");
        }
    }

    public void UpgradeWeapon()
    {
        if (GameManager.gameManager.SpendCurrency(upgradeCosts[GameManager.gameManager.weaponLevel])) // 게임 매니저 코드 돈 있으면 true 및 돈 소모 없으면 false
        {
            if (GameManager.gameManager.weaponLevel >= 3)
            {
                Debug.Log("풀강입니다");
            }
            else
            {
                Destroy(gameObject);
                GameManager.gameManager.weaponLevel += 1;
            }
            Debug.Log("구매성공");
        }
        else
        {
            // 상품 구매에 실패한 경우 메시지나 돈 없음을 알려주는 표시 보여주기
            Debug.Log("구매실패");
        }
    }
}
