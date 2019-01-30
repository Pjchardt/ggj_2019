using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RoomData
{
    public Room Room;
    public GameObject TotemRooom;
}

public class Hub : MonoBehaviour
{
    public static Hub Instance;

    public UnityEvent beginEvents;
    public RoomData [] Rooms;

    int chapter = 0;

    private void Awake()
    {
        Instance = this;  
    }

    private void Start()
    {
        Begin();
    }

    void Begin()
    {
        if (beginEvents != null) { beginEvents.Invoke(); }

        StartNextRoom();
    }

    void StartNextRoom()
    {
        AudioManager.Instance.NewChapter(chapter);
        //Rooms[chapter].Room.gameObject.SetActive(true);
        Rooms[chapter].Room.RoomEntered();
    }

    public void RoomLeft ()
    {
        //new chapter
        chapter++;
        if (chapter < Rooms.Length)
        {
            StartNextRoom();
        }
        else
        {
            EndGame();
        }
    }

    public void RoomComplete(Room r)
    {
        RoomData rD = FindRoomData(r);
        rD.TotemRooom.SetActive(true);
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
        //would need bool to check each room for completion, but what for?
    }

    void EndGame()
    {

    }
}
