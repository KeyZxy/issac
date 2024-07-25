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
    public int maxHealth = 6;//���Ѫ��
    public int health = 6;//Ѫ��
    public float speed = 3.0f;//����
    public float playerRange = 5f;//���
    private float shotTiming=0;//�����ʱ
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

}
