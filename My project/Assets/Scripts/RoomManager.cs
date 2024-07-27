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
            room.OpenTheDoor();

        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Room room in rooms)
        {
            room.PlayerInside();
        }
    }
}
