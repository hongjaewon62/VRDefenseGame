using UnityEngine;
using UnityEngine.UI;

public class GoalSpot : MonoBehaviour
{
    public float amplitude = 1.0f;      // 곡선의 진폭 (높이)
    public float frequency = 1.0f;      // 곡선의 주파수 (반복 횟수)
    public float speed = 1.0f;          // 이동 속도

    private Vector3 initialPosition;    // 처음 위치
    private Image image;                // 투명하게 바꾸기 위한 변수

    void Start()
    {
        image = GetComponent<Image>();
        Debug.Assert(image != null, "Error (Null Reference) : 애니메이션 컴포넌트가 존재하지 않습니다.");

        // 현재 위치를 저장함
        initialPosition = transform.position;
    }

    void Update()
    {
        // 현재 시간에 따라 y축 위치를 조절
        float newY = initialPosition.y + Mathf.Sin(Time.time * speed * frequency) * amplitude;

        // 새로운 위치를 설정
        Vector3 newPosition = transform.position;
        newPosition.y = newY;

        transform.position = newPosition;
    }
}
