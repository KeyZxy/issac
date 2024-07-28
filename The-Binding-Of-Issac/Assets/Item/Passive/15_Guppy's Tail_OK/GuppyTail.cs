using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuppyTail : ItemInfo
{
    public override void Start()
    {
        base.Start();
        SetItemCode(15);
        SetItemString("������ ����",
                      "�߸� ���� ����",
                      "���� �� �ִ� ü�� 1ĭ���� ����"
                    + "\n��� + 9");
    }
    public override void UseItem()
    {
        PlayerManager.instance.playerMaxHp = 2;
        PlayerManager.instance.playerHp = PlayerManager.instance.playerMaxHp;
        UIManager.instance.DelHeart();
        PlayerManager.instance.deathCount = 9;
        UIManager.instance.deathCount.gameObject.transform.parent.gameObject.SetActive(true);
    }
}
