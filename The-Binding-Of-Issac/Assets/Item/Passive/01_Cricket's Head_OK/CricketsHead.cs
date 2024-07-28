using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CricketsHead : ItemInfo
{
    public override void Start()
    {
        base.Start();
        SetItemCode(1);
        SetItemString("ũ������ �Ӹ�",
                      "���ݷ� ����",
                      "���� �� ���� �� Ŀ����." 
                    + "\n���ݷ� + 0.5" 
                    + "\n���ݷ� * 1.3" 
                    + "\n���� ũ�� * 1.1��");
    }
    public override void UseItem()
    {
        PlayerManager.instance.playerDamage += 0.5f;
        PlayerManager.instance.playerDamage *= 1.3f;
        PlayerManager.instance.playerTearSize *= 1.1f;
        base.UseItem();
        PlayerManager.instance.ChgTearSize();
        PlayerManager.instance.SetHeadSkin(3);
    }
}
