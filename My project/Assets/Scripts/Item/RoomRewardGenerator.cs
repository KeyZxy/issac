using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRewardGenerator : MonoBehaviour
{
    public List<GameObject> rewards; // 奖励物品列表，包含宝箱、金币、钥匙、红心、炸弹
    public float dropRadius = 2.0f; // 掉落半径
    private List<GameObject> enemiesInRoom; // 房间内的怪物列表

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
        // 如果房间内所有怪物都被消灭
        if (enemiesInRoom.Count == 0)
        {
            // 随机生成奖励物品
            GenerateRewards();
            // 禁用脚本以防止重复生成
            this.enabled = false;
        }
        else
        {
            // 检查怪物是否被消灭，并从列表中移除被消灭的怪物
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
        int rewardCount = Random.Range(2, 5); // 随机生成2到4个奖励物品

        for (int i = 0; i < rewardCount; i++)
        {
            GameObject rewardPrefab = rewards[Random.Range(0, rewards.Count)];
            Vector2 dropPosition = (Vector2)transform.position + Random.insideUnitCircle * dropRadius;
            Instantiate(rewardPrefab, dropPosition, Quaternion.identity);
        }

        Debug.Log("生成奖励物品成功");
    }
}
