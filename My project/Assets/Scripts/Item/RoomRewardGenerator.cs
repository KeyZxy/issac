using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRewardGenerator : MonoBehaviour
{
    public List<GameObject> rewards; // ������Ʒ�б��������䡢��ҡ�Կ�ס����ġ�ը��
    public float dropRadius = 2.0f; // ����뾶
    private List<GameObject> enemiesInRoom; // �����ڵĹ����б�

    void Start()
    {
        enemiesInRoom = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    void Update()
    {
        CheckEnemies();
    }

    void CheckEnemies()
    {
        // ������������й��ﶼ������
        if (enemiesInRoom.Count == 0)
        {
            // ������ɽ�����Ʒ
            GenerateRewards();
            // ���ýű��Է�ֹ�ظ�����
            this.enabled = false;
        }
        else
        {
            // �������Ƿ����𣬲����б����Ƴ�������Ĺ���
            for (int i = enemiesInRoom.Count - 1; i >= 0; i--)
            {
                if (enemiesInRoom[i] == null)
                {
                    enemiesInRoom.RemoveAt(i);
                }
            }
        }
    }

    public void GenerateRewards()
    {
        int rewardCount = Random.Range(2, 5); // �������2��4��������Ʒ

        for (int i = 0; i < rewardCount; i++)
        {
            GameObject rewardPrefab = rewards[Random.Range(0, rewards.Count)];
            Vector2 dropPosition = (Vector2)transform.position + Random.insideUnitCircle * dropRadius;
            Instantiate(rewardPrefab, dropPosition, Quaternion.identity);
        }

        Debug.Log("���ɽ�����Ʒ�ɹ�");
    }
}
