using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region singleton
    public static ItemManager instance; // ����ģʽ��ʵ��
    private void Awake()
    {
        if (instance == null)
            instance = this; // ���ʵ��Ϊ�գ�����ǰʵ����ֵ����
        else
            Destroy(this); // �������ʵ�������ٵ�ǰ����
    }
    #endregion

    public int item_Activated_Count; // ��¼��Ҽ�����ߵĴ���

    [Header("Drop Item State")]
    public int coinCount = 0;        // �������
    public int bombCount = 0;        // ը������
    public int keyCount = 0;         // Կ������

    [Header("Passive Item State")]
    public bool[] PassiveItems;      // ��������״̬����
    

    [Header("Unity Setup")]
    public GameObject bombPrefab;    // ը��Ԥ����
    public ItemTable itemTable;      // ���߱�
    public Transform itemStorage;    // ���ߴ洢λ�ã���Ʒ/�������ߣ�
    public GameObject goldTable;     // ��ұ�

    [Header("Prefabs")]
    public GameObject tableEffect;   // ����Ч��Ԥ����

    private void Start()
    {
        PassiveItems = new bool[100]; // ��ʼ��������������

    }
}
