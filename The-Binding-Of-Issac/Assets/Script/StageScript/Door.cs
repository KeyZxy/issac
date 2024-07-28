using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform movePosition;
    public GameObject roomInfo;
    public int doorDir = -1;
    public float doorDamage;

    [Header("Unity Setup")]
    public GameObject closeDoor;
    public GameObject openDoor;
    public bool doorKey; // ���谡 �ʿ��ѹ��� üũ ���� ( false )  ���谡 �ʿ���� ���� üũ ( true )

    public void CheckedClear()
    {
        // door key : true :  usingKey / normal room / boss room
        if(roomInfo.GetComponent<Room>().isClear && doorKey)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void UsingKey()
    {
        doorKey = true;
    }

    public void OpenDoor()
    {
        closeDoor.SetActive(false);
        openDoor.SetActive(true);
    }
    public void CloseDoor()
    {   
        closeDoor.SetActive(true);
        openDoor.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(roomInfo.GetComponent<Room>().isClear) // ���� Ŭ����Ȼ��� �϶�
        {
            if(collision.gameObject.CompareTag("Player") && doorKey) // ���� �ε��� ����� �÷��̾���
            {
                if (Input.GetKey(KeyCode.W) && doorDir == 0)
                {
                    collision.transform.position = movePosition.transform.position; // �÷��̾ �̵�
                }
                else if (Input.GetKey(KeyCode.D) && doorDir == 1)
                {
                    collision.transform.position = movePosition.transform.position; // �÷��̾ �̵�
                }
                else if (Input.GetKey(KeyCode.S) && doorDir == 2)
                {
                    collision.transform.position = movePosition.transform.position; // �÷��̾ �̵�
                }
                else if (Input.GetKey(KeyCode.A) && doorDir == 3)
                {
                    collision.transform.position = movePosition.transform.position; // �÷��̾ �̵�
                }

                if(doorDamage != 0)
                {
                    PlayerManager.instance.GetDamage();
                }
            }

            // ���踦 ����ؾ��ϴ� ���϶�
            else if(collision.gameObject.CompareTag("Player") && !doorKey && ItemManager.instance.keyCount > 0)
            {
                    // �ش�������� ����Ű�� �ѹ��� ������ Ű ���.
                if (Input.GetKey(KeyCode.W) && doorDir == 0)
                {
                     UsingKey();
                    ItemManager.instance.keyCount--;
                    roomInfo.GetComponent<Room>().DoorSound(2);
                }
                else if (Input.GetKey(KeyCode.D) && doorDir == 1)
                {
                    UsingKey();
                    ItemManager.instance.keyCount--;
                    roomInfo.GetComponent<Room>().DoorSound(2);
                }
                else if (Input.GetKey(KeyCode.S) && doorDir == 2)
                {
                    UsingKey();
                    ItemManager.instance.keyCount--;
                    roomInfo.GetComponent<Room>().DoorSound(2);
                }
                else if (Input.GetKey(KeyCode.A) && doorDir == 3)
                {
                    UsingKey();
                    ItemManager.instance.keyCount--;
                    roomInfo.GetComponent<Room>().DoorSound(2);
                }
            }
        }
    }
}
