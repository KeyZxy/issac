using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MapObject
{
    protected int spriteIndex;
    protected int objectLayer;
    protected abstract override void initialization();                                      // ������Ʈ ���� ������ �ʱ�ȭ
    public abstract override void ResetObject();                                            // ������Ʈ �ʱ�ȭ ( Ǯ�� )
    public abstract override void Returnobject();                                           // ������Ʈ �ʱ�ȭ ( Ǯ�� )

    public virtual void GetDamage() { }                                                     // ������Ʈ ������
    protected virtual void ChangeObjectSPrite() { }                                         // ������Ʈ�� ��ȭ ( �������� �޾����� / �ı��Ǿ����� ��������Ʈ �� ������Ʈ ���� )
    protected virtual void DestorySound() { gameObject.GetComponent<AudioSource>().Play(); } // ������Ʈ �ı��� ����        
    protected virtual void DropItem() { }                                                   // ������Ʈ �ı��� ������ ���
}   
