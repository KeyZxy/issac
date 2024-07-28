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
    //状态
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

    public int maxHealth = 6;//最大血量
    public int health = 6;//血量
    public float speed = 5f;//移速
    public float SpeedMultiple = 1f;//道具改变移速量
    public float playerRange = 5f;//射程
    private float shotTiming = 0;//射击计时
    public int BombNum = 10;//炸弹数量
    public int KeyNum = 3;//钥匙数量
    public int CoinNum = 10;//金币数量
    public int HeartNum = 0;//心之容器数量
    public float Knockback = 1;//击退
    //伤害
    public float Damage
    {
        //伤害 = 3.5 * 伤害倍数 * √(基础伤害 * 1.2f + 1f)+ 额外伤害
        get { return (float)Math.Round(3.5f * DamageMultiple * Mathf.Sqrt((DamageBase * 1.2f + 1f)) + DamageAdded, 2); }
    }
    public float DamageMultiple = 1f;
    public float DamageBase = 0f;
    public float DamageAdded = 0f;
    //射速Tears
    //用于计算射击延迟,一般道具的射速效果作用于此
    public float Tears = 0;
    //射击延迟,用于计算射击间隔,某些道具作用于Multiple（倍数）和Added（额外）
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
    //射击间隔,用于实际的发射计算
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
        //归一化，取值0 - 1,斜着走的速度不会超过移动速度。
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
        _Rigid.velocity = moveInput * speed;
    }

    // 射击
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
    // 创建并设置子弹
    void ShootBullet(Vector2 direction)
    {

        GameObject bulletObj = Instantiate(BulletPrefab);
        bulletObj.transform.position = transform.position;
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.BasicSet(direction);
        bullet.SetPlayerPosition(transform.position);
        bullet.playerRange = playerRange;

    }
    // 设置炸弹
    void GenerateBomb()
    {
        if (BombNum >= 1)
        {
            BombNum--;
            Vector2 pos = transform.position + new Vector3(0, -0.15f);
            Instantiate<TheBomb>(bombPrefab, pos, Quaternion.identity);


        }
    }
    //获取基础掉落物
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
    /// 被击退效果
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    IEnumerator knockBackCoroutine(Vector2 force)
    {
        //降低输入操作带来的移动量
        SpeedMultiple = 0.5f;

        float length = 0.3f;
        float overTime = 0.1f;
        float timeleft = overTime;
        while (timeleft > 0)
        {
            //overTime时间内移动direction * length的距离
            transform.Translate(force * length * Time.deltaTime / overTime);
            timeleft -= Time.deltaTime;
            yield return null;
        }

        //还原
        SpeedMultiple = 1;
    }
    /// <summary>
    /// 进入无敌状态并闪烁
    /// </summary>
    IEnumerator Invincible()
    {
        isInvincible = true;
        Color red = new Color(1, 0.2f, 0.2f, 1);

        float time = 0;//计时
        float flashCD = 0;//闪烁计时

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
