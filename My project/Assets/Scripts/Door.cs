using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform movePosition; // �ŵ��ƶ�λ��
    public GameObject roomInfo; // ������Ϣ
    public int doorDir = -1; // �ŵķ���
    public float doorDamage; // �ŵ��˺�ֵ

    [Header("Unity Setup")]
    public GameObject closeDoor; // �ر�״̬����
    public GameObject openDoor; // ��״̬����
    public bool doorKey; // ���Ƿ���Կ�� ( false ��ʾû��Կ��, true ��ʾ��Կ�� )

    // ��鷿���Ƿ�����ɾ�
    public void CheckedClear()
    {
        // �����������ɾ�������Կ��
        if (roomInfo.GetComponent<Room>().isClear && doorKey)
        {
            OpenDoor(); // ����
        }
        else
        {
            CloseDoor(); // �ر���
        }
    }

    // ʹ��Կ�״���
    public void UsingKey()
    {
        doorKey = true; // ����Կ��״̬����Ϊ true
    }

    // ����
    public void OpenDoor()
    {
        closeDoor.SetActive(false); // ���عر�״̬����
        openDoor.SetActive(true); // ��ʾ��״̬����
    }

    // �ر���
    public void CloseDoor()
    {
        closeDoor.SetActive(true); // ��ʾ�ر�״̬����
        openDoor.SetActive(false); // ���ش�״̬����
    }

    // ����ײ�������Ӵ�ʱ����
    private void OnTriggerStay2D(Collider2D collision)
    {
        // �����������ɾ�
        if (roomInfo.GetComponent<Room>().isClear) 
        {
            // �����ײ������Ҳ�����Կ��
            if (collision.gameObject.CompareTag("Player") && doorKey) 
            {
                // ������� W �������ŷ����� 0
                if (Input.GetKey(KeyCode.W) && doorDir == 0)
                {
                    collision.transform.position = movePosition.transform.position; // ���Ŷ���
                }
                // ������� D �������ŷ����� 1
                else if (Input.GetKey(KeyCode.D) && doorDir == 1)
                {
                    collision.transform.position = movePosition.transform.position; // ���Ŷ���
                }
                // ������� S �������ŷ����� 2
                else if (Input.GetKey(KeyCode.S) && doorDir == 2)
                {
                    collision.transform.position = movePosition.transform.position; // ���Ŷ���
                }
                // ������� A �������ŷ����� 3
                else if (Input.GetKey(KeyCode.A) && doorDir == 3)
                {
                    collision.transform.position = movePosition.transform.position; // ���Ŷ���
                }

                //// ��������˺�ֵ
                //if (doorDamage != 0)
                //{
                //    PlayerManager.instance.GetDamage(); // ����ܵ��˺�
                //}
            }
            // ������û��Կ�׵�����Ʒ����������Կ��
            else if (collision.gameObject.CompareTag("Player") && !doorKey && ItemManager.instance.keyCount > 0)
            {
                // ������� W �������ŷ����� 0
                if (Input.GetKey(KeyCode.W) && doorDir == 0)
                {
                    UsingKey(); // ʹ��Կ��
                    ItemManager.instance.keyCount--; // Կ����������
                    //roomInfo.GetComponent<Room>().DoorSound(2); // ���ſ�����Ч
                }
                // ������� D �������ŷ����� 1
                else if (Input.GetKey(KeyCode.D) && doorDir == 1)
                {
                    UsingKey(); // ʹ��Կ��
                    ItemManager.instance.keyCount--; // Կ����������
                    //roomInfo.GetComponent<Room>().DoorSound(2); // ���ſ�����Ч
                }
                // ������� S �������ŷ����� 2
                else if (Input.GetKey(KeyCode.S) && doorDir == 2)
                {
                    UsingKey(); // ʹ��Կ��
                    ItemManager.instance.keyCount--; // Կ����������
                    //roomInfo.GetComponent<Room>().DoorSound(2); // ���ſ�����Ч
                }
                // ������� A �������ŷ����� 3
                else if (Input.GetKey(KeyCode.A) && doorDir == 3)
                {
                    UsingKey(); // ʹ��Կ��
                    ItemManager.instance.keyCount--; // Կ����������
                    //roomInfo.GetComponent<Room>().DoorSound(2); // ���ſ�����Ч
                }
            }
        }
    }
}
