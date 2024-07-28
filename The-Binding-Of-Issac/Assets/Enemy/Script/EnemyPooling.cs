using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    /// <summary>
    /// 
    ///  - �Ѿ� pooling -
    /// pooling�� �ϳ� ����� ���� , ���� ���Ͱ� pooling�� ����
    /// 
    /// </summary>

    public static EnemyPooling Instance;

    [Header("pooling")]
    [SerializeField] Transform[] childArr;
    // index 0 : ����
    // index 1 : straightBullet �� ���� ����
    // index 2 : followBullet ``

    [SerializeField] Transform straightPooling_parent;  // �Ѿ��� ��� ���� �θ� (�� ������Ʈ)
    [SerializeField] Transform followPooling_Parent;    // ``

    [Header("pooling ������Ʈ")]
    [SerializeField] private GameObject straightBullet;
    [SerializeField] private GameObject followBullet;
    [SerializeField] Sprite oriBulletImage;
    [SerializeField] int cntBullet;

    [Header("pooliong queue")]
    Queue<GameObject> poolingStraightBullet     = new Queue<GameObject>();
    Queue<GameObject> poolingFollowBullet       = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        cntBullet = 10;

        // ���߿� playermanager...? ���� �ʱ�ȭ�ϱ�
        EnemyBullet_Initialize(cntBullet);
    }

    // �ʱ�ȭ ,  queue�� �̸� initCount ��ŭ �־��
    private void EnemyBullet_Initialize(int initCount)
    {
        childArr = gameObject.GetComponentsInChildren<Transform>();         // �ڽĿ�����Ʈ�� �迭��
        straightPooling_parent  = childArr[1];                       // �迭�� 1
        followPooling_Parent    = childArr[2];                       // �迭�� 2 

        for (int i = 0; i < initCount; i++) 
        {
            poolingStraightBullet.Enqueue(createStraightBullet());  //queue �� �� �߰�
            poolingFollowBullet.Enqueue(createFollowBullet());
        }
    }

    // straight bullet ����
    private GameObject createStraightBullet() 
    {
        // ������Ʈ ���� 
        GameObject newObj = Instantiate(straightBullet) as GameObject;
        newObj.gameObject.SetActive(false);                         // queue�� ���� ���� �� ���̰� 
        newObj.transform.SetParent(straightPooling_parent);         // �θ���
        newObj.transform.localPosition = Vector3.zero;              // ��ġ�� 0,0,0
        return newObj;
    }

    // follow Bullet ����
    private GameObject createFollowBullet()
    {
        GameObject newObj = Instantiate(followBullet) as GameObject;
        newObj.gameObject.SetActive(false);                        // queue�� ���� ���� �� ���̰� 
        newObj.transform.SetParent(followPooling_Parent);          // �θ���
        newObj.transform.localPosition = Vector3.zero;
        return newObj;
    }

    // �ٸ� ��ũ��Ʈ���� straightBullet ������ �� ����ϴ�
    public GameObject GetStraightBullet(GameObject shootPosi) 
    {
        GameObject obj;

        // straight pool�� ������Ʈ�� ����� ������
        if (Instance.poolingStraightBullet.Count > 0)
        {
            obj = Instance.poolingStraightBullet.Dequeue(); // queue���� �� �տ� �ִ°� ����
        }
        // straight pool�� ������Ʈ�� ����� ���� ������
        else
        {
            obj = Instance.createStraightBullet();      // ���ο� �Ѿ� �����
            poolingStraightBullet.Enqueue(obj);  //queue �� �� �߰�
        }

        obj.transform.position = shootPosi.transform.position;  // ��ũ��Ʈ �����ϴ� ��ġ��
        // ������ ������Ʈ�� return
        obj.GetComponent<SpriteRenderer>().sprite = oriBulletImage;
        obj.gameObject.SetActive(true);                         // pooling �迭���� setActive(false)�� �س��� �� ������ ���̰� ����

        // ���� ����
        Enemy_Bullet bulletScript = obj.GetComponent<Enemy_Bullet>();
        bulletScript.property_Ani = obj.gameObject.GetComponent<Animator>();
        bulletScript.property_WaitForDest = 0.5f;
        bulletScript.property_BulletSpeed = 5f;

        return obj;
    }

    // �ٸ� ��ũ��Ʈ���� straightBullet ������ �� ����ϴ�
    public GameObject GetFollowBullet(GameObject shootPosi)
    {
        GameObject obj;

        // straight pool�� ������Ʈ�� ����� ������
        if (Instance.poolingStraightBullet.Count > 0)
        {
            obj = Instance.poolingFollowBullet.Dequeue(); // queue���� �� �տ� �ִ°� ����
        }
        // straight pool�� ������Ʈ�� ����� ���� ������
        else
        {
            obj = Instance.createFollowBullet();      // ���ο� �Ѿ� �����
            poolingFollowBullet.Enqueue(obj);  //queue �� �� �߰�
        }

        obj.transform.position = shootPosi.transform.position;  // ��ũ��Ʈ �����ϴ� ��ġ��
        // ������ ������Ʈ�� return
        obj.GetComponent<SpriteRenderer>().sprite = oriBulletImage;
        obj.gameObject.SetActive(true);                         // pooling �迭���� setActive(false)�� �س��� �� ������ ���̰� ����

        //���ü���
        Enemy_Bullet bulletScript = obj.GetComponent<Enemy_Bullet>();
        bulletScript.property_Ani = obj.gameObject.GetComponent<Animator>();
        bulletScript.property_WaitForDest = 0.5f;
        bulletScript.property_BulletSpeed = 5f;

        bulletScript.property_PlayerPosi = GameObject.FindWithTag("Player").transform;
        bulletScript.property_BulletDesti
            = new Vector3(bulletScript.property_PlayerPosi.transform.position.x,
                                bulletScript.property_PlayerPosi.transform.position.y, 0);
        bulletScript.property_enemyBulletIsBomb = true;

        return obj;
    }


    /// <summary>
    /// 
    /// 1. �ٸ� ������Ʈ���� �Ѿ��� �����ϰ� �ı��ɶ�, 
    /// 2. return���� pooling�迭�� �־���
    /// 
    /// 3. bulletDestroy �ִϸ��̼��� ���� ��, �ִϸ��̼� �̺�Ʈ�� ��  
    /// 
    /// </summary>

    public void returnBullet(GameObject obj) 
    {
        // straight ���� follow ���� �˻��ؼ� 
        // ���� �ش��ϴ� queue�� �־������

        if (obj.GetComponent<EnemyStraightBullet>() != null)        // straight ��ũ��Ʈ�� ���������?
        {
            // �θ��� �Ʒ� (0,0,0)����
            obj.transform.localPosition = Vector3.zero;
            
            obj.gameObject.SetActive(false);
            poolingStraightBullet.Enqueue(obj);            // �ǵ��ƿ��� �ٽ� pooling �迭�� �־���
        }

        else if (obj.GetComponent<EnemyFollowBullet>() != null)     // follow ��ũ��Ʈ�� ��� ������?
        {
            // �θ��� �Ʒ� (0,0,0)����
            obj.transform.localPosition = Vector3.zero;

            // ���󰡴� �Ѿ� �ʱ�ȭ
            obj.GetComponent<Enemy_Bullet>().property_BulletDesti = Vector3.zero;
            obj.GetComponent<Enemy_Bullet>().property_PlayerPosi = null;

            obj.gameObject.SetActive(false);
            poolingFollowBullet.Enqueue(obj);              // �ǵ��ƿ��� �ٽ� pooling �迭�� �־���
        }
      
    }

    

  
}
