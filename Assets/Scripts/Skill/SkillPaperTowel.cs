using System.Collections;
using UnityEngine;

public class SkillPaperTowel : MonoBehaviour
{
    //[SerializeField]
    //private int damage = 1;

    [SerializeField]
    private GameObject paperToewlPrefab;
    public bool isCooldown = false;         // ��Ÿ�� Ȱ��ȭ / ��Ȱ��ȭ üũ
    private GameObject paperTowel;

    private Transform playerTransform;      // �÷��̾� ��ġ

    private void Awake()
    {
        playerTransform = gameObject.transform.root;            // �÷��̾� ��ġ ����
    }

    public IEnumerator PaperTowel()
    {
        isCooldown = true;
        Vector3 playerPosition = playerTransform.transform.position;        // �÷��̾� ��ġ
        Vector3 playerForward = playerTransform.transform.forward;          // �÷��̾� ��
        paperTowel = Instantiate(paperToewlPrefab, playerPosition + playerForward + Vector3.up, playerTransform.rotation);
        Rigidbody rigidBody = paperTowel.GetComponent<Rigidbody>();

        SkillState paparState = GameManager.gameManager.paperState[GameManager.gameManager.GetCurrentWave()];
        paperTowel.transform.localScale *= paparState.objSize;

        if (rigidBody != null)
        {
            rigidBody.AddForce(playerForward * paparState.force, ForceMode.Impulse);           // �÷��̾� ���ʿ��� ���ư�
        }

        StartCoroutine(DestroyPrefab());

        yield return new WaitForSeconds(paparState.cooldown);

        isCooldown = false;
    }

    private IEnumerator DestroyPrefab()
    {
        yield return new WaitForSeconds(3f);

        Destroy(paperTowel);                        // ����
    }
}
