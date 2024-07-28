using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantSpider : ItemInfo
{
    public override void Start()
    {
        base.Start();
        SetItemCode(2);
        SetItemString("�������� �Ź�",
                      "�� ���� ��",
                      "���� �� �̸��� ���� �� �� �� �����." 
                    + "\n ������ 4������ ������.");
    }

    public override void UseItem()
    {
        PlayerManager.instance.playerShotDelay /= 0.65f;
        base.UseItem();
        PlayerManager.instance.SetHeadSkin(2);
    }
}