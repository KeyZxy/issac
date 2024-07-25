using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isClear = false; // �����Ƿ�������
    public bool playerInRoom = false; // ����Ƿ��ڷ�����
    public Transform[] roomObjects; // �����е�����
    public List<GameObject> enemis = new List<GameObject>(); // �����еĵ���
    AudioSource roomAudio; // �������ƵԴ

    [Header("Unity Setup")]
    public Transform roomGrid; // ���������
    public Transform cameraPosition; // �������λ��
    public Transform[] doorPosition; // �ŵ�λ��
    public Transform[] movePosition; // �ƶ�λ��

    private void Start()
    {
        roomAudio = GetComponent<AudioSource>(); // ��ȡ��ƵԴ���
    }

    private void Update()
    {
        if (playerInRoom)
        {
            CameraSetting(); // ���������λ��
        }

        if (!isClear)
        {
            CheckRoom(); // ��鷿���Ƿ�����
        }
    }

    void CheckRoom()
    {
        bool flag = true; // ����Ƿ��������
        for (int i = 0; i < enemis.Count; i++)
        {
            if (enemis[i] != null)
            {
                flag = false; // ������е��ˣ����Ϊδ����
                enemis[i].GetComponent<TEnemy>().playerInRoom = playerInRoom; // ���õ��˵�����ڷ���״̬
            }
        }
        isClear = flag; // ��������״̬

        // ��������������������ڷ�����
        if (isClear && playerInRoom)
        {

            // ����ض����������ѻ��
            if (ItemManager.instance.PassiveItems[6] && ItemManager.instance.PassiveItems[12])
            {
                if (PlayerManager.instance.CanBlockDamage == 0)
                {
                    PlayerManager.instance.CanBlockDamage++; // ���ӷ�������
                }
            }

            // ������Ʒ
            ItemManager.instance.itemTable.Dropitem(transform.position, Random.Range(0, 1000) % 4);

            // �����Ŵ򿪵�����
            //DoorSound(1);
        }
    }

    void CameraSetting()
    {
        float cameraMoveSpeed = 0.3f; // ������ƶ��ٶ�
        GameManager.instance.myCamera.transform.SetParent(cameraPosition); // ����������ĸ�����
        GameManager.instance.myCamera.transform.localPosition = Vector3.MoveTowards(GameManager.instance.myCamera.transform.localPosition, new Vector3(0, 0, 0), cameraMoveSpeed); // �ƶ������λ��
    }

    public void SetGrid()
    {
        roomObjects = new Transform[roomGrid.transform.childCount]; // ��ʼ�����������е���������
        for (int i = 0; i < roomObjects.Length; i++)
        {
            roomObjects[i] = roomGrid.GetChild(i); // ��ȡ�����е�ÿ���Ӷ���
        }
    }

    //public void DoorSound(int mode)
    //{
    //    AudioClip doorAudio = SoundManager.instance.GetDoorClip(mode); // ��ȡ�ŵ���Ч
    //    roomAudio.clip = doorAudio; // ������Ч����
    //    roomAudio.Play(); // ������Ч
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRoom = true; // ��ҽ��뷿��

            // �������δ�������
            //if (!isClear)
            //{
            //    DoorSound(0); // �����Źرյ�����
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRoom = false; // ����뿪����
        }
    }
}
