using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moter : TEnemy
{
    // ���� �� �ĸ� �θ��� ����

    [SerializeField] GameObject attackFly;

    public override void En_setState()
    {
        playerInRoom     = false;
        dieParameter     = "isDie";

        hp               = 3f;
        sight            = 5f;
        moveSpeed        = 1.5f;
        waitforSecond    = 0.5f;

        maxhp            = hp;

        enemyNumber = 1;
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

    private void OnDestroy()        // ���� �ĸ� �θ��� ����
    {
        if (hp <= 0.1f) 
        {
            GenerateAttackFly();
            GenerateAttackFly();
        }
    }

    void GenerateAttackFly()
    {
        GameObject obj = Instantiate(attackFly, transform.position, Quaternion.identity) as GameObject;

        // SoundManage�� sfxObject�� �߰�.
        if (obj.GetComponent<AudioSource>() != null)
        {
            SoundManager.instance.sfxDestoryObjects.Add(obj.GetComponent<AudioSource>());
            obj.GetComponent<AudioSource>().volume = SoundManager.instance.GetSFXVolume();
        }    

        roomInfo.GetComponent<Room>().enemis.Add(obj);
    }

}
