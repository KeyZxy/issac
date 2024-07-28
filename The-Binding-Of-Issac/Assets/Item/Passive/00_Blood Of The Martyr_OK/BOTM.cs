using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOTM : ItemInfo
{
    [Header("Sprite")]
    public Sprite redTearImg;
    public Sprite MartyrImg;


    public override void Start()
    {
        base.Start();
        SetItemCode(0);
        SetItemString("�����",
                      "�������� ��",
                      "���� �� ���ݷ� + 1" +
                      "\n���� ������ ���������� �ٲ۴�.");
    }
    public override void UseItem()
    {
        PlayerManager.instance.playerDamage++;
        base.UseItem();
        //Dr.Fetus ���� ������ ���� ���������� ����
        if (!ItemManager.instance.PassiveItems[16])
        {
            PlayerManager.instance.SetTearSkin(1);
        }
        Invoke("getBOTM", 1f);
    }

    public void getBOTM()
    {
        // ĳ���� �� ����� ����
        GameManager.instance.playerObject.GetComponent<PlayerController>().HeadItem.GetComponent<SpriteRenderer>().sprite = MartyrImg;
    }
}
