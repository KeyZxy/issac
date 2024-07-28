using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{
    [Header("Unity Setup")]
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] bossPrefabs;

    // �����ϰ� ���� ��ȯ
    public GameObject GetEnemy()
    {
        int rd = Random.Range(0, enemyPrefabs.Length);

        GameObject enemy;
        enemy = Instantiate(enemyPrefabs[rd]) as GameObject;
        return enemy;
    }

    public GameObject GetEnemy(int index)
    {
        GameObject enemy;
        enemy = Instantiate(enemyPrefabs[index]) as GameObject;
        return enemy;
    }


    public GameObject GetBoss()
    {
        // ���������� ������ ������ ����
        GameObject boss = Instantiate(bossPrefabs[GameManager.instance.stageLevel-1]) as GameObject;  
        return boss;
    }
}