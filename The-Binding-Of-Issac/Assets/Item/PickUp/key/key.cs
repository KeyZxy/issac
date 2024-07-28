using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator keyAni;

    public AudioSource audioSource;

    public AudioClip pickupClip;
    public AudioClip dropClip;

    public Sprite defaultSprite;

    [SerializeField] bool collisionDelay;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>(); // �и� �ʱ�ȭ�� ����µ�
        keyAni = GetComponent<Animator>();
    }

    public void DropKey()
    {
        // ���� ����� ��� �浹 ����
        collisionDelay = false;
        StartCoroutine(CollisionDelay());

        DropSound();


        float randomX = Random.Range(-1.0f, 1.0f);
        float randomY = Random.Range(-1.0f, 1.0f);
        float randomForce = Random.Range(50f, 70f);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(randomX, randomY) * randomForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collisionDelay)
        {
            GetSound();

            gameObject.layer = 31;
            keyAni.SetTrigger("GetKey");
        }
    }

    public void GetKey()
    {
        ItemManager.instance.keyCount++;
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

    void DropSound()
    {
        if(audioSource == null) // �� ���ٰ� �ߴ��� �𸣰ھ�
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.volume = SoundManager.instance.GetSFXVolume();
        audioSource.clip = dropClip;
        audioSource.Play();
    }

    void GetSound()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioSource.volume = SoundManager.instance.GetSFXVolume();
        audioSource.clip = pickupClip;
        audioSource.Play();
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
