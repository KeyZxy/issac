using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isClear = false; // 房间是否已清理
    public bool playerInRoom = false; // 玩家是否在房间内
    public Transform[] roomObjects; // 房间中的物体
    public List<GameObject> enemis = new List<GameObject>(); // 房间中的敌人
    AudioSource roomAudio; // 房间的音频源

    [Header("Unity Setup")]
    public Transform roomGrid; // 房间的网格
    public Transform cameraPosition; // 摄像机的位置
    public Transform[] doorPosition; // 门的位置
    public Transform[] movePosition; // 移动位置

    private void Start()
    {
        roomAudio = GetComponent<AudioSource>(); // 获取音频源组件
    }

    private void Update()
    {
        if (playerInRoom)
        {
            CameraSetting(); // 设置摄像机位置
        }

        if (!isClear)
        {
            CheckRoom(); // 检查房间是否清理
        }
    }

    void CheckRoom()
    {
        bool flag = true; // 标记是否清理完毕
        for (int i = 0; i < enemis.Count; i++)
        {
            if (enemis[i] != null)
            {
                flag = false; // 如果还有敌人，标记为未清理
                enemis[i].GetComponent<TEnemy>().playerInRoom = playerInRoom; // 设置敌人的玩家在房间状态
            }
        }
        isClear = flag; // 更新清理状态

        // 如果房间清理完毕且玩家在房间内
        if (isClear && playerInRoom)
        {

            // 如果特定被动道具已获得
            if (ItemManager.instance.PassiveItems[6] && ItemManager.instance.PassiveItems[12])
            {
                if (PlayerManager.instance.CanBlockDamage == 0)
                {
                    PlayerManager.instance.CanBlockDamage++; // 增加防御能力
                }
            }

            // 掉落物品
            ItemManager.instance.itemTable.Dropitem(transform.position, Random.Range(0, 1000) % 4);

            // 播放门打开的声音
            //DoorSound(1);
        }
    }

    void CameraSetting()
    {
        float cameraMoveSpeed = 0.3f; // 摄像机移动速度
        GameManager.instance.myCamera.transform.SetParent(cameraPosition); // 设置摄像机的父对象
        GameManager.instance.myCamera.transform.localPosition = Vector3.MoveTowards(GameManager.instance.myCamera.transform.localPosition, new Vector3(0, 0, 0), cameraMoveSpeed); // 移动摄像机位置
    }

    public void SetGrid()
    {
        roomObjects = new Transform[roomGrid.transform.childCount]; // 初始化房间网格中的物体数组
        for (int i = 0; i < roomObjects.Length; i++)
        {
            roomObjects[i] = roomGrid.GetChild(i); // 获取网格中的每个子对象
        }
    }

    //public void DoorSound(int mode)
    //{
    //    AudioClip doorAudio = SoundManager.instance.GetDoorClip(mode); // 获取门的音效
    //    roomAudio.clip = doorAudio; // 设置音效剪辑
    //    roomAudio.Play(); // 播放音效
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRoom = true; // 玩家进入房间

            // 如果房间未清理完毕
            //if (!isClear)
            //{
            //    DoorSound(0); // 播放门关闭的声音
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRoom = false; // 玩家离开房间
        }
    }
}
