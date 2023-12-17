using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public int damageAmount; // 골에 닿았을 때 입힐 데미지량
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            ApplyDamageToGoal(monster);
            monster.Die();
        }
    }

    private void ApplyDamageToGoal(Monster monster) // 골에 닿은 몬스터의 데미지 정보를 가져와서 골의 체력에 데미지를 적용
    {
        damageAmount = monster.monsterData.damage; // 몬스터의 데미지 정보 가져오기
        GameManager.gameManager.DecreaseHealth(damageAmount); // 체력 감소
        UiManager.uiManager.UpdateHealthText(GameManager.gameManager.currentHealth, GameManager.gameManager.maxHealth);
    }
}
