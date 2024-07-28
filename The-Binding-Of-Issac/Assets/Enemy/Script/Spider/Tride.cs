using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Playables;



public class Tride : TEnemy
{
    // prowl
    public override void En_setState()
    {
        playerInRoom    = false;
        dieParameter    = "isDie";
        jumpParameter   = "isJump";

        hp              = 3f;
        sight           = 5f;
        moveSpeed       = 1f;
        waitforSecond   = 0.5f;
        attaackSpeed    = 0.5f;
        jumpSpeed       = 2.5f;

        maxhp           = hp;

        enemyNumber = 6;
    }

    public override void En_kindOfEnemy()
    {
        isTracking      = true;
        isProwl         = false;
        isDetective     = false;
        isShoot         = false;
        isJump          = true;
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


    public void LandingSound()
    {
        audioSource.Play();
    }

}
