using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTable : MonoBehaviour
{
    [SerializeField]
    GameObject[] DropItems; // �����������
    // 0 : ��� , 1 : ���� , 2 : ը�� , 3 : Կ��

    [SerializeField]
    GameObject[] PassiveItems; // ������������

    [SerializeField] private List<int> passive; // �������������б�


    [Header("Pooling")]
    [SerializeField] Transform dropItemPool_Transform; // ������߳صĸ�����
    Stack<GameObject> coinPool = new Stack<GameObject>(); // ��ҳ�
    Stack<GameObject> heartPool = new Stack<GameObject>(); // �ĳ�
    Stack<GameObject> bombPool = new Stack<GameObject>(); // ը����
    Stack<GameObject> keyPool = new Stack<GameObject>(); // Կ�׳�

    private void Start()
    {
        // ��ʼ��������ߺͳ�
        Itemduplication();
        SetPooling();
    }

    #region initialization
    void Itemduplication()
    {
        passive = new List<int>();

        for (int i = 0; i < PassiveItems.Length; i++)
            passive.Add(i); // ��ӱ�����������


    }

    void SetPooling()
    {
        // ��ʼ����
        coinPool = new Stack<GameObject>();
        heartPool = new Stack<GameObject>();
        bombPool = new Stack<GameObject>();
        keyPool = new Stack<GameObject>();

        // �������󲢷������
        for (int i = 0; i < 40; i++)
        {
            CreateCoin();
            CreateHeart();
            CreateBomb();
            CreateKey();
        }
    }

    void CreateCoin()
    {
        GameObject coin = Instantiate(DropItems[0], dropItemPool_Transform.position, Quaternion.identity);
        coin.transform.SetParent(dropItemPool_Transform);
        coinPool.Push(coin);
        coin.SetActive(false);
    }

    void CreateHeart()
    {
        GameObject heart = Instantiate(DropItems[1], dropItemPool_Transform.position, Quaternion.identity);
        heart.transform.SetParent(dropItemPool_Transform);
        heartPool.Push(heart);
        heart.SetActive(false);
    }

    void CreateBomb()
    {
        GameObject bomb = Instantiate(DropItems[2], dropItemPool_Transform.position, Quaternion.identity);
        bomb.transform.SetParent(dropItemPool_Transform);
        bombPool.Push(bomb);
        bomb.SetActive(false);
    }

    void CreateKey()
    {
        GameObject key = Instantiate(DropItems[3], dropItemPool_Transform.position, Quaternion.identity);
        key.transform.SetParent(dropItemPool_Transform);
        keyPool.Push(key);
        key.SetActive(false);
    }
    #endregion

    #region pooling
    public GameObject GetDropItem(int index)
    {
        switch (index)
        {
            #region 
            case 0:
                if (coinPool.Count == 0)
                {
                    CreateCoin(); // �����ҳ�Ϊ�գ��������
                }
                GameObject coin = coinPool.Pop();
                coin.SetActive(true);
                coin.GetComponent<Coin>().SetCollisionDelay(true); // ������ײ�ӳ�
                return coin;
            #endregion
            #region 
            case 1:
                if (heartPool.Count == 0)
                {
                    CreateHeart(); // ����ĳ�Ϊ�գ�������
                }
                GameObject heart = heartPool.Pop();
                heart.SetActive(true);
                heart.GetComponent<Heart>().SetCollisionDelay(true); // ������ײ�ӳ�
                return heart;
            #endregion
            #region 
            case 2:
                if (bombPool.Count == 0)
                {
                    CreateBomb(); // ���ը����Ϊ�գ�����ը��
                }
                GameObject bomb = bombPool.Pop();
                bomb.SetActive(true);
                bomb.GetComponent<DropBomb>().SetCollisionDelay(true); // ������ײ�ӳ�
                return bomb;
            #endregion
            #region 
            case 3:
                if (keyPool.Count == 0)
                {
                    CreateKey(); // ���Կ�׳�Ϊ�գ�����Կ��
                }
                GameObject key = keyPool.Pop();
                key.SetActive(true);
                key.GetComponent<key>().SetCollisionDelay(true); // ������ײ�ӳ�
                return key;
                #endregion
        }
        return null;
    }

    public void Dropitem(Vector3 dropPosition, int itemCode)
    {
        GameObject dropItem;
        switch (itemCode)
        {
            case 0:
                dropItem = GetDropItem(itemCode); // ��ȡ�������
                dropItem.transform.position = dropPosition;
                dropItem.GetComponent<Coin>().DropCoin();
                break;
            case 1:
                dropItem = GetDropItem(itemCode); // ��ȡ�������
                dropItem.transform.position = dropPosition;
                dropItem.GetComponent<Heart>().DropHeart();
                break;
            case 2:
                dropItem = GetDropItem(itemCode); // ��ȡ�������
                dropItem.transform.position = dropPosition;
                dropItem.GetComponent<DropBomb>().DropBomb_move();
                break;
            case 3:
                dropItem = GetDropItem(itemCode); // ��ȡ�������
                dropItem.transform.position = dropPosition;
                dropItem.GetComponent<key>().DropKey();
                break;
            default:
                dropItem = GetDropItem(itemCode); // ��ȡ�������
                dropItem.transform.position = dropPosition;
                dropItem.GetComponent<Coin>().DropCoin();
                break;
        }
    }

    public void ReturnDropItem(GameObject dropitem)
    {
        if (dropitem.GetComponent<Coin>() != null)
        {
            dropitem.GetComponent<Coin>().ResetObject();
            dropitem.SetActive(false);
            coinPool.Push(dropitem); // �����߷��س���
        }
        else if (dropitem.GetComponent<Heart>() != null)
        {
            dropitem.GetComponent<Heart>().ResetObject();
            dropitem.SetActive(false);
            heartPool.Push(dropitem);
        }
        else if (dropitem.GetComponent<DropBomb>() != null)
        {
            dropitem.GetComponent<DropBomb>().ResetObject();
            dropitem.SetActive(false);
            bombPool.Push(dropitem);
        }
        else if (dropitem.GetComponent<key>() != null)
        {
            dropitem.GetComponent<key>().ResetObject();
            dropitem.SetActive(false);
            keyPool.Push(dropitem);
        }
        else
        {
            Destroy(dropitem);
        }
    }

    public void AllReturnDropItem()
    {
        for (int i = 0; i < dropItemPool_Transform.childCount; i++)
        {
            GameObject obj = dropItemPool_Transform.GetChild(i).gameObject;
            ReturnDropItem(obj); // �����е�����߷��س���
        }
    }
    #endregion



    public GameObject DropPassive()
    {
        GameObject obj;
        if (passive.Count == 0)
        {
            // ������������б�Ϊ�գ�����null��������Ĭ����Ϊ
            return null;
        }
        ShuffleList(ref passive); // ������򱻶������б�
        obj = PassiveItems[passive[0]]; // ��ȡ��һ����������
        passive.RemoveAt(0); // ���б����Ƴ��õ���
        return obj;
    }
    public GameObject OpenGoldChest()//������ɱ�������
    {
        List<int> topThreePassiveIndices = new List<int> { 0, 1, 2 }; // ����һ���б����ǰ�����������ߵ�����
        ShuffleList(ref topThreePassiveIndices); // �������б����ϴ��

        // ����ϴ�ƺ��˳�����η���ǰ������������
        foreach (int index in topThreePassiveIndices)
        {
            if (index < passive.Count)
            {
                return PassiveItems[index];
            }
        }

        // ���ǰ�������������ѿգ��򷵻ص�һ����������
        return PassiveItems[0];
    }
    private void ShuffleList(ref List<int> list)
    {
        int rd1, rd2;
        int tmp;

        for (int i = 0; i < list.Count; i++)
        {
            rd1 = Random.Range(0, list.Count); // ���ɵ�һ���������
            rd2 = Random.Range(0, list.Count); // ���ɵڶ����������

            tmp = list[rd1]; // ������������λ�õ�Ԫ��
            list[rd1] = list[rd2];
            list[rd2] = tmp;
        }
    }

    public Sprite GetStuffImage(int index)
    {
        return PassiveItems[index].GetComponent<SpriteRenderer>().sprite; // ���ر������ߵľ���ͼƬ
    }

    #region archive
    public GameObject ObjectBreak() 
    {
        int rd = Random.Range(0, DropItems.Length - 1); // ����һ��0��DropItems���ȼ�1֮��������
        return DropItems[rd]; // �����������λ�õĵ������
    }

    public GameObject OpenNormalChest(int rd)
    {
        //return GetDropItem(rd);
        return DropItems[rd]; // ����ָ������λ�õĵ������
    }
    #endregion
}