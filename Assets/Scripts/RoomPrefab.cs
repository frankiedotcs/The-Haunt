using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomPrefab : ScriptableObject
{
    /// <summary>
    /// Create a dictionary for all the objects
    /// Frankie Wrote this but didn't have time to implement it
    /// </summary>
    public static Dictionary<int, Object> roomList = new Dictionary<int, Object>(); 

    /// <summary>
    /// loads all the rooms into a list
    /// </summary>
    /// <param name="pPath"></param>
    public static void LoadAll(string pPath) {

        Object[] Rooms = Resources.LoadAll(pPath); 

        foreach (Object obj in Rooms) {
            roomList.Add(obj.name.GetHashCode(), (Object)obj);
        }
    }

    /// <summary>
    /// Returns the object prefab based on the room and has to match the active scene
    /// </summary>
    /// <param name="roomName"></param>
    /// <returns></returns>
    
    public static Object getPrefab(string roomName)
    {
        Object obj;
        Object emptyObject = null;

        if(roomList.TryGetValue(roomName.GetHashCode(), out obj)) {
            return obj;
        }
        else {
            Debug.Log("Room was not found");
            return emptyObject;
        }

    }
   
        

}
