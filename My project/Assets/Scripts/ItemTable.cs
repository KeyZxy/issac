using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTable : MonoBehaviour
{
    [SerializeField]
    GameObject[] DropItems; // 掉落道具数组
    // 0 : 金币 , 1 : 红心 , 2 : 炸弹 , 3 : 钥匙

    [SerializeField]
    GameObject[] PassiveItems; // 被动道具数组

    [SerializeField] private List<int> passive; // 被动道具索引列表


    [Header("Pooling")]
    [SerializeField] Transform dropItemPool_Transform; // 掉落道具池的父物体
    Stack<GameObject> coinPool = new Stack<GameObject>(); // 金币池
    Stack<GameObject> heartPool = new Stack<GameObject>(); // 心池
    Stack<GameObject> bombPool = new Stack<GameObject>(); // 炸弹池
    Stack<GameObject> keyPool = new Stack<GameObject>(); // 钥匙池

    private void Start()
    {
        // 初始化掉落道具和池
        Itemduplication();
        SetPooling();
    }

    #region initialization
    void Itemduplication()
    {
        passive = new List<int>();

        for (int i = 0; i < PassiveItems.Length; i++)
            passive.Add(i); // 添加被动道具索引


    }

    void SetPooling()
    {
        // 初始化池
        coinPool = new Stack<GameObject>();
        heartPool = new Stack<GameObject>();
        bombPool = new Stack<GameObject>();
        keyPool = new Stack<GameObject>();

        // 创建对象并放入池中
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
                    CreateCoin(); // 如果金币池为空，创建金币
                }
                GameObject coin = coinPool.Pop();
                coin.SetActive(true);
                coin.GetComponent<Coin>().SetCollisionDelay(true); // 设置碰撞延迟
                return coin;
            #endregion
            #region 
            case 1:
                if (heartPool.Count == 0)
                {
                    CreateHeart(); // 如果心池为空，创建心
                }
                GameObject heart = heartPool.Pop();
                heart.SetActive(true);
                heart.GetComponent<Heart>().SetCollisionDelay(true); // 设置碰撞延迟
                return heart;
            #endregion
            #region 
            case 2:
                if (bombPool.Count == 0)
                {
                    CreateBomb(); // 如果炸弹池为空，创建炸弹
                }
                GameObject bomb = bombPool.Pop();
                bomb.SetActive(true);
                bomb.GetComponent<DropBomb>().SetCollisionDelay(true); // 设置碰撞延迟
                return bomb;
            #endregion
            #region 
            case 3:
                if (keyPool.Count == 0)
                {
                    CreateKey(); // 如果钥匙池为空，创建钥匙
                }
                GameObject key = keyPool.Pop();
                key.SetActive(true);
                key.GetComponent<key>().SetCollisionDelay(true); // 设置碰撞延迟
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
                dropItem = GetDropItem(itemCode); // 获取掉落道具
                dropItem.transform.position = dropPosition;
                dropItem.GetComponent<Coin>().DropCoin();
                break;
            case 1:
                dropItem = GetDropItem(itemCode); // 获取掉落道具
                dropItem.transform.position = dropPosition;
                dropItem.GetComponent<Heart>().DropHeart();
                break;
            case 2:
                dropItem = GetDropItem(itemCode); // 获取掉落道具
                dropItem.transform.position = dropPosition;
                dropItem.GetComponent<DropBomb>().DropBomb_move();
                break;
            case 3:
                dropItem = GetDropItem(itemCode); // 获取掉落道具
                dropItem.transform.position = dropPosition;
                dropItem.GetComponent<key>().DropKey();
                break;
            default:
                dropItem = GetDropItem(itemCode); // 获取掉落道具
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
            coinPool.Push(dropitem); // 将道具返回池中
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
            ReturnDropItem(obj); // 将所有掉落道具返回池中
        }
    }
    #endregion



    public GameObject DropPassive()
    {
        GameObject obj;
        if (passive.Count == 0)
        {
            // 如果被动道具列表为空，返回null或者其他默认行为
            return null;
        }
        ShuffleList(ref passive); // 随机排序被动道具列表
        obj = PassiveItems[passive[0]]; // 获取第一个被动道具
        passive.RemoveAt(0); // 从列表中移除该道具
        return obj;
    }
    public GameObject OpenGoldChest()//随机生成被动道具
    {
        List<int> topThreePassiveIndices = new List<int> { 0, 1, 2 }; // 定义一个列表包含前三个被动道具的索引
        ShuffleList(ref topThreePassiveIndices); // 对索引列表进行洗牌

        // 根据洗牌后的顺序依次返回前三个被动道具
        foreach (int index in topThreePassiveIndices)
        {
            if (index < passive.Count)
            {
                return PassiveItems[index];
            }
        }

        // 如果前三个被动道具已空，则返回第一个被动道具
        return PassiveItems[0];
    }
    private void ShuffleList(ref List<int> list)
    {
        int rd1, rd2;
        int tmp;

        for (int i = 0; i < list.Count; i++)
        {
            rd1 = Random.Range(0, list.Count); // 生成第一个随机索引
            rd2 = Random.Range(0, list.Count); // 生成第二个随机索引

            tmp = list[rd1]; // 交换两个索引位置的元素
            list[rd1] = list[rd2];
            list[rd2] = tmp;
        }
    }

    public Sprite GetStuffImage(int index)
    {
        return PassiveItems[index].GetComponent<SpriteRenderer>().sprite; // 返回被动道具的精灵图片
    }

    #region archive
    public GameObject ObjectBreak() 
    {
        int rd = Random.Range(0, DropItems.Length - 1); // 生成一个0到DropItems长度减1之间的随机数
        return DropItems[rd]; // 返回随机索引位置的掉落道具
    }

    public GameObject OpenNormalChest(int rd)
    {
        //return GetDropItem(rd);
        return DropItems[rd]; // 返回指定索引位置的掉落道具
    }
    #endregion
}