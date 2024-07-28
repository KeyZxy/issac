using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float shotSpeed = 10f;//����
    public float LiveTime = 2f;
    public Animator tearBoomAnim;
    private Vector2 _Direction;
    public Vector3 playerPosition;
    private float _HasLiveTime = 0;
    public float playerRange = 5f;//���


    private void Start()
    {
        tearBoomAnim = GetComponent<Animator>();

    }
    void Update()
    {

        _HasLiveTime += Time.deltaTime;
        if (_HasLiveTime > LiveTime)
        {
            Destroy(gameObject);
        }
        transform.Translate(_Direction * (shotSpeed * Time.deltaTime));
        if (Vector3.Distance(transform.position, playerPosition) > playerRange)
        {
            Destroy(gameObject);
        }
    }
    public void BasicSet(Vector2 dir)
    {
        _Direction = dir;
    }
    public void SetPlayerPosition(Vector3 playerPos)
    {
        playerPosition = playerPos;
    }
    
    public void OnTriggerEnter2D(Collider2D collision)//�����������ײ����ʱ��
    {
        string tag = collision.gameObject.tag;
        Rigidbody2D rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
        if (tag == "Wall"||tag=="Door"||tag=="Enemy")
        {
            tearBoomAnim.SetTrigger("BoomTear");
            Destroy(gameObject);//ֻҪ��ײ����ײ��ʹݻ��ӵ�����
        }
        
    }

}
