using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poops : MonoBehaviour
{
    public int state = 0;
    public Animator poopsanima;
    // Start is called before the first frame update
    void Start()
    {
        poopsanima = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            if (state < 4)
            {
                state += 1;
                poopsanima.SetInteger("state", state);
            }
        }
    }
}
