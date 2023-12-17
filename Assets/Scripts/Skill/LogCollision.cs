using UnityEngine;

public class LogCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Monster monster = collision.gameObject.GetComponent<Monster>();
        SkillState logState = GameManager.gameManager.logState[GameManager.gameManager.GetCurrentWave()];

        if (monster != null)
        {
            // 몬스터에 데미지를 입힘
            monster.TakeDamage(logState.damage, gameObject.transform.position, logState.knockback, true);
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.collider);           // 한 번만 충돌하도록 설정
        }
    }
}
