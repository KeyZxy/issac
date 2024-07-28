using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoyMilk : ItemInfo
{
    public override void Start()
    {
        base.Start();
        SetItemCode(4);
        SetItemString("����",
                      "�� ������..",
                      "���� �� ���ݷ� * 0.5" 
                    + "\n���ݼӵ� * 2.5");
    }

    public override void UseItem()
    {
        PlayerManager.instance.playerDamage *= 0.5f;
        PlayerManager.instance.playerShotDelay /= 2.5f;
        PlayerManager.instance.playerTearSize *= 0.45f;
        PlayerManager.instance.ChgTearSize();
        base.UseItem();
    }
}
