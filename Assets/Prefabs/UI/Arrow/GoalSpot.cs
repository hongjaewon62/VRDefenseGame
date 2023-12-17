using UnityEngine;
using UnityEngine.UI;

public class GoalSpot : MonoBehaviour
{
    public float amplitude = 1.0f;      // ��� ���� (����)
    public float frequency = 1.0f;      // ��� ���ļ� (�ݺ� Ƚ��)
    public float speed = 1.0f;          // �̵� �ӵ�

    private Vector3 initialPosition;    // ó�� ��ġ
    private Image image;                // �����ϰ� �ٲٱ� ���� ����

    void Start()
    {
        image = GetComponent<Image>();
        Debug.Assert(image != null, "Error (Null Reference) : �ִϸ��̼� ������Ʈ�� �������� �ʽ��ϴ�.");

        // ���� ��ġ�� ������
        initialPosition = transform.position;
    }

    void Update()
    {
        // ���� �ð��� ���� y�� ��ġ�� ����
        float newY = initialPosition.y + Mathf.Sin(Time.time * speed * frequency) * amplitude;

        // ���ο� ��ġ�� ����
        Vector3 newPosition = transform.position;
        newPosition.y = newY;

        transform.position = newPosition;
    }
}
