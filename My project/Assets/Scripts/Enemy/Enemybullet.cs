using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Enemybullet : MonoBehaviour
{
    public float Speed = 10.0f;
    public float LiveTime = 3.0f;
    public float harm = 1.0f;
    private float _HasLiveTime = 0;

    public Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _HasLiveTime += Time.deltaTime;
        if (_HasLiveTime > LiveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door"))//撞墙消失
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Player"))//打中角色
        {
            Vector2 force = (other.transform.position - transform.position).normalized;
            other.GetComponent<Player>().BeAttacked(2, force, 1.5f);
            Destroy(gameObject);
        }
    }

    public void SetSpeed(Vector2 speed)
    {
        rigidbody.velocity = speed;
    }
}
