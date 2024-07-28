using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCat : ItemInfo
{
    public override void Start()
    {
        base.Start();
        SetItemCode(14);
        SetItemString("���� �����",
                      "���ǰ� �׾���...",
                      "���� �� �ִ� ü���� 1ĭ���� ����"
                    + "\n��� �� 50% Ȯ���� �ִ�ü�� ȸ�� / ���");
    }

    public override void UseItem()
    {
        PlayerManager.instance.playerMaxHp = 2;
        PlayerManager.instance.playerHp = PlayerManager.instance.playerMaxHp;
        UIManager.instance.DelHeart();
    }
}
