using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeremonialRobes : ItemInfo
{

    public override void Start()
    {
        base.Start();
        SetItemCode(5);
        SetItemString("�ǽĿ� ����",
                      "��ο� �ΰ�",
                      "���� �� �ִ� ü�� + 2ĭ"
                    + "\n���ݷ� + 1");
    }

    public override void UseItem()
    {
        PlayerManager.instance.playerMaxHp += 4;
        UIManager.instance.AddHeart();
        PlayerManager.instance.playerDamage += 1;
        base.UseItem();
        //ĳ���� ���� ����
        PlayerManager.instance.SetBodySkin(3);
        PlayerManager.instance.SetHeadSkin(5);
    }
}
