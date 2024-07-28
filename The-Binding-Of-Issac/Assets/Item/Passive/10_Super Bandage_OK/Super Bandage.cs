using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBandage : ItemInfo
{

    public override void Start()
    {
        base.Start();
        SetItemCode(10);
        SetItemString("���� ��â��",
                      "�׸� ����! �ʹ� ����!",
                      "���� �� �ִ� ü�� - 1ĭ"
                    + "\nü�� - 1ĭ"
                    + "\n���ݼӵ� + 0.1");
    }
    public override void UseItem()
    {
        PlayerManager.instance.playerMaxHp-=2;
        PlayerManager.instance.playerHp-=2;
        PlayerManager.instance.playerShotDelay-=0.1f;
        base.UseItem();
        UIManager.instance.DelHeart();
        UIManager.instance.SetPlayerCurrentHP();

        if (!ItemManager.instance.PassiveItems[16])
        {
            PlayerManager.instance.SetTearSkin(1);
        }
    }
}
