using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroom : ItemInfo
{
    public GameObject playerObj;
    public override void Start()
    {
        base.Start();
        SetItemCode(3);
        SetItemString("��� ����",
                      "��� ����!",
                      "���� �� �ִ� ü�� + 1ĭ" 
                    + "\nü���� ��� ȸ��" 
                    + "\n�̵��ӵ� + 0.3" 
                    + "\n���ݷ� + 0.3"
                    + "\n���ݷ� * 1.5��" 
                    + "\n��Ÿ� + 2.5" 
                    + "\n���� ũ�� * 1.1��" 
                    + "\n�÷��̾� ũ�� * 1.1��");
    }

    public override void UseItem()
    {
        playerObj = GameManager.instance.playerObject;

        PlayerManager.instance.playerMaxHp += 2;
        PlayerManager.instance.playerHp = PlayerManager.instance.playerMaxHp;
        UIManager.instance.AddHeart();
        UIManager.instance.SetPlayerCurrentHP();

        PlayerManager.instance.playerDamage += 0.3f;
        PlayerManager.instance.playerDamage *= 1.5f;
        PlayerManager.instance.playerMoveSpeed += 0.3f;
        PlayerManager.instance.playerRange += 2.5f;
        PlayerManager.instance.playerTearSize *= 1.15f;
        PlayerManager.instance.playerSize *= 1.2f;
        base.UseItem();
        PlayerManager.instance.ChgTearSize();
        PlayerManager.instance.ChgPlayerSize();
    }
}
