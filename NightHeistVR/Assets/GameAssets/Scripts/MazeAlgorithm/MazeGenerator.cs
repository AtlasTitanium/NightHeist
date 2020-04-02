using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Range(2,50)]
    public int amountOfRooms = 2;

    private MazeSpawn mazeSpawner;

    public MazeCalls.CallNextRoom nextRoomCall;
   
}

public static class MazeCalls {
    public delegate void CallNextRoom();
}
