using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTable : MonoBehaviour
{
    GameObject roomObject;
    Room roomInfo;
    [Header("State")]
    [SerializeField] int cost;
    [SerializeField] GameObject item;
    public string[] itemInfomation = new string[2];

    [Header("Unty Setup")]
    [SerializeField] Transform itemPos;
    [SerializeField] GameObject Cost_10;
    [SerializeField] GameObject Cost_1;
    public Sprite[] costImages;
    private void FixedUpdate()
    {
        if(roomInfo != null)
        {
            // �÷��̾ �濡 ��������.
            if(roomInfo.playerInRoom)
            {
                int mode = Random.Range(0, 1000);
                SetShopItem(mode%3);

                roomInfo = null;
            }
        }

        if(item != null)
        {
            ItemLayer();
        }
    }

    public void ResetObject()
    {
        roomObject = null;
        roomInfo = null;
        cost = 0;
        if(item != null)
        {
            // ������������� Ȯ��
            // ����������̶�� ������ƮǮ�� �����ְ� 
            // �׿� �������̶�� ����
            if (CheckedDropItem(item))
            {
                item.GetComponent<Collider2D>().enabled = true; // �ݶ��̴� on 
                ItemManager.instance.itemTable.ReturnDropItem(item);
            }
            else
                Destroy(item);

            item = null;
        }
        itemInfomation[0] = "";
        itemInfomation[1] = "";

        Cost_1.GetComponent<SpriteRenderer>().sprite = costImages[0];
        Cost_10.GetComponent<SpriteRenderer>().sprite = costImages[0];

        gameObject.SetActive(false);

        gameObject.layer = 14;
    }

    bool CheckedDropItem(GameObject obj)
    {
        if (obj.GetComponent<key>() != null || obj.GetComponent<Heart>() != null || obj.GetComponent<DropBomb>() != null)
            return true;
        return false;
    }

    void ItemLayer()
    {
        if (GameManager.instance.playerObject.transform.position.y > gameObject.transform.position.y)
        {
            item.GetComponent<SpriteRenderer>().sortingOrder = 110;
        }
        else
        {
            item.GetComponent<SpriteRenderer>().sortingOrder = 90;
        }
    }

    public void SetRoomInfo(GameObject room)
    {
        roomObject = room;
        roomInfo = roomObject.GetComponent<Room>();
    }

    public void SetShopItem(int mode)
    {
        // ��Ƽ��/��ű�/�нú� ������ ����
        if (mode == 0)
        {
            cost = 15; // ��� ���� ( 15�� )
            ItemCost(cost);
            CreateItem();
        }

        // ��ȸ�� ����� 
        else
        {
            cost = Random.Range(4, 11); // ��� ���� ( 4 ~ 10 �� ���� )
            ItemCost(cost);
            CreateDropItem();
        }
    }

    void ItemCost(int cost)
    {
        string tmp = cost.ToString();
        if (tmp.Length == 1)
        {
            Cost_1.GetComponent<SpriteRenderer>().sprite = costImages[cost];
        }

        else
        {
            Cost_10.GetComponent<SpriteRenderer>().sprite = costImages[1];
            Cost_1.GetComponent<SpriteRenderer>().sprite = costImages[tmp[1] - '0'];
        }
    }

    public void CreateDropItem()
    {
        int rd = Random.Range(1, 4);
        item = ItemManager.instance.itemTable.GetDropItem(rd);
        item.GetComponent<Collider2D>().enabled = false; // �ݶ��̴� off
        item.transform.position = itemPos.position;
    }

    public void CreateItem()
    {
        // ������Ʈ ����
        item = Instantiate(ItemManager.instance.itemTable.OpenGoldChest()) as GameObject;

        item.GetComponent<Collider2D>().enabled = false; // �ݶ��̴� off
        item.GetComponent<Rigidbody2D>().velocity = Vector3.zero; // �ʱ�ȭ

        item.transform.SetParent(itemPos);
        item.transform.localPosition = Vector3.zero;

        Invoke("SetInfomation",0.2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ��
        if (collision.gameObject.CompareTag("Player") && ItemManager.instance.coinCount >= cost)
        {
            item.GetComponent<Collider2D>().enabled = true; // �������� �ݶ��̴��� �ٽ� ����.
            item.transform.position = collision.transform.position; // ���� ���� �ٷ� �÷��̾ ȹ���Ҽ��ֵ��� �÷��̾� ��ġ�� �����̵�
            ItemManager.instance.coinCount -= cost; // ���� ��� ���

            gameObject.layer = 31; // ���� ���̺�� �浹 ���ϵ��� ���̾� ����
        }
    }

    void SetInfomation()
    {
        if (item.GetComponent<ItemInfo>() != null)
        {
            itemInfomation[0] = item.GetComponent<ItemInfo>().itemTitle;
            itemInfomation[1] = item.GetComponent<ItemInfo>().itemInformation;
        }
        else if (item.GetComponent<ActiveInfo>() != null)
        {
            itemInfomation[0] = item.GetComponent<ActiveInfo>().itemTitle;
            itemInfomation[1] = item.GetComponent<ActiveInfo>().itemInformation;
        }
        else if (item.GetComponent<TrinketInfo>() != null)
        {
            itemInfomation[0] = item.GetComponent<TrinketInfo>().itemTitle;
            itemInfomation[1] = item.GetComponent<TrinketInfo>().itemInformation;
        }
    }
}
