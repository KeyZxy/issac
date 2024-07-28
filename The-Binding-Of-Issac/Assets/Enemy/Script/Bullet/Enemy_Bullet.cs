using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    [Header("Enemy_Bullet")]
    protected Animator ani;
    protected float bulletSpeed;
    protected float waitForDest;

    protected Vector3 bulletDesti;
    protected Transform playerPosi;

    protected bool enemyBulletIsBomb;

    public Vector3 property_BulletDesti     { get => bulletDesti;  set { bulletDesti = value; } }
    public Transform property_PlayerPosi    { get => playerPosi;  set { playerPosi = value; } }
    public Animator property_Ani            { get => ani; set { ani = value; } }
    public float property_BulletSpeed       { get => bulletSpeed; set { bulletSpeed = value; } }
    public float property_WaitForDest       { get => waitForDest; set { waitForDest = value; } } 
    public bool property_enemyBulletIsBomb { get => enemyBulletIsBomb; set { enemyBulletIsBomb = value; } }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // �÷��̾�� ���̸� ����
        {
            ani.SetTrigger("bulletDestroy");
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            PlayerManager.instance.GetDamage(); //�÷��̾�� ������

            enemyBulletIsBomb = false;
        }

        //�� �Ǵ� ���� ������
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Object_Rock"))
        {
            ani.SetTrigger("bulletDestroy");
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

            enemyBulletIsBomb = false;
        }

        //�˿� ������
        else if (collision.gameObject.CompareTag("Object_Poop"))
        {
            ani.SetTrigger("bulletDestroy");
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

            collision.gameObject.GetComponent<Poop>().GetDamage();
            enemyBulletIsBomb = false;
        }
        //�ҿ� ������
        else if (collision.gameObject.CompareTag("Object_Fire"))
        {
            ani.SetTrigger("bulletDestroy");
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

            collision.gameObject.GetComponent<FirePlace>().GetDamage();
            enemyBulletIsBomb = false;
        }
    }

}
