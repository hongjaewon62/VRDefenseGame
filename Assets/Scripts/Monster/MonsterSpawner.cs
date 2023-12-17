using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject spawner;          // 몬스터 스폰 위치
    private int spawnersLength;         // 스폰 장소 개수
    private Wave currentWave;           // 현재 웨이브 단계 정보
    private int spawnMonsterCount = 0; // 현재 웨이브 생성 몬스터 숫자

    private void Start()
    {
        spawnersLength = spawner.transform.childCount;
    }

    public void StartWave(Wave wave)
    {currentWave = wave; // 웨이브 정보 저장
        StartCoroutine("SpawnMonster"); // 웨이브 시작
    }

    private IEnumerator SpawnMonster()
    {
        
        while( spawnMonsterCount < currentWave.maxMonsterCount)
        {
            int monsterIndex = Random.Range(0, currentWave.monsterPrefabs.Length);
            int randomPosition = Random.Range(0, spawnersLength);

            // 몬스터 생성
            GameObject newMonster = Instantiate(currentWave.monsterPrefabs[monsterIndex], spawner.transform.GetChild(randomPosition).transform.position, spawner.transform.GetChild(randomPosition).transform.rotation);
            Monster monsterController = newMonster.GetComponent<Monster>();
            if (monsterController != null)
            {
                monsterController.OnMonsterDeath += HandleMonsterDeath;
            }

            spawnMonsterCount++;
            GameManager.gameManager.AddMonster();
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
        GameManager.gameManager.FinishSpawn();
    }
    private void HandleMonsterDeath(GameObject monster)
    {
        // 몬스터가 사망하면 호출되는 콜백 함수
        // 이 함수에서 몬스터를 관리 목록에서 제거하고 몬스터 수를 감소시킴
        GameManager.gameManager.DieMonster();
        Destroy(monster);
    }
}

