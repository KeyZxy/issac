using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject BulletPrefab;
    private Rigidbody2D _Rigid;
    private Animator _Animator;
    public Vector2 moveInput;
    public int maxHealth = 6;//最大血量
    public int health = 6;//血量
    public float speed = 3.0f;//移速
    public float playerRange = 5f;//射程
    private float shotTiming=0;//射击计时
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
        
        _Animator = GetComponent<Animator>();
        
    }
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Move(x,y);
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

}
