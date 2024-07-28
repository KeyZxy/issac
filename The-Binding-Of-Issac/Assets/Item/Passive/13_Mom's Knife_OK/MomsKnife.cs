using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomsKnife : ItemInfo
{
    public Sprite knifeImg;
    public override void Start()
    {
        base.Start();
        SetItemCode(13);
        SetItemString("������ ��Į",
                      "���� ������ �ʾҽ��ϴ�.",
                      "���� �� ������ ������� ��Į�� ������."
                    + "\n���ݷ� + 2.5"
                    + "\nDr.Fetus ���� �� ��Į�� ������ �ʴ´�.");
    }
    public override void UseItem()
    {
        if (!ItemManager.instance.PassiveItems[16])
        {
            PlayerManager.instance.playerDamage += 2.5f;
            base.UseItem();
            Invoke("GenerateKnife", 1.0f);
        }
    }

    private void GenerateKnife()
    {
        GameManager.instance.playerObject.GetComponent<PlayerController>().knifePosition.gameObject.SetActive(true);
        GameManager.instance.playerObject.GetComponent<PlayerController>().knife.SetActive(true);
    }
}
