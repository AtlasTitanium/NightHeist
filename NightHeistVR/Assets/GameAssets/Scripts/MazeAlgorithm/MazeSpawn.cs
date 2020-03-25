using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {North, East, South, West }
public class MazeSpawn : MonoBehaviour
{
    private GameObject mazeParent;

    public GameObject startRoom, endRoom, serverRoom;
    public GameObject[] trapRooms;
    public GameObject[] baseRooms;

    private GameObject currentRoomObj;
    private RoomBehaviour currentRoom;
    private RoomBehaviour linkingRoom;
    
    private GameObject linkingDoorway;
    private Direction previousDirection;
    private int i = 0;

    public List<GameObject> builtRooms = new List<GameObject>();

    private void Start() {
        mazeParent = new GameObject();
        mazeParent.name = "Maze Parent";

        currentRoomObj = startRoom;

        currentRoomObj = Instantiate(currentRoomObj, Vector3.zero, Quaternion.identity, mazeParent.transform);
        currentRoomObj.name = "StartRoom";
        currentRoom = currentRoomObj.GetComponent<RoomBehaviour>();
        builtRooms.Add(currentRoomObj);
        PlaceRoom();
    }

    private void PlaceRoom() {
        currentRoomObj = Instantiate(baseRooms[Random.Range(0, baseRooms.Length)], new Vector3(999,999,999), Quaternion.identity, mazeParent.transform);
        currentRoomObj.name = "" + i;
        linkingRoom = currentRoomObj.GetComponent<RoomBehaviour>();

        GetRandomSide();
    }

    private void GetRandomSide() {
        int random = Random.Range(0, 4);
        //int random = 1;
        switch (random) {
        case 0:
            if(previousDirection == Direction.South || currentRoom.D_North == null) {
                GetRandomSide();
                return;
            }
            previousDirection = Direction.North;
            Connect(currentRoom.D_North, linkingRoom.D_South);
            break;
        case 1:
            if (previousDirection == Direction.West || currentRoom.D_East == null) {
                GetRandomSide();
                return;
            }
            previousDirection = Direction.East;
            Connect(currentRoom.D_East, linkingRoom.D_West);
            break;
        case 2:
            if (previousDirection == Direction.North || currentRoom.D_South == null) {
                GetRandomSide();
                return;
            }
            previousDirection = Direction.South;
            Connect(currentRoom.D_South, linkingRoom.D_North);
            break;
        case 3:
            if (previousDirection == Direction.East || currentRoom.D_West == null) {
                GetRandomSide();
                return;
            }
            previousDirection = Direction.West;
            Connect(currentRoom.D_West, linkingRoom.D_East);
            break;
        }
    }

    private void GetCorrectSides() {
        Debug.Log("current room: " + currentRoom + " + linking room: " + linkingRoom + " + direction: " + previousDirection);
        switch (previousDirection) {
        case Direction.North:
            if(linkingRoom.D_South == null) {
                PlaceRoom();
                return;
            }
            RemoveDoors(currentRoom.D_North, linkingRoom.D_South);
            break;
        case Direction.East:
            if (linkingRoom.D_West == null) {
                PlaceRoom();
                return;
            }
            RemoveDoors(currentRoom.D_East, linkingRoom.D_West);
            break;
        case Direction.South:
            if (linkingRoom.D_North == null) {
                PlaceRoom();
                return;
            }
            RemoveDoors(currentRoom.D_South, linkingRoom.D_North);
            break;
        case Direction.West:
            if (linkingRoom.D_East == null) {
                PlaceRoom();
                return;
            }
            RemoveDoors(currentRoom.D_West, linkingRoom.D_East);
            break;
        }
    }

    private void Connect(GameObject currentRoomDoor, GameObject connectedRoomDoor) {
        Vector3 linkingRoomOffset = (currentRoomDoor.transform.localPosition - connectedRoomDoor.transform.localPosition) + currentRoom.transform.position;
        if (Physics.CheckBox(linkingRoomOffset, linkingRoom.boxCollider.size / 2)) {
            Destroy(currentRoomObj);
            currentRoomObj = builtRooms[Random.Range(0, builtRooms.Count-1)];
            currentRoom = currentRoomObj.GetComponent<RoomBehaviour>();
            PlaceRoom();
            return;

            Collider[] objects = Physics.OverlapBox(linkingRoomOffset, linkingRoom.boxCollider.size / 2);
            foreach (Collider obj in objects) {
                if (obj.GetComponent<RoomBehaviour>()) {
                    Debug.Log("remove doors instead of spawn");
                    Destroy(currentRoomObj);
                    linkingRoom = obj.GetComponent<RoomBehaviour>();
                    GetCorrectSides();
                }
            }
        } else {
            currentRoomObj.transform.position = linkingRoomOffset;
            builtRooms.Add(currentRoomObj);
            RemoveDoors(currentRoomDoor, connectedRoomDoor);
        }

    }

    private void RemoveDoors(GameObject currentRoomDoor, GameObject connectedRoomDoor) {
        Destroy(currentRoomDoor);
        currentRoomDoor = null;
        Destroy(connectedRoomDoor);
        connectedRoomDoor = null;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Destroy(mazeParent);
            mazeParent = null;
            Start();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            i++;
            currentRoom = linkingRoom;
            PlaceRoom();
        }
    }
}
