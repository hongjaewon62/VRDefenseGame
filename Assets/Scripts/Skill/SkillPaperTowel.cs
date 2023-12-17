using System.Collections;
using UnityEngine;

public class SkillPaperTowel : MonoBehaviour
{
    //[SerializeField]
    //private int damage = 1;

    [SerializeField]
    private GameObject paperToewlPrefab;
    public bool isCooldown = false;         // 쿨타임 활성화 / 비활성화 체크
    private GameObject paperTowel;

    private Transform playerTransform;      // 플레이어 위치

    private void Awake()
    {
        playerTransform = gameObject.transform.root;            // 플레이어 위치 지정
    }

    public IEnumerator PaperTowel()
    {
        isCooldown = true;
        Vector3 playerPosition = playerTransform.transform.position;        // 플레이어 위치
        Vector3 playerForward = playerTransform.transform.forward;          // 플레이어 앞
        paperTowel = Instantiate(paperToewlPrefab, playerPosition + playerForward + Vector3.up, playerTransform.rotation);
        Rigidbody rigidBody = paperTowel.GetComponent<Rigidbody>();

        SkillState paparState = GameManager.gameManager.paperState[GameManager.gameManager.GetCurrentWave()];
        paperTowel.transform.localScale *= paparState.objSize;

        if (rigidBody != null)
        {
            rigidBody.AddForce(playerForward * paparState.force, ForceMode.Impulse);           // 플레이어 앞쪽에서 날아감
        }

        StartCoroutine(DestroyPrefab());

        yield return new WaitForSeconds(paparState.cooldown);

        isCooldown = false;
    }

    private IEnumerator DestroyPrefab()
    {
        yield return new WaitForSeconds(3f);

        Destroy(paperTowel);                        // 삭제
    }
}
