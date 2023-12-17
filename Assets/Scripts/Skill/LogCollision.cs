using UnityEngine;

public class LogCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Monster monster = collision.gameObject.GetComponent<Monster>();
        SkillState logState = GameManager.gameManager.logState[GameManager.gameManager.GetCurrentWave()];

        if (monster != null)
        {
            // ���Ϳ� �������� ����
            monster.TakeDamage(logState.damage, gameObject.transform.position, logState.knockback, true);
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.collider);           // �� ���� �浹�ϵ��� ����
        }
    }
}
