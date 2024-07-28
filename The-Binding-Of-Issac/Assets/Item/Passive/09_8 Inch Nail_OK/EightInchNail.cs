using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightInchNail : ItemInfo
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetItemCode(9);
        SetItemString("8��ġ ���",
                      "�ڻ쳻 ����!",
                      "ĳ���Ͱ� ���� ��� ���� �߻��Ѵ�." +
                      "\n ���ݷ� + 1.5");
    }

    public override void UseItem()
    {
        base.UseItem();
        PlayerManager.instance.playerDamage += 1.5f;
        if (!ItemManager.instance.PassiveItems[16])
        {
            PlayerManager.instance.SetTearSkin(2);
        }
    }
}
