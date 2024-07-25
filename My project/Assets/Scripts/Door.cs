using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform movePosition; // 门的移动位置
    public GameObject roomInfo; // 房间信息
    public int doorDir = -1; // 门的方向
    public float doorDamage; // 门的伤害值

    [Header("Unity Setup")]
    public GameObject closeDoor; // 关闭状态的门
    public GameObject openDoor; // 打开状态的门
    public bool doorKey; // 门是否有钥匙 ( false 表示没有钥匙, true 表示有钥匙 )

    // 检查房间是否清理干净
    public void CheckedClear()
    {
        // 如果房间清理干净且门有钥匙
        if (roomInfo.GetComponent<Room>().isClear && doorKey)
        {
            OpenDoor(); // 打开门
        }
        else
        {
            CloseDoor(); // 关闭门
        }
    }

    // 使用钥匙打开门
    public void UsingKey()
    {
        doorKey = true; // 将门钥匙状态设置为 true
    }

    // 打开门
    public void OpenDoor()
    {
        closeDoor.SetActive(false); // 隐藏关闭状态的门
        openDoor.SetActive(true); // 显示打开状态的门
    }

    // 关闭门
    public void CloseDoor()
    {
        closeDoor.SetActive(true); // 显示关闭状态的门
        openDoor.SetActive(false); // 隐藏打开状态的门
    }

    // 当碰撞器持续接触时调用
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 如果房间清理干净
        if (roomInfo.GetComponent<Room>().isClear) 
        {
            // 如果碰撞物是玩家并且有钥匙
            if (collision.gameObject.CompareTag("Player") && doorKey) 
            {
                // 如果按下 W 键并且门方向是 0
                if (Input.GetKey(KeyCode.W) && doorDir == 0)
                {
                    collision.transform.position = movePosition.transform.position; // 穿门而入
                }
                // 如果按下 D 键并且门方向是 1
                else if (Input.GetKey(KeyCode.D) && doorDir == 1)
                {
                    collision.transform.position = movePosition.transform.position; // 穿门而入
                }
                // 如果按下 S 键并且门方向是 2
                else if (Input.GetKey(KeyCode.S) && doorDir == 2)
                {
                    collision.transform.position = movePosition.transform.position; // 穿门而入
                }
                // 如果按下 A 键并且门方向是 3
                else if (Input.GetKey(KeyCode.A) && doorDir == 3)
                {
                    collision.transform.position = movePosition.transform.position; // 穿门而入
                }

                //// 如果门有伤害值
                //if (doorDamage != 0)
                //{
                //    PlayerManager.instance.GetDamage(); // 玩家受到伤害
                //}
            }
            // 如果玩家没有钥匙但是物品管理器里有钥匙
            else if (collision.gameObject.CompareTag("Player") && !doorKey && ItemManager.instance.keyCount > 0)
            {
                // 如果按下 W 键并且门方向是 0
                if (Input.GetKey(KeyCode.W) && doorDir == 0)
                {
                    UsingKey(); // 使用钥匙
                    ItemManager.instance.keyCount--; // 钥匙数量减少
                    //roomInfo.GetComponent<Room>().DoorSound(2); // 播放开门音效
                }
                // 如果按下 D 键并且门方向是 1
                else if (Input.GetKey(KeyCode.D) && doorDir == 1)
                {
                    UsingKey(); // 使用钥匙
                    ItemManager.instance.keyCount--; // 钥匙数量减少
                    //roomInfo.GetComponent<Room>().DoorSound(2); // 播放开门音效
                }
                // 如果按下 S 键并且门方向是 2
                else if (Input.GetKey(KeyCode.S) && doorDir == 2)
                {
                    UsingKey(); // 使用钥匙
                    ItemManager.instance.keyCount--; // 钥匙数量减少
                    //roomInfo.GetComponent<Room>().DoorSound(2); // 播放开门音效
                }
                // 如果按下 A 键并且门方向是 3
                else if (Input.GetKey(KeyCode.A) && doorDir == 3)
                {
                    UsingKey(); // 使用钥匙
                    ItemManager.instance.keyCount--; // 钥匙数量减少
                    //roomInfo.GetComponent<Room>().DoorSound(2); // 播放开门音效
                }
            }
        }
    }
}
