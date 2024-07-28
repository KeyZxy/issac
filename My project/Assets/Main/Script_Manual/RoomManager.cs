using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> rooms;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Room room in rooms)
        {
            if (room.CompareTag("StartRoom"))
            {
                room.HasExploed = true;
            }
            else
            {
                room.HasExploed = false;
            }
            if (room.CompareTag("Room") || room.CompareTag("StartRoom") || room.CompareTag("EndRoom"))
            {
                room.OpenTheDoor();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Room room in rooms)
        {
            room.PlayerInside();
            if (room.EnemyNum == 0 && (room.CompareTag("Room") || room.CompareTag("EndRoom")))
            {
                room.HasExploed = true;
                room.ShouldOpen();
            }
            if (room.CompareTag("PropsRoom") && GameObject.Find("Player").GetComponent<PlayerControllor>().sliveKey == true)
            {
                room.OpenTheDoor();
            }
            if (room.CompareTag("EndRoom") && GameObject.Find("Player").GetComponent<PlayerControllor>().goldKey == true)
            {
                room.OpenTheEndDoor();
            }
        }
    }
}
