using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    #region singleton
    public static ItemManager instance; // 单例模式的实例
    private void Awake()
    {
        if (instance == null)
            instance = this; // 如果实例为空，将当前实例赋值给它
        else
            Destroy(this); // 如果已有实例，销毁当前对象
    }
    #endregion

    public int item_Activated_Count; // 记录玩家激活道具的次数

    [Header("Drop Item State")]
    public int coinCount = 0;        // 金币数量
    public int bombCount = 0;        // 炸弹数量
    public int keyCount = 0;         // 钥匙数量

    [Header("Passive Item State")]
    public bool[] PassiveItems;      // 被动道具状态数组
    

    [Header("Unity Setup")]
    public GameObject bombPrefab;    // 炸弹预制体
    public ItemTable itemTable;      // 道具表
    public Transform itemStorage;    // 道具存储位置（饰品/主动道具）
    public GameObject goldTable;     // 金币表

    [Header("Prefabs")]
    public GameObject tableEffect;   // 道具效果预制体

    private void Start()
    {
        PassiveItems = new bool[100]; // 初始化被动道具数组

    }
}
