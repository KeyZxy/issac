using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFly : TEnemy
{
    // tracking
    public override void En_setState()
    {
        playerInRoom    = false;
        dieParameter    = "isDie";

        hp              = 2f;
        sight           = 5f;
        moveSpeed       = 1.5f;
        waitforSecond   = 0.5f;

        maxhp           = hp;
        enemyNumber = 0;
    }

    public override void En_kindOfEnemy()
    {
        isTracking      = true;
        isProwl         = false;
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