using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isClear = false;
    public bool playerInRoom = false;
    public Transform[] roomObjects;
    public List<GameObject> enemis = new List<GameObject>();
    AudioSource roomAudio;

    [Header("Unity Setup")] 
    public Transform roomGrid;
    public Transform cameraPosition;
    public Transform[] doorPosition;
    public Transform[] movePosition;

    private void Start()
    {
        roomAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(playerInRoom)
        {
            CameraSetting();
        }

        if(!isClear)
        {
            CheckRoom();
        }
    }
    void CheckRoom()
    {
        bool flag = true;   
        for(int i = 0; i < enemis.Count; i++)
        {
            if (enemis[i] != null)
            {
                flag = false;
                enemis[i].GetComponent<TEnemy>().playerInRoom = playerInRoom;
            }
        }
        isClear = flag;

        //���� Ŭ��������� �÷��̾ �濡 ������.
        if(isClear && playerInRoom) // ���� Ŭ���� + �濡 �÷��̾� ����
        {
            // ��Ƽ�� ������ ������ ����.
            if(ItemManager.instance.ActiveItem != null) // ������ ��Ƽ�� �������� �����Ҷ�.
            {
                ItemManager.instance.ActiveItem.GetComponent<ActiveInfo>().GetEnergy();
                UIManager.instance.UpdateActiveEnergy();
            }

            // ���� �߰�.
            if (ItemManager.instance.PassiveItems[6] && ItemManager.instance.PassiveItems[12])
            {
                if (PlayerManager.instance.CanBlockDamage == 0)
                {
                PlayerManager.instance.CanBlockDamage++;
                }
            }

            //�� Ŭ���� ����
            ItemManager.instance.itemTable.Dropitem(transform.position, Random.Range(0, 1000) % 4);

            //door open sound
            DoorSound(1);
        }
    }

    void CameraSetting()
    {
        float cameraMoveSpeed = 0.3f;
        GameManager.instance.myCamera.transform.SetParent(cameraPosition);
        GameManager.instance.myCamera.transform.localPosition = Vector3.MoveTowards(GameManager.instance.myCamera.transform.localPosition, new Vector3(0, 0, 0), cameraMoveSpeed);
    }

    public void SetGrid()
    {
        roomObjects = new Transform[roomGrid.transform.childCount]; // grid ������ŭ  �迭 ����.
        for(int i = 0; i < roomObjects.Length; i++)
        {
            roomObjects[i] = roomGrid.GetChild(i);
        }
    }

    public void DoorSound(int mode)
    {

        AudioClip doorAudio =  SoundManager.instance.GetDoorClip(mode); // �����Ŭ�� �޾ƿ���.
        roomAudio.clip = doorAudio; // �����Ŭ�� ����
        roomAudio.Play(); // ���
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerInRoom = true;
            
            // �濡 �÷��̾ ����������
            // Ŭ���� �Ǿ�����������
            // �� ������ ���� 
            if(!isClear)
            {
                DoorSound(0); // close Sound
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRoom = false;
        }
    }
}
