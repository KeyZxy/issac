using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : TEnemy
{
    /// <summary>
    /// <Larry J>
    /// 1. player�� �濡 ������ ���� ���� , isPlayerInRoom ���� �ʿ����
    /// 2. �������� FIxedUpdate���� ����
    /// 3. animator�� �Ӹ��� ������ �� head�� �ִ��̼����� ������ 
    /// 
    /// </summary>

    [Header("Larry")]
    [SerializeField] float distanceBetween;
    [SerializeField] int larryLength; // body ���� ��
    [SerializeField] GameObject larryHead_;
    [SerializeField] GameObject larryBody_;

    [SerializeField] List<GameObject> bodyParts = new List<GameObject>(); // Larry�� ��, ���� ������Ʈ�� ����
    public List<GameObject> snakeBody = new List<GameObject>();

    float countUp = 0;
    [SerializeField] Vector3 MoveDir; //������ ���� (��, �Ʒ�, ��,  ��)
    public int stateNum; //���� ������ ��ȣ
    [SerializeField] float stateTime; //���� ������ �ð�
    [SerializeField] float currTime; //���� ������ �ð�
    [SerializeField] bool chageState; // ���º�ȯ 
    [SerializeField] bool isReadyDie; //������ �� ���� �Ǿ����� true
    float iniHp; // �ʱ� hp ����

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //SnakeManager
        // bodyParts�迭�� �� �־��ֱ�
        larryLength = 12; //head ���� 12 �� 
        countUp = 0;
        distanceBetween = 0.2f;

        CreateBodyParts();  // bodyPart List�� �� �ֱ�
        CreateSnakeBody(); //  SnakepPart List�� �� �ֱ� 

        // Enemy
        hp = 110f;
        sight = 5f;
        moveSpeed = 5f; // �̰� �ٲٸ� distanceBetween�� �ٲ㼭 ���� �ϴ� Ÿ�̹� �������!!!
        waitforSecond = 0.3f;
        maxhp = hp;

        //Snake
        stateNum = 0; //���� ��ȣ
        stateTime = 3f;
        chageState = true;
        isReadyDie = false;
        randTime(); //�ʱ⿡ ������ �ð� �ѹ� ���س���
        currTime = stateTime; // �ʱ⿡ ���� �ð�
        iniHp = hp;

    }
    
    private void FixedUpdate()
    {
        if (bodyParts.Count > 0)
        {
            CreateSnakeBody();
        }
        Move();
    }
    
    private void Update()
    {
        //������ �� ����������� �˻�
        if (!isReadyDie)
            hp = iniHp;
        if (snakeBody.Count == larryLength)
            isReadyDie = true;

        //����
        larryDie();
    }

    //�������� �Դ�
    public void getDamageLarry() 
    {
        base.GetDamage(PlayerManager.instance.playerDamage);
    }


    //��ź�� �������� �Դ�
    public void getBombDamage(float bombDamage) 
    {
        base.GetDamage(bombDamage);
    }

    //�������� �ִ�
    public void hitDamagePlayer() 
    {
        //�÷��̾�� �ε����� �÷��̾��� hp����
        PlayerManager.instance.GetDamage();
    }

    // �����̴� ���� ����
    public void Move()
    {
        SnakeMovement();

        //���� ���ϱ�
        currTime -= Time.deltaTime;
        if (currTime >= 0 && chageState)
        {
            randNum(); //���� ���� ���ϱ� (1~4)
            chageAniDir(); // �ִϸ��̼ǰ� ���� �ٲ�
            chageState = false;
        }
        else if (currTime <= 0)
        {
            stateReset();
        }
    }

    //�ʱ�ȭ
    public void stateReset() 
    {  
        randTime(); // ���� Ÿ�� ���ϱ�
        currTime = stateTime; //�ð� �ʱ�ȭ
        chageState = true;

    }

    //��ũ��Ʈ�� bodyParts�迭�� ������Ʈ �ֱ� (start���� �ѹ� ����)
    public void CreateBodyParts() 
    {
        bodyParts.Add(larryHead_);          //�Ӹ��ְ�
        for(int i= 1;i<larryLength; i++)    //������ŭ ����
        {
            bodyParts.Add(larryBody_);
        }
    }

    // �ϴ� �ϵ��ڵ� �س���, ���߿� �����غ���!
    // �ǰ� 10 �پ��� ���� �ϳ��� ������
    public void larryDie() 
    {
        //�ǰ� 90 �����̰�
        if (hp <= 90)
        {
            // ������ũ �迭�� 12�� �̸� (�� ������), snakeBody�� �ִ°� �ϳ� �����
            if (snakeBody.Count == larryLength) 
            {
                GameObject deleSnake = snakeBody[larryLength - 1];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie" , true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake , waitforSecond);
                DestorySound();
            }
        }
        // �ǰ� 80 �����̰�
        if (hp <= 80f)
        {
            // ������ũ �迭�� 11�� �̸�, snakeBody�� �ִ°� �ϳ� �����
            if (snakeBody.Count == larryLength-1 )
            {
                GameObject deleSnake = snakeBody[larryLength - 2];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake, waitforSecond);
                DestorySound();
            }
        }
        // �ǰ� 70 �����̰�
        if (hp <= 70f )
        {
            // ������ũ �迭�� 10�� �̸�, snakeBody�� �ִ°� �ϳ� �����
            if (snakeBody.Count == larryLength - 2)
            {
                GameObject deleSnake = snakeBody[larryLength - 3];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake, waitforSecond);
                DestorySound();
            }
        }
        if (hp <= 60f )
        {
            // ������ũ �迭�� 9�� �̸�, snakeBody�� �ִ°� �ϳ� �����
            if (snakeBody.Count == larryLength - 3)
            {
                GameObject deleSnake = snakeBody[larryLength - 4];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake, waitforSecond);
                DestorySound();
            }
        }
        if (hp <= 50f )
        {
            // ������ũ �迭�� 8�� �̸�, snakeBody�� �ִ°� �ϳ� �����
            if (snakeBody.Count == larryLength - 4)
            {
                GameObject deleSnake = snakeBody[larryLength - 5];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake, waitforSecond);
                DestorySound();
            }
        }
        if (hp <= 40f )
        {
            // ������ũ �迭�� 7�� �̸�, snakeBody�� �ִ°� �ϳ� �����
            if (snakeBody.Count == larryLength - 5)
            {
                GameObject deleSnake = snakeBody[larryLength - 6];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake, waitforSecond);
                DestorySound();
            }
        }
        if (hp <= 30f )
        {
            // ������ũ �迭�� 6�� �̸�, snakeBody�� �ִ°� �ϳ� �����
            if (snakeBody.Count == larryLength - 6)
            {
                GameObject deleSnake = snakeBody[larryLength - 7];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake, waitforSecond);
            }
        }
        if (hp <= 20f )
        {
            // ������ũ �迭�� 4�� �̸�, snakeBody�� �ִ°� 1 + 1 �����
            if (snakeBody.Count == larryLength - 7)
            {
                GameObject deleSnake = snakeBody[larryLength - 8];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake, waitforSecond);
                // �ϳ� �� �����
                GameObject deleSnake2 = snakeBody[larryLength - 9];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake2);
                Destroy(deleSnake2 , waitforSecond);
                DestorySound();
            }
        }

        //�ǰ� 10 ������ ��, �Ӹ� 1 + ������ 1��
        if (hp <= 10f)
        {
            // ������ũ �迭�� 3�� �̸�, snakeBody�� �ִ°� 1�����
            if (snakeBody.Count == larryLength - 9)
            {
                GameObject deleSnake = snakeBody[larryLength - 10];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake, waitforSecond);
                DestorySound();
            }
        }

        // ������
        if(hp <= 0) 
        {
            //�������� �����ִ� body �����
            if (snakeBody.Count == larryLength - 10)
            {
                GameObject deleSnake = snakeBody[snakeBody.Count -1];
                deleSnake.GetComponent<Animator>().SetBool("isLarryBodyDie", true);
                snakeBody.Remove(deleSnake);
                Destroy(deleSnake, waitforSecond);
                DestorySound();
            }

            // �״� �ִϸ��̼� ����
            animator.SetBool("isLarryDie", true);
            // Destory
            Destroy(gameObject , waitforSecond);
        }


    }

    void DestorySound()
    {
        audioSource.clip = SoundManager.instance.GetEnemyDeadClip();
        audioSource.Play();
    }

    // ���¿� ���� �ִϸ��̼ǰ� ���� ��ȯ
    public void chageAniDir() 
    {
        if (stateNum == 1)
        { 
            MoveDir = Vector3.up;
            animator.SetTrigger("isLarryTop");
        }
        else if (stateNum == 2) 
        {
            MoveDir = Vector3.down;
            animator.SetTrigger("isLarryDown");
        }
        else if (stateNum == 3) 
        { 
            MoveDir = Vector3.left;
            animator.SetTrigger("isLarryLeft");
        }
        else if (stateNum == 4) 
        { 
            MoveDir = Vector3.right;
            animator.SetTrigger("isLarryRight");
        }

    }

    //���� ���� 
    void randNum()
    {
        // state 1.up , 2. down , 3.left , 4. right ���� �߿� �ϳ��� �������� ��
        int rand = 0;
        bool isreturn = true;

        //�� ó������ ������ ����������
        if (stateNum == 0)
        {
            stateNum = 4;
        }
        else
        {
            while (true) 
            {
                rand = Random.Range(1, 5); // 1~4��
                if (rand != stateNum) //���� ���°� �ƴϸ�
                {
                    stateNum = rand;

                    isreturn = true;
                    break;
                }
            }
            if (isreturn)
                return;
        }

    }
    //���� �ð�
    void randTime()
    {
        //1f ~ 10f ���̿��� �ð�
        stateTime = Random.Range(0.3f, 1f);
    }

    // ������
    void SnakeMovement()
    {
        //snakeBody[0]�� �Ӹ� , �Ӹ��� �������� MoveDir�������� �̵�
        snakeBody[0].transform.position += MoveDir * moveSpeed * Time.deltaTime;



        // snakeBody�� ������ ������ ( = �Ӹ��� �����ϰ� ������ ������)
        // ������ �����̰� ���� �ɾ������!
        if (snakeBody.Count > 1)
        {
            // for���� ���鼭 ù��° ����~������ �̵�
            for (int i = 1; i < snakeBody.Count; i++)
            {
                // ������ ������ MarkManager�� ������
                MarkManager markM = snakeBody[i - 1].GetComponent<MarkManager>();
                // i��° ������ ��ġ�� "������ ����"�� ��ġ
                snakeBody[i].transform.position = markM.markerList[0].position;
                // i��° ������ ȸ���� ``
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                // �ѹ� �̵� ������ list�� ������
                markM.markerList.RemoveAt(0);
            }
        }

    }

    //���� ���� �ڵ�
    void CreateSnakeBody()
    {
        //�Ӹ� ���� (snakeBody�� �ƹ��͵� ���� �Ǿ����� ���� ��)
        if (snakeBody.Count == 0)
        {
            // bodyParts�� ù����(�Ӹ�) �κ���, ������ġ�� ����
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            //ù��° ��� (�Ӹ�)�� �ִϸ����� ��������
            animator = temp1.GetComponent<Animator>();

            // ������Ʈ �߰�
            // MarkManager������Ʈ , Rigidbody2D������Ʈ (Rigidbody2D�� ���� �Ⱦ����� �̸� �־���´ٰ� ��������) 
            if (!temp1.GetComponent<MarkManager>())
                temp1.AddComponent<MarkManager>();

            // snakeBody�� instanceȭ �� ������Ʈ �߰�
            snakeBody.Add(temp1);
            // bodyParts�� ù��° ���� (ù��°�� �����ϸ鼭 �ڿ� �ִ� ������Ʈ�� �ε����� ������ / 1�� 0����, 2��1��.....)
            bodyParts.RemoveAt(0);
        }


        //snakeBody�� ���� �ٷ� �տ� (�ε��� -1)�� MarkManager�� ������
        MarkManager markM = snakeBody[snakeBody.Count - 1].GetComponent<MarkManager>();
        // countUp�� �ʱ�ȭ �� �� ���� (0 �϶� ����)
        if (countUp == 0)
        {
            // ���� �տ� �ִ� (�ε��� -1) ����Ʈ�� ����
            markM.ClearmMarkerList();
        }

        // ���������� ���� , distanceBetween ����
        countUp += Time.deltaTime;
        if (countUp >= distanceBetween)
        {
            // bodyParts�� ù��°�� ���� ������ ������Ʈ�� ��ġ�� ����
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position, markM.markerList[0].rotation, transform);
            
            //LarryHeadParent���� ù���� ������Ʈ�� �ڽ�����
            temp.transform.SetParent(snakeBody[0].transform);

            // ������Ʈ �߰�
            if (!temp.GetComponent<MarkManager>())
            {
                temp.AddComponent<MarkManager>();
            }

            // snakeBody�� �߰�
            snakeBody.Add(temp);
            // �߰��� ������Ʈ�� bodyParts���� �����
            bodyParts.RemoveAt(0);
            // �߰��� ������Ʈ�� ����Ʈ�� ����
            temp.GetComponent<MarkManager>().ClearmMarkerList();
            // count �ʱ�ȭ
            countUp = 0;
        }
    }


}