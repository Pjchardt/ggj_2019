using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    public UnityEvent startEvents;

    public int RoomObjectsNeeded;
    public float RoomRadius;

    int roomObjectsHooked;

    public void RoomEntered()
    {
        if (startEvents != null) { startEvents.Invoke(); }
    }

	public void RoomObjectHooked()
    {
        roomObjectsHooked++;
    
        /*if (roomObjectsHooked >= RoomObjectsNeeded)
        {
            Hub.Instance.RoomCompleted(this);
        }*/
    }

    public void LeavingRoom()
    {
        //do end of room stuff
        Hub.Instance.RoomLeft();
    }
}
