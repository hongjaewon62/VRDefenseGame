using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float trapDuration = 2f;  // 함정의 동작 시간

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster") // 닿은 오브젝트가 몬스터일 경우에만 실행
        {
            Monster monster = other.GetComponent<Monster>();

            // 함정에 걸린 몬스터에 대한 처리 추가적으로 하수구 같은 걸 구현한다면 트랩 중앙 위치로 이동시키는 코드 넣기
            monster.SetTrapped(trapDuration);
            Debug.Log("catch");
        }
    }
}

