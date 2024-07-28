using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWafer : ItemInfo
{
    public override void Start()
    {
        base.Start();
        SetItemCode(8);
        SetItemString("�л�",
                      "ź��ȭ�� ���",
                      "���� �� �̵��ӵ� - 1.3"
                    + "\n���ݷ� + 2"
                    + "\n��Ÿ� - 2"
                    + "\n���� �ӵ� - 1");
    }
    public override void UseItem()
    {
        Transform parentSize = GameManager.instance.playerObject.transform;
        Transform bodySize = GameManager.instance.playerObject.transform.GetChild(1).GetComponent<Transform>();
        PlayerManager.instance.playerMoveSpeed -= 1.3f;
        PlayerManager.instance.playerDamage += 2f;
        PlayerManager.instance.playerRange -= 2f;
        PlayerManager.instance.playerTearSpeed -= 1f;
        base.UseItem();
        bodySize.localScale = new Vector3(parentSize.localScale.x * 1.2f, parentSize.localScale.y, 0);
    }
}
