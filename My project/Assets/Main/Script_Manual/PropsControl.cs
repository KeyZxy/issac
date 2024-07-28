using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopsControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int state = 0;
    public Animator poopsanima;
    public GameObject poops;
    public GameObject coin;
    private bool created = false;
    void Start()
    {
        poopsanima = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 4)
        {
            poops.GetComponent<Collider2D>().enabled = false;
            Destroy(poops.GetComponent<Rigidbody2D>());
            if (created == false)
            {
                Instantiate(coin, transform.position, Quaternion.identity);
                created = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            if (state <4)
            {
                state += 1;
                poopsanima.SetInteger("state", state);
            }
        }
    }

}
