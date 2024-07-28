using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapObject : MonoBehaviour
{
    protected int noCollisionLayer = 31;

    private void Start()
    {
        initialization();
    }

    protected abstract void initialization();          // ������Ʈ ���� ������ �ʱ�ȭ
    public abstract void ResetObject();                // ������Ʈ �ʱ�ȭ ( Ǯ�� 
    public abstract void Returnobject();               // ������Ʈ �ʱ�ȭ ( Ǯ��
}
