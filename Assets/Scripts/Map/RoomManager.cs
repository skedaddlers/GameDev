using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private List<Room> rooms;
    public List<Room> Rooms { get => rooms; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddRoom(Room room)
    {
        rooms.Add(room);
    }

    public void RemoveRoom(Room room)
    {
        rooms.Remove(room);
    }
}
