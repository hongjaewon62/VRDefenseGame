using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private MonsterSpawner monsterSpawner;
    [SerializeField]

    public bool isStarted = false;
    public void StartWave()
    {
        if (isStarted == false)
        {
            int currentWaveIndex = GameManager.gameManager.GetCurrentWave() - 1; // ���̺� �ܰ�
            monsterSpawner.StartWave(waves[currentWaveIndex]);
            isStarted = true;
        }
    }
}



[System.Serializable]
public struct Wave
{
    public float spawnTime;                    // ���̺� �� ���� �ֱ�
    public int maxMonsterCount;                // ���̺� �� ���� ����
    public GameObject[] monsterPrefabs;        // �� ���� ����
}