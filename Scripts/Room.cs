using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2 GridPos { get; set; }
    public RoomType Type { get; set; }
    public bool DoorTop { get; set; }
    public bool DoorBot { get; set; }
    public bool DoorLeft { get; set; }
    public bool DoorRight { get; set; }

    public List<Room> NeighboorRooms { get; set; }

    public Room(Vector2 _gridPos, RoomType _type)
    {
        GridPos = _gridPos;
        Type = _type;
    }
}

public enum RoomType { Start, Normal, Exit, Secret }
