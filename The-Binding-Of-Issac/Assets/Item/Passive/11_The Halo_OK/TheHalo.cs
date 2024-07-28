using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheHalo : ItemInfo
{
    public Sprite HaloImg;
    public override void Start()
    {
        base.Start();
        SetItemCode(11);
        SetItemString("����",
                      "õ��",
                      "���� �� �ִ� ü�� + 1ĭ"
                    + "\n���ݼӵ� + 0.07"
                    + "\n���ݷ� - 0.3");
    }
    public override void UseItem()
    {
        PlayerManager.instance.playerMaxHp += 2;
        UIManager.instance.AddHeart();
        PlayerManager.instance.playerShotDelay -= 0.07f;
        PlayerManager.instance.playerDamage -= 0.3f;

        base.UseItem();
        //ĳ���� �Ӹ� ���� õ�縵 ����
        GameManager.instance.playerObject.GetComponent<PlayerController>().HeadItem.GetComponent<SpriteRenderer>().sprite = HaloImg;

    }
}
