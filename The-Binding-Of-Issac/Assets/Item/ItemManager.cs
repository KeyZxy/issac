using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region singleton
    public static ItemManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    public int item_Activated_Count; //�÷��̾ �����̽��ٸ� ���� Ƚ��, 1ȸ�� �������� ���� �ɷ�ġ ���� (��Ƽ�� ������)

    [Header("Drop Item State")]
    public int coinCount = 0;        // ���� ���� ���� 
    public int bombCount = 0;        // ��ź ���� ����
    public int keyCount = 0;         // ���� ���� ����

    [Header("Passive Item State")]
    public bool[] PassiveItems;      // �нú� ������ ���� ��Ȳ
    public bool[] TrinketItems;      // ��ű� ������ ȹ�� ��Ȳ
    public bool[] ActiveItems;       // ��Ƽ�� ������ ȹ�� ��Ȳ


    [Header("unique Item State")]
    public GameObject ActiveItem;    // ���� �������� ��Ƽ�� ������
    public GameObject TrinketItem;   // ���� �������� ��ű� ������
    
    [Header("Unity Setup")]
    public GameObject bombPrefab;    // ��ġ�ϴ� ��ź ������Ʈ ������
    public ItemTable itemTable;      // ������ ��� ���� ��ũ��Ʈ
    public Transform itemStorage;    // ������ ���� ��� ( ��ű� / ��Ƽ�� )
    public GameObject goldTable;     // ������ ���̺� ������
    
    [Header("Prefabs")]
    public GameObject tableEffect;   // ������ ���� ����Ʈ

    private void Start()
    {
        PassiveItems = new bool[100];
        TrinketItems = new bool[100];
        ActiveItems = new bool[100];
    }
}
