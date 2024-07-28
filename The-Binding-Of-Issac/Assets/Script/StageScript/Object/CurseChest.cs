using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class CurseChest : Chest
{
    [Header("Chest State")]
    Room roomInfo;

    protected override void initialization()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = closeSprite;
    }

    public override void Returnobject()
    {
        GameManager.instance.roomGenerate.CurseChestPool.Push(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OpenChest();
        }
    }


    protected override void DropReward()
    {
        int rd = Random.Range(0, 10000);
        if(rd % 2 == 0)
        {
            GameObject it = Instantiate(ItemManager.instance.itemTable.DropPassive(), transform.position, Quaternion.identity) as GameObject;
            GameManager.instance.roomGenerate.itemList.Add(it);
            return;
        }

        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        // �ĸ� �Ǵ� �Ź��� ������ ���� 1�� ����
        int rd = Random.Range(0, 1000);
        int randomEnemyIndex;
        if (rd % 2 == 0)
            randomEnemyIndex = 0;
        else
            randomEnemyIndex = 5;

        // �ĸ� �Ǵ� �Ź̸� ���� ����.
        GameObject enemy = GameManager.instance.roomGenerate.enemyGenerate.GetEnemy(randomEnemyIndex);
        enemy.transform.SetParent(roomInfo.transform);
        enemy.transform.position = gameObject.transform.position;
        enemy.GetComponent<TEnemy>().roomInfo = roomInfo.gameObject;

        // �ش� ���� ���͸���Ʈ�� �߰�
        roomInfo.GetComponent<Room>().enemis.Add(enemy);

        // ���� �������� �ش�濡 ���Ͱ� �����ϱ⶧����
        // �ش� ���� Ŭ���� ���θ� false�� ����
        roomInfo.isClear = false; 

        // sfx ���� ������ ���� ������Ʈ ����
        GameManager.instance.roomGenerate.SetSFXDestoryObject(enemy);
    }

    public void SetRoomInfo(GameObject room)
    {
        roomInfo = room.GetComponent<Room>();
    }
}
