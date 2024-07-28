using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chest : MapObject
{
    [SerializeField] protected Sprite openSprite;
    [SerializeField] protected Sprite closeSprite;

    protected abstract override void initialization();                                      // ������Ʈ ���� ������ �ʱ�ȭ
    public override void ResetObject()                                                      // ������Ʈ �ʱ�ȭ ( Ǯ�� )
    {
        //�ʱ�ȭ
        gameObject.layer = 15;
        gameObject.GetComponent<SpriteRenderer>().sprite = closeSprite;

        // ������Ʈ ����.
        gameObject.SetActive(false);
    }
    public abstract override void Returnobject();                                           // ������Ʈ �ʱ�ȭ ( Ǯ�� )

    protected virtual void OpenChest()                                                      // ���� ����
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = openSprite;                      // �������� �̹����� ����
        DropReward();
        OpenSound(); // ���� 

        gameObject.layer = 16; // �������� ��������Ʈ
    }
    protected virtual void OpenSound()                                                      // ���� ���� ����
    {   
        gameObject.GetComponent<AudioSource>().Play();
    }                                                  

    protected virtual void DropReward() { }                                                 // ���� ����
}
