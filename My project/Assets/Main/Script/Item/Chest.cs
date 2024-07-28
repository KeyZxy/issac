using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<GameObject> rewards; // ������Ʒ�б�������ҡ�Կ�ס����ġ�ը��
    private Player player; // ��Ҷ���
    public Sprite shapeAfterOpen; // �򿪺�ı������ξ���
    private bool isOpened = false; // �����Ƿ��Ѿ�����
    protected int mixNum; // ��С������Ʒ����
    protected int maxNum; // ���������Ʒ����

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); // ���Ҵ��б�ǩ "Player" �Ķ��󲢻�ȡ Player ���
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            if (player.KeyNum > 0) // �������Ƿ���Կ��
            {
                OpenChest();
            }
            else
            {
                Debug.Log("��ҪԿ�ײ��ܴ򿪱��䣡");
            }
        }
    }

    void OpenChest()
    {
        isOpened = true;
        player.KeyNum--; // ����һ��Կ��
        GenerateReward();
        GetComponent<SpriteRenderer>().sprite = shapeAfterOpen; // ���ľ�������
    }

    void GenerateReward()
    {
        int num = UnityEngine.Random.Range(mixNum, maxNum + 1); // ���������Ʒ����
        for (int i = 0; i < num; i++)
        {
            // ���ѡ��һ��������Ʒ
            GameObject rewardPrefab = rewards[Random.Range(0, rewards.Count)];
            // �ڱ���λ�����ɽ�����Ʒ
            Instantiate(rewardPrefab, transform.position, Quaternion.identity);
            Vector2 force = UnityEngine.Random.insideUnitCircle * 7; // �����������
            if (rewardPrefab.GetComponent<Rigidbody2D>())
            {
                rewardPrefab.GetComponent<Rigidbody2D>().AddForce(force); // ����Ʒ�����
            }
            else if (rewardPrefab.GetComponent<IRandomGameObject>() != null)
            {
                rewardPrefab.GetComponent<IRandomGameObject>().Generate().GetComponent<Rigidbody2D>().AddForce(force); // �����Ʒʵ���� IRandomGameObject �ӿڣ����ɲ������
            }
            Debug.Log("���ɽ�����Ʒ�ɹ�");
        }
    }
}
