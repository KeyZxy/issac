using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianAngel : ItemInfo
{
    public override void Start()
    {
        base.Start();
        SetItemCode(12);
        SetItemString("��ȣ õ��",
                      "��ȣ",
                      "���� �� ĳ���� �ֺ��� õ�� ����"
                    + "\nHoly Mantle ���� ��"
                    + "\n�� Ŭ���� �� 1ȸ ��ȣ���� �ο��Ѵ�.");
    }

    public override void UseItem()
    {
        Invoke("GenerateAngel", 1.0f);
    }
    
    private void GenerateAngel()
    {
        GameManager.instance.playerObject.GetComponent<PlayerController>().familiarPosition.gameObject.SetActive(true);
    }
}
