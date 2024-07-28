using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using System;
using System.Runtime.Serialization;

public class Player : MonoBehaviour, IAttackable
{

    [Header("Sprite")]
    public Transform head;
    public Transform body;
    SpriteRenderer bodyRenderer;
    SpriteRenderer headRenderer;
    Rigidbody2D _Rigid;
    //״̬
    [HideInInspector]
    public bool isLive = true;
    bool isControllable = true;
    bool isInvincible = false;
    public bool goldKey = false;
    [Header("Function")]
    Vector2 moveInput;

    [Header("PlayerState")]
    public GameObject BulletPrefab;
    public TheBomb bombPrefab;
    //private Animator _Animator;

    public int maxHealth = 6;//���Ѫ��
    public int health = 6;//Ѫ��
    public float speed = 5f;//����
    public float SpeedMultiple = 1f;//���߸ı�������
    public float playerRange = 5f;//���
    private float shotTiming = 0;//�����ʱ
    public int BombNum = 10;//ը������
    public int KeyNum = 3;//Կ������
    public int CoinNum = 10;//�������
    public int HeartNum = 0;//��֮��������
    public float Knockback = 1;//����
    //�˺�
    public float Damage
    {
        //�˺� = 3.5 * �˺����� * ��(�����˺� * 1.2f + 1f)+ �����˺�
        get { return (float)Math.Round(3.5f * DamageMultiple * Mathf.Sqrt((DamageBase * 1.2f + 1f)) + DamageAdded, 2); }
    }
    public float DamageMultiple = 1f;
    public float DamageBase = 0f;
    public float DamageAdded = 0f;
    //����Tears
    //���ڼ�������ӳ�,һ����ߵ�����Ч�������ڴ�
    public float Tears = 0;
    //����ӳ�,���ڼ���������,ĳЩ����������Multiple����������Added�����⣩
    public int TearsDelay
    {
        get
        {
            int temp;
            if (Tears >= 0)
            {
                temp = (int)(16 - 6 * Mathf.Sqrt(1.3f * Tears + 1));
            }
            else if (Tears >= -10f / 13f)
            {
                temp = (int)((16 - 6 * Mathf.Sqrt(1.3f * Tears + 1)) - 6 * Tears);
            }
            else
            {
                temp = (int)(16 - 6 * Tears);
            }

            temp = temp > 5 ? temp : 5;
            temp = temp * TearsDelayMultiple + TearsDelayAdded;
            return temp > 1 ? temp : 1;
        }
    }
    public int TearsDelayMultiple = 1;
    public int TearsDelayAdded = 0;
    //������,����ʵ�ʵķ������
    private float ShotCD
    {
        get { return 1f / (30f / (TearsDelay + 1)); }
    }

    void Start()
    {
        _Rigid = GetComponent<Rigidbody2D>();
        headRenderer = head.GetComponent<SpriteRenderer>();
        bodyRenderer = body.GetComponent<SpriteRenderer>();
        bodyRenderer.enabled = true;
        headRenderer.enabled = true;
        // _Animator = GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Move(x, y);
        Shoot();

    }
    void Move(float x, float y)
    {
        moveInput = x * Vector2.right + y * Vector2.up;
        //��һ����ȡֵ0 - 1,б���ߵ��ٶȲ��ᳬ���ƶ��ٶȡ�
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
        _Rigid.velocity = moveInput * speed;
    }

    // ���
    void Shoot()
    {
        if (isLive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ShootBullet(Vector2.up);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ShootBullet(Vector2.down);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ShootBullet(Vector2.right);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ShootBullet(Vector2.left);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                GenerateBomb();
            }
        }
    }
    // �����������ӵ�
    void ShootBullet(Vector2 direction)
    {

        GameObject bulletObj = Instantiate(BulletPrefab);
        bulletObj.transform.position = transform.position;
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.BasicSet(direction);
        bullet.SetPlayerPosition(transform.position);
        bullet.playerRange = playerRange;

    }
    // ����ը��
    void GenerateBomb()
    {
        if (BombNum >= 1)
        {
            BombNum--;
            Vector2 pos = transform.position + new Vector3(0, -0.15f);
            Instantiate<TheBomb>(bombPrefab, pos, Quaternion.identity);


        }
    }
    //��ȡ����������
    public void OnTriggerEnter2D(Collider2D collision)
    {

        GetDropItem(collision.gameObject);


    }
    void GetDropItem(GameObject prof)
    {
        string itemTag = prof.tag;
        switch (itemTag)
        {
            case "Heart":
                health += 2;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                Destroy(prof);
                break;
            case "Coin":
                CoinNum += 1;
                Destroy(prof);
                break;
            case "Bomb":
                BombNum += 1;
                Destroy(prof);
                break;
            case "Key":
                KeyNum += 1;
                Destroy(prof);
                break;
            case "GoldKey":
                Destroy(prof);
                goldKey = true;
                break;

        }


    }
    public void BeAttacked(float damage, Vector2 direction, float forceMultiple = 1)
    {
        if (isInvincible || !isLive) { return; }
        health -= ((int)damage);
        if (health <= 0)
        {
            PlayerDeath();
        }
        if (isLive)
        {
            StartCoroutine(knockBackCoroutine(direction * forceMultiple));
            StartCoroutine(Invincible());
        }
    }

    /// <summary>
    /// ������Ч��
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    IEnumerator knockBackCoroutine(Vector2 force)
    {
        //������������������ƶ���
        SpeedMultiple = 0.5f;

        float length = 0.3f;
        float overTime = 0.1f;
        float timeleft = overTime;
        while (timeleft > 0)
        {
            //overTimeʱ�����ƶ�direction * length�ľ���
            transform.Translate(force * length * Time.deltaTime / overTime);
            timeleft -= Time.deltaTime;
            yield return null;
        }

        //��ԭ
        SpeedMultiple = 1;
    }
    /// <summary>
    /// �����޵�״̬����˸
    /// </summary>
    IEnumerator Invincible()
    {
        isInvincible = true;
        Color red = new Color(1, 0.2f, 0.2f, 1);

        float time = 0;//��ʱ
        float flashCD = 0;//��˸��ʱ

        while (time < 1f)
        {
            time += Time.deltaTime;
            flashCD += Time.deltaTime;
            if (flashCD > 0)
            {
                if (bodyRenderer.color == Color.white)
                {
                    bodyRenderer.color = red;
                    headRenderer.color = red;
                }
                else if (bodyRenderer.color == red)
                {
                    bodyRenderer.color = Color.white;
                    headRenderer.color = Color.white;
                }
                flashCD -= 0.13f;
            }
            yield return null;
        }
        isInvincible = false;
    }
    public void PlayerDeath()
    {
        isLive = false;
        isControllable = false;
        bodyRenderer.enabled = false;
        headRenderer.enabled = false;
        _Rigid.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void PlayerPause()
    {
        isControllable = false;
        _Rigid.velocity = Vector2.zero;
    }
    public void PlayerQuitPause()
    {
        isControllable = true;
    }

}
