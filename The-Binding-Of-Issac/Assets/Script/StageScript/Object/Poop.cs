using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : Obstacle
{
    [Header("Unity SetUp")]
    [SerializeField] Sprite[] poopSprite;

    protected override void initialization()
    {
        GetComponent<SpriteRenderer>().sprite = poopSprite[spriteIndex];
        objectLayer = 7;
    }

    public override void ResetObject()
    {
        // �ʱ�ȭ
        spriteIndex = 0;
        GetComponent<SpriteRenderer>().sprite = poopSprite[0];
        gameObject.layer = objectLayer;

        // ������Ʈ ����.
        gameObject.SetActive(false);
    }

    public override void Returnobject()
    {
        GameManager.instance.roomGenerate.PoopPool.Push(gameObject);
    }

    public override void GetDamage()
    {
        if (spriteIndex > 4)
            return;

        spriteIndex++;
        ChangeObjectSPrite();
        if(spriteIndex >= 4) // ü���� ���� ���̸�
        {
            gameObject.layer = noCollisionLayer;

            DestorySound();

            DropItem();
        }
    }

    protected override void ChangeObjectSPrite()
    {
        if (spriteIndex >= 4)
            spriteIndex = 4;

        gameObject.GetComponent<SpriteRenderer>().sprite = poopSprite[spriteIndex];
    }

    protected override void DropItem()
    {
        int rd = Random.Range(0, 3);
        if (rd == 0)
        {
            rd = Random.Range(0, 4);
            ItemManager.instance.itemTable.Dropitem(transform.position, rd);
        }
    }

}
