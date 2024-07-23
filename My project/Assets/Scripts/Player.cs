using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject Bullet;
    private SpriteRenderer SR;
    public Rigidbody2D rb;
    private Animator _Animator;
    private Transform m_Transform;
    public Vector2 moveInput;
    public float speed = 3.0f;
    public float cooltime = 1.0f;
    public float speedmultiple = 1.0f;
    

    void Move()
    {
        var h = Input.GetAxis("Horizontal_Player");
        var v = Input.GetAxis("Vertical_Player");
        moveInput = h * Vector2.right + v * Vector2.up;
        //归一化，取值0 - 1,斜着走的速度不会超过移动速度。
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
        rb.velocity = moveInput * speed * speedmultiple ;
    }
    
    void Fire(Vector2 direction)
    {
        GameObject bulletObj = Instantiate(Bullet, transform.position, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(direction);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        _Animator = GetComponent<Animator>();
        m_Transform = this.transform;
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Fire(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Fire(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Fire(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Fire(Vector2.right);
        }
        
        
    }
}
