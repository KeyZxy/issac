using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public enum TENEMY_STATE // ��ũ��Ʈ�� ���� ���� 
{
    Idle,
    Prowl,
    Tracking,
    Shoot,
    Jump,
    Die
}

public class TEnemy : MonoBehaviour
{
    /// <summary>
    /// - ���� § Zombie_FSM �� ����� �ϴ�, �ӽ��� ���ư��� ��ü 
    /// - Enemy ��ũ��Ʈ�� �� ����
    /// </summary>

    // TEnemy�� ���� �ӽ�
    public HeadMachine<TEnemy> headState;

    /// <summary>
    /// 1. ���� �̸� ������ ���� state�迭
    /// 2. System.Enum.GetValues(typeof(state�̸�)).Length : enum Ÿ���� ���� ���ϱ� ,  ���⼭�� 5
    /// </summary>
    public FSM<TEnemy>[] arrayState = new FSM<TEnemy>[System.Enum.GetValues(typeof(TENEMY_STATE)).Length];

    // ��� ���� Enemy�� ����
    [SerializeField] protected bool isTracking;              // tracking �ϴ°�?
    [SerializeField] protected bool isProwl;                 // prowl (��ȸ) �ϴ°�?
    [SerializeField] protected bool isDetective;             // detective (����) �ϴ°�?
    [SerializeField] protected bool isShoot;                 // shoot (�� ��) �ϴ°�?
    [SerializeField] protected bool isJump;                  // jump (����) �ϴ°�?

    // enemy ���� ������Ƽ
    public bool getIsTracking { get => isTracking; }
    public bool getisProwl { get => isProwl; }
    public bool getisDetective { get => isDetective; }
    public bool getisShoot { get => isShoot; }
    public bool getisJump { get => isJump; }

    // ��������
    private int stage;

    // Enemy�� ������ ���� �⺻ ���ȵ�
    public bool playerInRoom;                   // �÷��̾ �濡 ���Դ��� ����
    [SerializeField] protected float hp;                         // hp
    protected float sight;                      // �þ� ����
    protected float moveSpeed;                  // ������ �ӵ�
    protected float waitforSecond;              // destroy �� ��� �ð� 
    protected float attaackSpeed;               // ���� �ӵ�
    protected float bulletSpeed;                // �Ѿ� �ӵ� 
    protected float fTime;                      // prowl - ���� �̵� �ð�
    protected float randRange;                  // prowl - ���� �̵� �Ÿ�
    protected float jumpSpeed;                  // jump -  ���� �ӵ�
    protected int enemyNumber = -1;                  // enemyNumber = ���� ������ȣ ( Ǯ��

    //
    protected string dieParameter;              // �״�   �ִϸ��̼� ���� �Ķ����
    protected string shootParameter;            // �ѽ�� �ִϸ��̼� ���� �Ķ����
    protected string jumpParameter;             // ����   �ִϸ��̼� ���� �Ķ����
    protected bool knockBackState = false;      // �˹�s
    protected bool isFlipped = true;                      // ������
    bool isRaadyShoot;                          // shoot -  �� ��� ����

    // prowl
    float mx;                         // ���� x
    float my;                         // ���� y
    float xRan;                       // random - x ���� ������
    float yRan;                       // random - y ���� ������


    // enemy ���� ������Ƽ
    public bool setIsReadyShoot
    {
        set { isRaadyShoot = value; }
    }
    public float getMoveSpeed { get => moveSpeed;  }
    public float getattaackSpeed { get => attaackSpeed; }
    public float JumpSpeed
    {
        get => jumpSpeed;
        set { JumpSpeed = value; }
    }

    // Enemy�� Hp��
    protected float maxhp;                      // hp ���� max 
    public Image hpBarSlider;                   // hp ���� �̹���

    // ������Ʈ
    public GameObject enemyBullet;              // �Ѿ�
    public GameObject roomInfo;                 // �� ���� ������Ʈ
    public Transform playerPosi;                // �÷��̾� ��ġ

    // Enemy�� ������ �ִ� ������Ʈ (init ���� �ʱ�ȭ)
    protected Animator animator;                // �ִϸ�����
    protected AudioSource audioSource;          // ���� �����

    // Enemy�� ���� (enum)
    public TENEMY_STATE eCurState;              // ���� ����
    public TENEMY_STATE ePreState;              // ���� ����

    /// <summary>
    /// 1. setStateArray()
    ///     - ���� �迭 arrayState�� ���� 
    ///     - ���� �迭�� this�� �־ ���� ���Ͱ� ��
    ///     - ���� ���Ϳ��� ���� ���� (idle)�� �����ϸ�, ���º�ȭ�� �� ��
    /// 2. En_setState()
    ///     - ���Ͱ� �ʿ��� ����
    ///     - ex) playerInRoom , hp, sight��
    /// 3, En_kindOfEnemy()
    ///     - � �ൿ�� �ϴ��� 
    ///     - isTracking(����) , isProwl(��ȸ) ,isDetective(����) �� true, false�� ǥ��
    /// 4. En_Start()
    ///     - ���� ���¸� ���� ���ִ� �Լ�
    ///     - ���� ������ start�� �������
    ///     
    /// </summary>


    protected void En_stateArray()
    {
        init();
    }

    // �ʿ��� ������ �ʱ�ȭ
    private void init()
    {
        // Enemy_HeadMachine�� Ÿ���� TEnemy�� ���� , �ӽ��� ���� (new ���)
        headState = new HeadMachine<TEnemy>();

        /// <summary>
        /// 1. �ش� ���� (��ũ��Ʈ)�� �����ڸ� ��� -> Enemy �� �پ��ִ� ��ü�� �Ѱ��� 
        /// 2. new ��ũ��Ʈ �̸� (�Ű�����) 
        /// 3. Enemy�� ��� �ϴ� �������� ���� Enemy Ÿ��(Enemy�� ��� �ް� �ֱ� ����)
        ///  
        /// Q. �� Enemy_Idle���� ��ũ��Ʈ�� FSM<TENemy> �迭�� ���� �� �ִ°�?
        ///     A. FSM<Zombie_FSM>�� ���� ��ũ��Ʈ���� FSM<Zombie_FSM>�� arr�� �ֱ⶧�� 
        /// Q. �� ��ư�  arrayState[(int)TENEMY_STATE.Idle]�̷��� ����? arr[0]�ϸ� �ȵ�?
        ///     A. �������� ���ؼ� , arr[0] �ϸ� �迭�� 0��°�� � Ÿ������ �𸣴ϱ�
        /// </summary>
        arrayState[(int)TENEMY_STATE.Idle]      = new Enemy_Idle(this);
        arrayState[(int)TENEMY_STATE.Prowl]     = new Enemy_Prowl(this);
        arrayState[(int)TENEMY_STATE.Tracking]  = new Enemy_Tracking(this);
        arrayState[(int)TENEMY_STATE.Shoot]     = new Enemy_Shoot(this);
        arrayState[(int)TENEMY_STATE.Jump]      = new Enemy_Jump(this);
        arrayState[(int)TENEMY_STATE.Die]       = new Enemy_Die(this);
        //Debug.Log(this.gameObject.tag); -> �ڽ� ������Ʈ���� �����ϸ� �ڽ� ������Ʈ�� this

        // Enemy ���¸� Idle ���·� �ʱ� ����
        headState.Setstate(arrayState[(int)TENEMY_STATE.Idle], this);

        // ������Ʈ �ʱ�ȭ
        animator = this.gameObject.GetComponent<Animator>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    public virtual void En_setState() { }         // �ʱ� ���� (Getcomponent, hp ���� ��)
    public virtual void En_kindOfEnemy() { }      // Enemy�� ���� (isTracking , isProwl ,isDetective )
    public void En_Start()                        // ���� ���� start���� ����         
    {
        e_chageHp();                              // hp ����
        En_stateArray();                          // �ʱ� �迭 ����

        // Head Machine�� Enter�� ����,
        // �ʱ� ���°��� idle�̹Ƿ� idle�� excute �� �����Ѵ�. 
        E_Enter();                                // ���� ���� (idle)�� begin ���� 
    }

    public void E_Enter()
    {

        /// <summary>
        /// - init()�޼��忡�� ���� ���¸� idle �� ����
        /// - idle ������ Enter �޼��� ����
        /// </summary>
        headState.H_Enter();
    }
    public void E_Excute()
    {
        headState.H_Excute();
    }

    public void E_Exit()
    {
        headState.H_Exit(); ;
    }

    // ���������� hp ����
    public void e_chageHp() 
    {
        stage = GameManager.instance.stageLevel;    // �������� level ��������
        if (stage >= 2) 
        {
            hp = (stage * 2.5f) + hp;
            maxhp = hp;
        }

    }


    /// <summary>
    /// - ���� ��ȯ
    /// 1. ���� �ȿ��� (���� ��ũ��Ʈ �ȿ���) "������-> ��������" ��ȯ �� �� ���
    /// </summary>
    public void ChageFSM(TENEMY_STATE ps)
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(TENEMY_STATE)).Length; i++)
        {
            if (i == (int)ps)
                headState.Chage(arrayState[(int)ps]);
        }

    }

    /// <summary>
    /// * ��Ÿ ���� �޼���
    /// </summary>  

  

    // �÷��̾� �Ĵٺ���
    public void e_Lookplayer()
    {
        if (transform.position.x > playerPosi.position.x && isFlipped)
        {
            //transform.Rotate(0f, 180f, 0f);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            isFlipped = false;
        }
        else if (transform.position.x < playerPosi.position.x && !isFlipped)
        {
            //transform.Rotate(0f, 180f, 0f);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            isFlipped = true;
        }
    }
    // tracking�� enemy �� Ž��
    public void e_findPlayer()                             
    {
        playerPosi = GameObject.FindWithTag("Player").transform;
    }
    // tracking ������
    public void e_Tracking(float _speed)                               
    {
        if (knockBackState) // �˹� ����
            return;

        gameObject.transform.position
            = Vector3.MoveTowards(gameObject.transform.position, playerPosi.transform.position, _speed * Time.deltaTime);
    }

    // ���� �ȿ� player ����
    public bool e_SearchingPlayer()                         
    {
        float x = transform.position.x;
        float y = transform.position.y;
        Vector2 sightSize = new Vector2(x, y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(sightSize, sight); //���� ��ġ , ����

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    // enemy �ʱ� ��ġ
    public void e_moveInialize() 
    {
        mx = gameObject.transform.position.x;
        my = gameObject.transform.position.y;
        xRan = mx;
        yRan = my;
    } 

    //���� ������
    public void e_Prwol()
    {
        Vector3 moveRan = new Vector3(xRan, yRan, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, moveRan, moveSpeed * Time.deltaTime);
    }

    // �� ���
    public void e_Shoot() 
    {
        animator.SetTrigger(shootParameter);
        if (isRaadyShoot) {
            GameObject bulletobj = EnemyPooling.Instance.GetFollowBullet(this.gameObject);   // follow �ϴ� bullet pooling
            //bulletobj.transform.position = gameObject.transform.position;
            //GameObject bulletobj = Instantiate(enemyBullet, transform.position + new Vector3(0, -0.1f, 0), Quaternion.identity);
            isRaadyShoot = false;
        }
    }

    // Tracking -> Shoot���� �Ѿ�� ���� (invoke)
    public void invokeShoot()
    {
        Invoke("chageToShoot", attaackSpeed);             // attaackSpeed �Ŀ�  

    }
    public void chageToShoot()
    {
        ChageFSM(TENEMY_STATE.Shoot);           // Shoot���� ���� ��ȭ
    }

    // Tracking -> jump�� �Ѿ�� ���� (invoke)
    public void invokeJump()
    {
        Invoke("chageTojump", attaackSpeed);             //  attaackSpeed �Ŀ�

    }
    public void chageTojump()
    {
        ChageFSM(TENEMY_STATE.Jump);           // Shoot���� ���� ��ȭ
    }

    // jump ������ ����
    public void e_jumpSet()
    {
        animator.SetBool(jumpParameter, true);       //jumpAiPlayTime ��ŭ ���� ani ����
    }

    // jump -> tracking 
    public void e_doneJump()
    {
        animator.SetBool(jumpParameter, false);
        ChageFSM(TENEMY_STATE.Tracking);
    }

    // Prowl - ���� ������ �ڷ�ƾ ����
    public void startRaomPosiCoru()
    {
        StartCoroutine(checkPosi());
    }
    //���� ������ �˻�
    public IEnumerator checkPosi()
    {
        while (true)
        {
            yield return new WaitForSeconds(fTime);
            xRan = Random.Range(mx + randRange, mx - randRange);    // x��ġ�� ���� ��ġ randRange���� , ������ġ -randRange����
            yRan = Random.Range(my + randRange, my - randRange);    // y��ġ ����
        }
    }


    // �Ϲ� ���� collider�˻�
    private void OnCollisionStay2D(Collision2D collision)       
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.instance.GetDamage();                 //�÷��̾�� �ε����� �÷��̾��� hp����
        }
    }

    // �Ϲ� ���� trigger �˻�
    private void OnTriggerEnter2D(Collider2D collision)             
    {
        if (collision.gameObject.CompareTag("Tears"))               //�����̶� �ε����� �� ��ȭ
        {
            StartCoroutine(Hit());
        }
    }

    // �� ��ȭ �ڷ�ƾ
    IEnumerator Hit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // �˹� �ڷ�ƾ 
    public IEnumerator knockBack()
    {
        knockBackState = true;
        yield return new WaitForSeconds(0.2f);
        knockBackState = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    // �Ϲ� ���� hp ��
    // �Ϲ� ���� hp ��
    //public void CheckHp()
    //{
    //    hpBarSlider.fillAmount = hp / maxhp;
    //}
    public IEnumerator CheckHp()
    {
        //hpBarSlider.fillAmount = Mathf.Lerp(hpBarSlider.fillAmount, hp / maxhp, Time.deltaTime * 1f);
        while (hp <= maxhp)
        {
            float health = hp / maxhp;
            float currentHp = Mathf.Lerp(hpBarSlider.fillAmount, hp / maxhp, Time.deltaTime * 3.5f);
            hpBarSlider.fillAmount = currentHp;

            yield return null;
        }
    }

    // player ������ ��ŭ �� ���� (Tear ��ũ��Ʈ ���� ��� )
    public void GetDamage(float damage)
    {
        hp -= damage;
        StartCoroutine(CheckHp());
    }

    // enemy ����
    public bool e_isDead() 
    {
        if (hp <= 0)
            return true;
        return false;
    }

    // enemy ����
    public void e_destroyEnemy()
    {
        e_deadSound();
        animator.SetBool(dieParameter , true);
        Destroy(gameObject ,waitforSecond);
    }

    public void e_deadSound()
    {
        // ����ó�� - ������� ���� ������Ʈ
        if (audioSource == null) 
            return;

        //Debug.Log("dead");

        audioSource.clip = SoundManager.instance.GetEnemyDeadClip();
        audioSource.Play();
    }
}
