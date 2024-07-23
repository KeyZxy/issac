using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 15f;
    private Vector2 pos;
    

    void Update()
    {
       Destroy(gameObject,3f);
       transform.Translate(pos * (Speed * Time.deltaTime));
    }

    public void SetDirection(Vector2 dir)
    {
        pos = dir;
    }

    
}
