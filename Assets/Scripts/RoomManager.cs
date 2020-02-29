using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public Room currentRoom;

    Room[] rooms;

    private void Start()
    {
        rooms = FindObjectsOfType<Room>();
    }


    public void ChangeCurrentRoom(Room targetRoom)
    {
        foreach(Room r in rooms)
        {
            if (r == targetRoom)
            {
                r.roomContents.gameObject.SetActive(true);
                currentRoom = r;
            }
            else
            {
                r.roomContents.gameObject.SetActive(false);
            }
        }
    }
}
