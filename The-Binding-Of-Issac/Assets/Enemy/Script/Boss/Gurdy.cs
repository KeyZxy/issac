using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gurdy : TEnemy
{
    /// <summary>
    /// 
    /// 1. ������ ����
    /// 2. �¿쿡�� ����ź ��
    /// 3. �տ��� 4���� �Ѿ� ��
    /// 4. �տ� pooter �θ��� ��ȯ
    /// 
    /// </summary>
    /// 
    [Header("TEnemy")]
    [SerializeField] GameObject bigShootBullet;
    [SerializeField] GameObject pooter;
    [SerializeField] Animator childAni;
    public Transform[] Children;

    [SerializeField] float  stateTime;
    [SerializeField] int    stateNum;
    [SerializeField] float  currTime;                // ���� ������ �ð�
    bool coruState;
    Coroutine runningCoroutine = null;
    bool isGene;


    [SerializeField] AudioClip[] gurdySound;
    void Start()
    {
        // ������ , �ڽ� �迭 �޾ƿ���
        // index0 : ����
        // index1 : �Ӹ�
        // index2 : right
        // index3 : down
        // index4 : left
        // index5 : top
        // index6 : left top
        // index7 : genePosi

        Children = gameObject.GetComponentsInChildren<Transform>();
        animator = GetComponent<Animator>();
        childAni = Children[1].gameObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        playerInRoom        = false;
        dieParameter        = "isDie";

        // Enemy
        hp                  = 450f;
        waitforSecond       = 1f;   // �ױ��� �ð�
        attaackSpeed        = 1.5f; // �Ѿ� �߻� �ϴ� �ð� 
        bulletSpeed         = 5f;

        maxhp               = hp;

        //Gurdy
        randTime();

        currTime = stateTime;
        coruState = true;
        isGene = true;
    }

    private void Update()
    {
        if (playerInRoom) 
        {
            Move();
        }
    }

    void Move() 
    {
        if (e_isDead())
        {
            childAni.SetBool("isDie" , true);       // �ױ��� �Ӹ� �ִϸ��̼�
            animator.SetBool("isBeforeDie" , true); // �ױ��� ���� �ִϸ��̼�

            e_destroyEnemy();
        }


        currTime -= Time.deltaTime;
        if (currTime > 0)
        {

        }
        else if (currTime <= 0) 
        {
            if (runningCoroutine != null) //�������� �ڷ�ƾ�� ������
            {
                // ���߱�
                StopCoroutine(runningCoroutine);
                childAni.SetBool("isOpps", false);
                childAni.SetBool("isdisapear", false);
                childAni.SetBool("isShoot", false);
                childAni.SetBool("isHi", false);
                isGene = true;
            }


            // �ʱ�ȭ
            randNum();
            randTime();
            currTime = stateTime;

            if (stateNum == 1)
                gurdyShoot();
            else if (stateNum == 2)
                gurdyGeneFly();

            coruState = true;
        }

    }

    void gurdyShoot() 
    {
        if (coruState)
        {

            childAni.SetBool("isOpps", true);
            childAni.SetBool("isdisapear", true);
            runningCoroutine = StartCoroutine(ShootBullets());
            coruState = false;
        }
    }

    IEnumerator ShootBullets()
    {
        float randGravityScale;
        childAni.SetBool("isShoot", true);

        while (true)
        {
            int rand = Random.Range(2, 7);                  // �������� (2~6)
            GameObject _obj = Children[rand].gameObject;    // ���� ���⿡ ���� �ڽ� ������Ʈ

            float randWait = Random.Range(0.03f, 0.1f);      // ���� �Ѿ˹߻� ������ �ð�
            bool isbullet = true;
            if (isbullet)
            {

                GameObject bulletobj = EnemyPooling.Instance.GetStraightBullet(_obj);
                
                bulletobj.GetComponent<Rigidbody2D>().velocity = _obj.transform.right * bulletSpeed;

                randGravityScale = Random.Range(0f, 1f);
                bulletobj.GetComponent<Rigidbody2D>().gravityScale += randGravityScale;
                isbullet = false;
            }
            yield return new WaitForSeconds(randWait);
        }

    }

    void gurdyGeneFly() 
    {
        childAni.SetBool("isappear", true);
        childAni.SetBool("isHi", true) ;

        if (isGene)
        { 
            GenerateAttackFly();
            isGene = false;
        }

    }

    void GenerateAttackFly()
    {
        WhatIsThisSound();

        GameObject obj = Instantiate(pooter, Children[7].transform.position, Quaternion.identity) as GameObject;

        // SoundManage�� sfxObject�� �߰�.
        if (obj.GetComponent<AudioSource>() != null)
        {
            SoundManager.instance.sfxDestoryObjects.Add(obj.GetComponent<AudioSource>());
            obj.GetComponent<AudioSource>().volume = SoundManager.instance.GetSFXVolume();
        }

        roomInfo.GetComponent<Room>().enemis.Add(obj);
    }

    void WhatIsThisSound()
    {
        audioSource.clip = gurdySound[Random.Range(0, gurdySound.Length)];
        audioSource.Play();
    }

    void randTime()
    {
        //1f ~ 10f ���̿��� �ð�
        stateTime = Random.Range(1f, 3f);
    }

    void randNum()
    {
        stateNum = Random.Range(1, 3); // 1~2��
    }
}
