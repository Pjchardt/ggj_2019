using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public Room Room;
    public GameObject TotemRooom;
}

public class Hub : MonoBehaviour
{
    public static Hub Instance;

    public List<RoomData> Rooms;

    private void Awake()
    {
        Instance = this;

        foreach (RoomData rD in Rooms)
        {
            rD.TotemRooom.SetActive(false);
        }
    }

    public void RoomCompleted (Room r)
    {
        RoomData rD = FindRoomData(r);
        rD.TotemRooom.SetActive(true);
        Rooms.Remove(rD);
        CheckCompleted();
    }

    RoomData FindRoomData(Room r)
    {
        foreach (RoomData rD in Rooms)
        {
            if (rD.Room = r)
            {
                return rD;
            }
        }

        Debug.LogError("No room data for room: " + r.name);
        return null; //this is bad, we should never get here
    }

    void CheckCompleted()
    {
        if (Rooms.Count < 1)
        {
            //do end game
        }
    }
}
