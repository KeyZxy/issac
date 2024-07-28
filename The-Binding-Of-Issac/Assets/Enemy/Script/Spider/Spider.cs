using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spider : TEnemy
{
    // prowl
    public override void En_setState()
    {
        playerInRoom     = false;
        dieParameter     = "isDie";

        hp              = 3f;
        sight           = 5f;
        moveSpeed       = 3f;
        waitforSecond   = 0.5f;
        fTime           = 0.5f;
        randRange       = 3f;

        maxhp           = hp;

        enemyNumber = 5;
    }

    public override void En_kindOfEnemy()
    {
        isTracking      = false;
        isProwl         = true;
        isDetective     = false;
        isShoot         = false;
    }

    private void Start()
    {
        // ���� ���� state ����
        En_setState();              // ���� ����
        En_kindOfEnemy();           // enemy�� �ൿ ����

        En_Start();                  // �ʱ⼼��
    }

    private void Update()
    {
        E_Excute();                 // ���� ����
    }

}
