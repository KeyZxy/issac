using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("RoomInformation")]
    public static Room room;
    public GameObject DoorUp, DoorDown, DoorLeft, DoorRight;//四个方向的门
    public int DoorNum;//有几个门
    public bool LeftHasRoom, RightHasRoom, UpHasRoom, DownHasRoom;//四个方向有没有门
    
    [Header("EnemyInformation")]
    public int EnemyNum;//房间内敌人数量
    public List<GameObject> EnemyList = new List<GameObject>();
    public bool isActive = false;

    [Header("ExploredInformation")]
    public bool HasExploed;
    public bool PlayerInRoom = false;

    private Bounds roomBounds; // 房间的边界
    private void Awake()
    {
        room = GetComponent<Room>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // 计算房间的边界
        CalculateRoomBounds();
        DoorUp.SetActive(UpHasRoom);
        DoorDown.SetActive(DownHasRoom);
        DoorRight.SetActive(RightHasRoom);
        DoorLeft.SetActive(LeftHasRoom);
    }
    void CalculateRoomBounds()
    {
        // 计算房间的边界范围
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        roomBounds = bounds;
    }
    public void ShouldClose()//是否应该开门
    {
        //Debug.Log("Judging");
        if (PlayerInRoom == true && (HasExploed == false))
        {
            //Debug.Log("Should Close");
            Invoke("CloseTheDoor", 0.5f);
            isActive = true;
            /*            foreach (GameObject enemy in EnemyList) 
                        {
                            if (enemy.CompareTag("Clotty") )
                            {
                                enemy.GetComponent<ClottyControl>().CanActive();
                            }
                        }*/
        }
    }

    public void PlayerInside() // 检查玩家是否在房间内
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            if (roomBounds.Contains(player.transform.position))
            {
                PlayerInRoom = true;
                CameraControllor.instance.ChangeTarget(transform);
                ShouldClose();
            }
            else
            {
                PlayerInRoom = false;
            }
        }
    }

    public void OpenTheDoor()//开门 Show开门图层 && 停用碰撞体
    {
        if (LeftHasRoom)
        {
            transform.Find("Door_left").Find("Door_L_Open").gameObject.SetActive(true);
            transform.Find("Door_left").gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //Destroy(transform.Find("Door_left").gameObject.GetComponent<Rigidbody2D>());
            //Debug.Log("Opend Left");
        }
        if (UpHasRoom)
        {
            transform.Find("Door_up").Find("Door_U_Open").gameObject.SetActive(true);
            //Destroy(GameObject.Find("Door_up").GetComponent<Rigidbody2D>());
            transform.Find("Door_up").gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //Destroy(GameObject.Find("Door_up").GetComponent<Rigidbody2D>());
            //Debug.Log("Opend Up");
        }
        if (DownHasRoom)
        {
            transform.Find("Door_down").Find("Door_D_Open").gameObject.SetActive(true);
            //Destroy(GameObject.Find("Door_down").GetComponent<Rigidbody2D>());
            transform.Find("Door_down").gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //Destroy(GameObject.Find("Door_down").GetComponent<Rigidbody2D>());
            //Debug.Log("Opend Down");
        }
        if (RightHasRoom)
        {
            transform.Find("Door_right").Find("Door_R_Open").gameObject.SetActive(true);
            transform.Find("Door_right").gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //Destroy(transform.Find("Door_right").gameObject.GetComponent<Rigidbody2D>());
            //Destroy(GameObject.Find("Door_right").GetComponent<Rigidbody2D>());
            //Debug.Log("Opend Right");
        }
    }

    public void CloseTheDoor()//关门 关开门图层 && 启用碰撞体
    {
        if (LeftHasRoom)
        {
            transform.Find("Door_left").Find("Door_L_Open").gameObject.SetActive(false);
            transform.Find("Door_left").gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (UpHasRoom)
        {
            transform.Find("Door_up").Find("Door_U_Open").gameObject.SetActive(false);
            transform.Find("Door_up").gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (DownHasRoom)
        {
            transform.Find("Door_down").Find("Door_D_Open").gameObject.SetActive(false);
            transform.Find("Door_down").gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (RightHasRoom)
        {
            transform.Find("Door_right").Find("Door_R_Open").gameObject.SetActive(false);
            transform.Find("Door_right").gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
/*public void UpdateRoom(float xOffset,float yOffset)//算StepToStart和DoorNum
{
    StepToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));
    if (LeftHasRoom)
    {
        DoorNum++;
    }
    if(RightHasRoom)
    {
        DoorNum++;
    }
    if (UpHasRoom)
    {
        DoorNum++;
    }
    if (DownHasRoom)
    {
        DoorNum++;
    }
}*/

/*    private void OnTriggerEnter2D(Collider2D collision)//玩家碰门检测
    {
        if (collision.CompareTag("Player"))
        {
            CameraControllor.instance.ChangeTarget(transform);
        }
    }*/
/*public void ControlDoor()
{
    if (PlayerInRoom == true && EnemyNum >0)
    {
        CloseTheDoor();
    }
    else
    {
        OpenTheDoor();
    }
}

}
*/