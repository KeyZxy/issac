using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<GameObject> rewards; // 奖励物品列表，包含金币、钥匙、红心、炸弹
    private Player player; // 玩家对象
    public Sprite shapeAfterOpen; // 打开后的宝箱外形精灵
    private bool isOpened = false; // 宝箱是否已经被打开
    protected int mixNum; // 最小生成物品数量
    protected int maxNum; // 最大生成物品数量

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // 查找带有标签 "Player" 的对象并获取 Player 组件
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            if (player.KeyNum > 0) // 检查玩家是否有钥匙
            {
                OpenChest();
            }
            else
            {
                Debug.Log("需要钥匙才能打开宝箱！");
            }
        }
    }

    void OpenChest()
    {
        isOpened = true;
        player.KeyNum--; // 消耗一个钥匙
        GenerateReward();
        GetComponent<SpriteRenderer>().sprite = shapeAfterOpen; // 更改精灵外形
    }

    void GenerateReward()
    {
        int num = UnityEngine.Random.Range(mixNum, maxNum + 1); // 随机生成物品数量
        for (int i = 0; i < num; i++)
        {
            // 随机选择一个奖励物品
            GameObject rewardPrefab = rewards[Random.Range(0, rewards.Count)];
            // 在宝箱位置生成奖励物品
            Instantiate(rewardPrefab, transform.position, Quaternion.identity);
            Vector2 force = UnityEngine.Random.insideUnitCircle * 7; // 生成随机的力
            if (rewardPrefab.GetComponent<Rigidbody2D>())
            {
                rewardPrefab.GetComponent<Rigidbody2D>().AddForce(force); // 给物品添加力
            }
            else if (rewardPrefab.GetComponent<IRandomGameObject>() != null)
            {
                rewardPrefab.GetComponent<IRandomGameObject>().Generate().GetComponent<Rigidbody2D>().AddForce(force); // 如果物品实现了 IRandomGameObject 接口，生成并添加力
            }
            Debug.Log("生成奖励物品成功");
        }
    }
}
