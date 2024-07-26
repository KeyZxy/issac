using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isClear = false; // 房间是否已清理
    public bool playerInRoom = false; // 玩家是否在房间内
    public Transform[] roomObjects; // 房间中的物体
    public List<GameObject> enemis = new List<GameObject>(); // 房间中的敌人
    public Transform roomGrid; // 房间的网格
    public Transform cameraPosition; // 摄像机的位置
    public Transform[] doorPosition; // 门的位置
    public Transform[] movePosition; // 移动位置
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
}
