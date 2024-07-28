using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundWorm : MonoBehaviour
{
    public GameObject roundWorm;
    public GameObject sliceKey;

    public Room inWhichRoom;//在哪个房间
    public bool created = false;//有爆过银钥匙？
    public enum State { Idle, Active, Die };//三个状态
    public State state;
    public Rigidbody2D _rigid;
    public float MaxHp = 4.0f;//最大生命值
    public float CurrentHp;
    public Animator headanima;
    public Animator bodyanima;
    public bool CanBeAttacked = true;//可受击？

    public SpriteRenderer sr;
    public Color StartColor;
    private enum Direction { up, down, left, right };
    [Header("ActiveInformation")]
    private Direction direction;
    private float act_frequency = 2.0f;
    private float invoketime = 0.0f;
    public GameObject enemybullet;
    public float speed = 2.0f;//怪的速度
    public float EnemyBullet_Speed = 5.0f;//子弹速度
    public bool randomActive = false;//false——走 true——攻击

    public GameObject Head;
    public GameObject Body;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        CurrentHp = MaxHp;
        _rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHp <= 0.0f)
        {
            //Debug.Log("RoundWorm Has Died");
            state = State.Die;
            roundWorm.SetActive(false);
            if (created == false && GameObject.FindWithTag("Player").GetComponent<Player>().goldKey == false)
            {
                Instantiate(sliceKey, transform.position, Quaternion.identity);
                created = true;
            }
            created = true;
            inWhichRoom.EnemyNum -= 1;
        }

        if (state == State.Active)
        {
            invoketime += Time.deltaTime;
            if (invoketime > act_frequency)
            {
                invoketime = 0f;
                if (randomActive == true)
                {
                    RandomAttack();
                }
                else
                {
                    RandomMove();
                }
                randomActive = !randomActive;
            }
        }
    }


    public void CanActive()
    {
        state = State.Active;
    }

    public void RandomMove()
    {
        CanBeAttacked = false;
        //headanima.SetTrigger("GoDown");
        headanima.SetBool("UpToDown", true);
        headanima.SetBool("DownToUp", false);
        InvokeMove();
    }

    public void InvokeMove()
    {
        direction = (Direction)UnityEngine.Random.Range(0, 4);
        switch (direction)
        {
            case Direction.up:
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, gameObject.transform.position + new Vector3(0f, 1f, 0f) , speed*Time.deltaTime);
                gameObject.transform.position = gameObject.transform.position + new Vector3(0f, 1f, 0f);
                //CanBeAttacked = true;
                break;
            case Direction.down:
                //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, gameObject.transform.position + new Vector3(0f, -1f, 0f), speed * Time.deltaTime);
                gameObject.transform.position = gameObject.transform.position + new Vector3(0f, -1f, 0f);
                //CanBeAttacked = true;
                break;
            case Direction.left:
                gameObject.transform.position = gameObject.transform.position + new Vector3(-1f, 0f, 0f);
                //CanBeAttacked = true;
                break;
            case Direction.right:
                gameObject.transform.position = gameObject.transform.position + new Vector3(1f, 0f, 0f);
                //CanBeAttacked = true;
                break;
        }
        //headanima.SetTrigger("GoUp"); 
        headanima.SetBool("UpToDown", false);
        headanima.SetBool("DownToUp", true);
        CanBeAttacked = true;
    }

    public void RandomAttack()
    {
        Vector3 targetdir = (GameObject.FindWithTag("Player").transform.position - gameObject.transform.position).normalized * EnemyBullet_Speed;
        GameObject bullet = Instantiate(enemybullet, transform.position, Quaternion.identity);
        bullet.GetComponent<Enemybullet>().SetSpeed(targetdir);
    }

    private void OnTriggerEnter2D(Collider2D collision)//被攻击
    {
        if (collision.gameObject.CompareTag("Bullet") && CanBeAttacked == true)
        {
            Destroy(collision.gameObject);
            _rigid.AddForce(collision.gameObject.GetComponent<Rigidbody2D>().velocity.normalized, ForceMode2D.Impulse);
            CurrentHp -= 1.0f;
            HitColor();
        }
    }

    public void HitColor()
    {
        sr.color = Color.red;
        Invoke("ResetColor", 0.1f);
    }

    public void ResetColor()
    {
        sr.color = StartColor;
    }
}
