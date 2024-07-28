using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBomb : MonoBehaviour
{
    private Animator gb;
    public Sprite defaultSprite;


    bool collisionDelay;
    private void Start()
    {
        gb = GetComponent<Animator>();
    }

    public void DropBomb_move()
    {
        // ���� ����� ��� �浹 ����
        collisionDelay = false;
        StartCoroutine(CollisionDelay());

        float randomX = Random.Range(-1.0f, 1.0f);
        float randomY = Random.Range(-1.0f, 1.0f);
        float randomForce = Random.Range(50f, 70f);
        //���� ���� �� (����, ������ / �� , �Ʒ�) ������ �������� ���� �����̱� ���� AddForce
        GetComponent<Rigidbody2D>().AddForce(new Vector2(randomX, randomY) * randomForce);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collisionDelay)
        {
            gameObject.GetComponent<AudioSource>().volume = SoundManager.instance.GetSFXVolume();
            gameObject.GetComponent<AudioSource>().Play(); // ȹ�� ����

            gameObject.layer = 31; // �÷��̾�� �浹�����ʴ� ���̾�
            gb.SetTrigger("GetBomb");
            ItemManager.instance.bombCount++;
        }
    }


    //�Ծ��� �� �ִϸ��̼� �̺�Ʈ (������� �����)
    public void GetBomb()
    {
        transform.localPosition = Vector3.zero;
        StartCoroutine(Delay());
    }


    public void ResetObject()
    {
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
        gameObject.layer = 14;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        ItemManager.instance.itemTable.ReturnDropItem(gameObject);
    }

    IEnumerator CollisionDelay()
    {
        yield return new WaitForSeconds(0.2f);
        collisionDelay = true;
    }

    public void SetCollisionDelay(bool state)
    {
        collisionDelay = state;
    }
}
