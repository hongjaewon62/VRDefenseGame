using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public int damageAmount; // �� ����� �� ���� ��������
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            ApplyDamageToGoal(monster);
            monster.Die();
        }
    }

    private void ApplyDamageToGoal(Monster monster) // �� ���� ������ ������ ������ �����ͼ� ���� ü�¿� �������� ����
    {
        damageAmount = monster.monsterData.damage; // ������ ������ ���� ��������
        GameManager.gameManager.DecreaseHealth(damageAmount); // ü�� ����
        UiManager.uiManager.UpdateHealthText(GameManager.gameManager.currentHealth, GameManager.gameManager.maxHealth);
    }
}
