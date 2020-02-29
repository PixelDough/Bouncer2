using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public GameObject roomContents;

    RoomManager roomManager;
    BallController player;
    CompositeCollider2D collider;

    private void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        player = FindObjectOfType<BallController>();
        collider = GetComponent<CompositeCollider2D>();
    }

    private void Update()
    {

        if (collider.bounds.Contains((Vector2)player.transform.position))
        {
            if (roomManager.currentRoom != this)
            {
                roomManager.ChangeCurrentRoom(this);
            }
        }
    }
}
