using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isClear = false; // �����Ƿ�������
    public bool playerInRoom = false; // ����Ƿ��ڷ�����
    public Transform[] roomObjects; // �����е�����
    public List<GameObject> enemis = new List<GameObject>(); // �����еĵ���
    public Transform roomGrid; // ���������
    public Transform cameraPosition; // �������λ��
    public Transform[] doorPosition; // �ŵ�λ��
    public Transform[] movePosition; // �ƶ�λ��
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
}
