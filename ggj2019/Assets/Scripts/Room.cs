using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int RoomObjectsNeeded;
    public float RoomRadius;

    int roomObjectsHooked;

	public void RoomObjectHooked()
    {
        roomObjectsHooked++;

        if (roomObjectsHooked >= RoomObjectsNeeded)
        {
            Hub.Instance.RoomCompleted(this);
        }
    }
}
