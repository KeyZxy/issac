using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoomControl : MonoBehaviour
{
    [Header("RoomInformation")]
    public static Room room;
    public GameObject DoorUp, DoorDown, DoorLeft, DoorRight;//四个方向的门
    public int DoorNum;//有几个门
    public bool LeftHasRoom, RightHasRoom, UpHasRoom, DownHasRoom;//四个方向有没有门
    public BoxCollider2D Collider;

    public List<Collider2D> Doors = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        room = GetComponent<Room>();
        Collider = room.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
