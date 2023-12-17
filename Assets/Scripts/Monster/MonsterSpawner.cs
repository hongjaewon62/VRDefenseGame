using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject spawner;          // ���� ���� ��ġ
    private int spawnersLength;         // ���� ��� ����
    private Wave currentWave;           // ���� ���̺� �ܰ� ����
    private int spawnMonsterCount = 0; // ���� ���̺� ���� ���� ����

    private void Start()
    {
        spawnersLength = spawner.transform.childCount;
    }

    public void StartWave(Wave wave)
    {currentWave = wave; // ���̺� ���� ����
        StartCoroutine("SpawnMonster"); // ���̺� ����
    }

    private IEnumerator SpawnMonster()
    {
        
        while( spawnMonsterCount < currentWave.maxMonsterCount)
        {
            int monsterIndex = Random.Range(0, currentWave.monsterPrefabs.Length);
            int randomPosition = Random.Range(0, spawnersLength);

            // ���� ����
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
        // ���Ͱ� ����ϸ� ȣ��Ǵ� �ݹ� �Լ�
        // �� �Լ����� ���͸� ���� ��Ͽ��� �����ϰ� ���� ���� ���ҽ�Ŵ
        GameManager.gameManager.DieMonster();
        Destroy(monster);
    }
}

