using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperTowelCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Monster monster = collision.gameObject.GetComponent<Monster>();

        if (monster != null)
        {
            // 몬스터에 데미지를 입힘
            monster.HitByPaperTowel();
            monster.Invoke("Die", 3.0f);
            Destroy(gameObject);
        }
    }
}
