using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Obstacle
{
    [SerializeField] Sprite[] rockSprite;

    protected override void initialization()
    {
        GetComponent<SpriteRenderer>().sprite = rockSprite[spriteIndex];
        objectLayer = 8;
    }

    public override void ResetObject()
    {
        // �ʱ�ȭ
        spriteIndex = 0;
        gameObject.GetComponent<SpriteRenderer>().sprite = rockSprite[0];
        gameObject.layer = objectLayer;

        // ������Ʈ ����.
        gameObject.SetActive(false);
    }

    public override void Returnobject()
    {
        GameManager.instance.roomGenerate.RockPool.Push(gameObject);
    }

    public override void GetDamage()
    {
        if (spriteIndex == 1)
            return;
        spriteIndex = 1;
        ChangeObjectSPrite();
        gameObject.layer = noCollisionLayer;

        DestorySound();
    }

    protected override void ChangeObjectSPrite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = rockSprite[spriteIndex];
    }
}
