using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClottyControl : MonoBehaviour
{
    public GameObject clotty;
    public Room inWhichRoom;
    public enum State {Idle,Active,Die};
    public State state;
    public Rigidbody2D _rigid;
    public float MaxHp = 6.0f;
    public float CurrentHp;
    public Animator anima;

    public SpriteRenderer sr;
    public Color StartColor;

    [Header("MoveControl")]
    public int RundomNum;
    private enum Direction { up, down, left, right };
    private Direction direction;
    private float attack_frequency = 3.0f;
    private float invoketime = 0.0f;
    public GameObject enemybullet;
    public float speed = 1.5f;
    public float EnemyBullet_Speed = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        CurrentHp = MaxHp;
        _rigid = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
        sr= GetComponent<SpriteRenderer>();
        StartColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHp <= 0.0f)
        {
            state = State.Die;
            clotty.SetActive(false);
            inWhichRoom.EnemyNum -= 1;
        }
        if (state == State.Active)
        {
            RandomMove();
        }
    }

    public void CanActive()
    {
        state = State.Active;
    }
    
    public void RandomMove()
    {
        invoketime += Time.deltaTime;
        if (invoketime > attack_frequency)
        {
            invoketime = 0f;
            direction = (Direction)UnityEngine.Random.Range(0, 4);
            GameObject bullet_up = Instantiate(enemybullet, transform.position, Quaternion.identity);
            GameObject bullet_down = Instantiate(enemybullet, transform.position, Quaternion.identity);
            GameObject bullet_left = Instantiate(enemybullet, transform.position, Quaternion.identity);
            GameObject bullet_right = Instantiate(enemybullet, transform.position, Quaternion.identity);
            anima.SetBool("isAttack", true);
            bullet_up.GetComponent<EnemyBulletControl>().SetSpeed(new Vector2(0,-EnemyBullet_Speed));
            bullet_down.GetComponent<EnemyBulletControl>().SetSpeed(new Vector2(0,-EnemyBullet_Speed));
            bullet_left.GetComponent<EnemyBulletControl>().SetSpeed(new Vector2(-EnemyBullet_Speed, 0));
            bullet_right.GetComponent<EnemyBulletControl>().SetSpeed(new Vector2(EnemyBullet_Speed, 0));
            switch (direction)
            {
                case Direction.up:
                    _rigid.AddForce(new Vector2(0, speed), ForceMode2D.Impulse);
                    break;
                case Direction.down:
                    _rigid.AddForce(new Vector2(0, -speed), ForceMode2D.Impulse);
                    break;
                case Direction.left:
                    _rigid.AddForce(new Vector2(-speed, 0), ForceMode2D.Impulse);
                    break;
                case Direction.right:
                    _rigid.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
                    break;
            }
        }
        anima.SetBool("isAttack", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)//±»¹¥»÷
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            _rigid.AddForce(collision.gameObject.GetComponent<Rigidbody2D>().velocity.normalized, ForceMode2D.Impulse);
            CurrentHp -= 1.0f;
            HitColor();
        }
    }

    public void HitColor()//¹¥»÷ÉÁË¸
    {
        sr.color = Color.red;
        Invoke("ResetColor", 0.1f);
    }

    public void ResetColor()
    {
        sr.color = StartColor;
    }
}
