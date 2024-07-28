using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float Speed = 10.0f;
    public float LiveTime = 3.0f;

    private Vector2 _Direction;
    private float _HasLiveTime = 0;

    private void Update()
    {
        _HasLiveTime += Time.deltaTime;
        if (_HasLiveTime > LiveTime)
        {
            Destroy(gameObject);
        }

        transform.Translate(_Direction * (Speed * Time.deltaTime));
    }

    public void SetDirection(Vector2 dir)
    {
        _Direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door"))//撞墙消失
        {
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Clotty"))//打中怪物
        {
            Destroy(gameObject);
        }

    }
}
