using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlace : Obstacle
{
    AudioSource sfxAudio;
    [Header("Unity SetUp")]
    [SerializeField] Sprite woodSprite;
    [SerializeField] GameObject eft;
    [SerializeField] GameObject boxCollider;
    [SerializeField] AudioClip destoryClip;
    [SerializeField] AudioClip fireClip;

    Sprite defaultSprite;
    Vector3 defaultScale;
    protected override void initialization()
    {
        objectLayer = 0;

        sfxAudio = GetComponent<AudioSource>();
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
        defaultScale = eft.transform.localScale;
    }

    public override void ResetObject()
    {
        //�ʱ�ȭ
        spriteIndex = 0;
        FireSound();
        eft.SetActive(true);
        boxCollider.SetActive(true);
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
        gameObject.layer = objectLayer;
        eft.transform.localScale = defaultScale;

        //������Ʈ ����
        gameObject.SetActive(false);
    }

    public override void Returnobject()
    {
        GameManager.instance.roomGenerate.FirePool.Push(gameObject);
    }


    public override void GetDamage()
    {
        if (spriteIndex > 4)
            return;

        spriteIndex++;
        ChangeObjectSPrite();
        if (spriteIndex >= 4)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = woodSprite;
            gameObject.layer = noCollisionLayer;

            DestorySound();

            eft.SetActive(false);
            boxCollider.SetActive(false);

            DropItem();
        }
    }

    protected override void ChangeObjectSPrite()
    {
        eft.transform.localScale = new Vector3(eft.transform.localScale.x - 0.2f, eft.transform.localScale.y - 0.2f, 0);
    }

    protected override void DestorySound()
    {
        AudioSource objectAudio = gameObject.GetComponent<AudioSource>();
        objectAudio.Stop();
        objectAudio.playOnAwake = false;
        objectAudio.loop = false;
        objectAudio.clip = destoryClip;
        objectAudio.Play();
    }

    protected override void DropItem()
    {
        int rd = Random.Range(0, 3);
        if (rd == 0)
        {
            rd = Random.Range(0, 4);
            ItemManager.instance.itemTable.Dropitem(transform.position, rd);
        }
    }


    void FireSound()
    {
        AudioSource objectAudio = gameObject.GetComponent<AudioSource>();
        objectAudio.playOnAwake = true;
        objectAudio.loop = true;
        objectAudio.clip = fireClip;
        objectAudio.Play();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.instance.GetDamage();
        }
    }
}
