using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HolyMantle : ItemInfo
{
    public override void Start()
    {
        base.Start();
        SetItemCode(6);
        SetItemString("�ż��� ����",
                      "�������� ����",
                      "3ȸ ���� �ο��Ѵ�." 
                    + "\nGuardian Angel ���� ��" 
                    + "\n�� Ŭ���� �� 1ȸ ���� �ο��Ѵ�.");
    }
    public override void UseItem()
    {
        PlayerManager.instance.CanBlockDamage += 3;
        UIManager.instance.guardCount.transform.parent.gameObject.SetActive(true);
    }
}
