using System.Collections;
using UnityEngine;

public class SkillLog : MonoBehaviour
{
    [SerializeField]
    private GameObject logPrefab;
    public bool isCooldown = false;         // 쿨타임 활성화 / 비활성화 체크
    private GameObject log;

    private Transform playerTransform;      // 플레이어 위치

    private void Awake()
    {
        playerTransform = gameObject.transform.root;            // 플레이어 위치 지정
    }

    public IEnumerator Log()
    {
        isCooldown = true;
        Vector3 playerPosition = playerTransform.transform.position;        // 플레이어 위치
        Vector3 playerForward = playerTransform.transform.forward;          // 플레이어 앞
        Quaternion rotation = Quaternion.Euler(0f, playerTransform.rotation.eulerAngles.y + 90f, 0f);

        log = Instantiate(logPrefab, playerPosition + playerForward, rotation);
        Rigidbody rigidBody = log.GetComponent<Rigidbody>();

        int level = GameManager.gameManager.GetCurrentWave();
        SkillState skillState = GameManager.gameManager.logState[level];

        log.transform.localScale *= skillState.objSize;

        if (rigidBody != null)
        {
            rigidBody.AddForce(playerForward * skillState.force, ForceMode.Impulse);   // 플레이어 앞쪽에서 날아감
        }

        StartCoroutine(DestroyPrefab());

        yield return new WaitForSeconds(skillState.cooldown);

        isCooldown = false;
    }

    private IEnumerator DestroyPrefab()
    {
        yield return new WaitForSeconds(3f);

        Destroy(log);                        // 삭제
    }
}
