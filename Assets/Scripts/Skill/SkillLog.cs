using System.Collections;
using UnityEngine;

public class SkillLog : MonoBehaviour
{
    [SerializeField]
    private GameObject logPrefab;
    public bool isCooldown = false;         // ��Ÿ�� Ȱ��ȭ / ��Ȱ��ȭ üũ
    private GameObject log;

    private Transform playerTransform;      // �÷��̾� ��ġ

    private void Awake()
    {
        playerTransform = gameObject.transform.root;            // �÷��̾� ��ġ ����
    }

    public IEnumerator Log()
    {
        isCooldown = true;
        Vector3 playerPosition = playerTransform.transform.position;        // �÷��̾� ��ġ
        Vector3 playerForward = playerTransform.transform.forward;          // �÷��̾� ��
        Quaternion rotation = Quaternion.Euler(0f, playerTransform.rotation.eulerAngles.y + 90f, 0f);

        log = Instantiate(logPrefab, playerPosition + playerForward, rotation);
        Rigidbody rigidBody = log.GetComponent<Rigidbody>();

        int level = GameManager.gameManager.GetCurrentWave();
        SkillState skillState = GameManager.gameManager.logState[level];

        log.transform.localScale *= skillState.objSize;

        if (rigidBody != null)
        {
            rigidBody.AddForce(playerForward * skillState.force, ForceMode.Impulse);   // �÷��̾� ���ʿ��� ���ư�
        }

        StartCoroutine(DestroyPrefab());

        yield return new WaitForSeconds(skillState.cooldown);

        isCooldown = false;
    }

    private IEnumerator DestroyPrefab()
    {
        yield return new WaitForSeconds(3f);

        Destroy(log);                        // ����
    }
}
