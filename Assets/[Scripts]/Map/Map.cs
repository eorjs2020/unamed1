using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor.U2D.Aseprite;


public enum BoxStatus
{
    OPENED,
    CLOSED,
    
}
public class Box
{
    public bool DoesExist;
    public BoxStatus status;
    public ItemLevel level;

    public Box(bool doesExist, BoxStatus status, ItemLevel level)
    {
        DoesExist = doesExist;
        this.status = status;
        this.level = level;
    }
}


public class Map
{
    //public MapStructure mapStructure;
    public List<Room> RoomDataList = new List<Room>();
    public List<int> randomOrder;

    //public MapObject MapObject;
    public int MaxOpenedRoomNum = 4;
    public int MapLevel;
    public int mapNum;
    public bool isBoss;
    public bool isCleared = false;
    public bool isStartRoom = false;
    public int numOfOpenedRoom = 0;
    public int numOfRevealedRoom = 0;
    public static readonly Vector2 InitPositionVector = new Vector2(-9999, -9999);
    public Vector2 playerPositionSaved;
    
    public Map(int MapNum, MapStructure map)
    {
        //this.mapStructure = map;
        mapNum = MapNum;

        
        

        if (MapNum == 0)
        {
            //MapObject = GameObject.Instantiate(map.StartRoom, Vector3.zero, Quaternion.identity).GetComponent<MapObject>();
            //MapObject.Init(gameMgr);
            isStartRoom = true;
            MapLevel = 0;
            return;
        }
        else
        {
            // our lobby will be 50, and start lobby index is 0.
            // so our lobby will start from 1 to 50
            // MapLevel is from 0 ~ 4
            MapLevel = (mapNum - 1) / 10;
        }

        //// instantiate lobby objects
        //MapObject = GameObject.Instantiate(map.LobbyList[MapIndex].Lobby, Vector3.zero, Quaternion.identity).GetComponent<MapObject>();


        //RoomList = new List<Room>();


        RoomType[] roomTypes = { RoomType.Green, RoomType.Blue, RoomType.Red, RoomType.Yellow };
        foreach (var roomType in roomTypes)
        {
            for (int i = 0; i < 2; i++)
            {
                Room room = new Room(roomType, this);
                room.SetRoomObjectType(map.LobbyList[MapLevel]);
                RoomDataList.Add(room);
            }
        }
        RoomDataList = CustomUtility.ShuffleList(RoomDataList);

        randomOrder = CustomUtility.ShuffleList(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 });

        // we have to reveal two door at first
        SetRevealRoomRandomly();
        SetRevealRoomRandomly();

        playerPositionSaved = InitPositionVector;
        
    }

    public void SetRevealRoomRandomly()
    {
        // Filter the list to find rooms that are not yet revealed
        var unrevealedRooms = RoomDataList.Where(room => !room.isRevealed).ToList();

        // Check if there are any unrevealed rooms
        if (unrevealedRooms.Count == 0)
        {
            Debug.LogWarning("All rooms are already revealed!");
            return;
        }

        // Select a random room from the unrevealed list
        int randomIndex = UnityEngine.Random.Range(0, unrevealedRooms.Count);
        unrevealedRooms[randomIndex].isRevealed = true;

        numOfRevealedRoom++;

        // Debug log to confirm which room was revealed
        Debug.Log($"Room revealed: {unrevealedRooms[randomIndex]}");
    }

    public void OpenRoom(int roomNum)
    {
        numOfOpenedRoom++;
        Room room = RoomDataList[roomNum];
        room.isOpened = true;
        room.isRevealed = true;

    }

    public void MapClearSequence()
    {
        isCleared = true;
        BlockTheRoomsExceptOpened();
    }

    private void BlockTheRoomsExceptOpened()
    {
        var unOpenedRooms = RoomDataList.Where(room => !room.isOpened).ToList();
        if (unOpenedRooms.Count == 0)
        {
            Debug.LogWarning("There is no UnOpendRoom!?!?");
            return;
        }
        unOpenedRooms.ForEach(room => { room.isBlocked = true; });
    }

    public void InitPlayerPosition()
    {
        playerPositionSaved = InitPositionVector;
    }

}

public class Room
{
    public Map Mapdata;
    public bool isEnemy;
    public int Enemylevel;
    // This is for saving, if we can change those to List, we have to change
    public Box box1;
    public Box box2;
    public Box box3;
    public Box box4;
    public RoomType roomType;
    public bool isCleared;
    public bool isRevealed;
    public bool isBlocked;
    public bool isOpened;
    public int RoomNum;
    public int RoomObjectIndex;
    public Room(RoomType roomType, Map mapData)
    {
        Mapdata = mapData;
        isEnemy = (UnityEngine.Random.value > 0.0f);
        box1 = new Box( (UnityEngine.Random.value > 0.5f),BoxStatus.CLOSED, (ItemLevel)mapData.MapLevel);
        box2 = new Box((UnityEngine.Random.value > 0.5f), BoxStatus.CLOSED, (ItemLevel)mapData.MapLevel);
        box3 = new Box((UnityEngine.Random.value > 0.5f), BoxStatus.CLOSED, (ItemLevel)mapData.MapLevel);
        box4 = new Box((UnityEngine.Random.value > 0.5f), BoxStatus.CLOSED, (ItemLevel)mapData.MapLevel);
       
        isCleared = false;
        isRevealed = false;
        isBlocked = false;
        isOpened = false;
        this.roomType = roomType;
    }

    public void SetRoomObjectType(LobbyStructure lobbyStructure)
    {
        if (lobbyStructure.RoomPrefabList.Count <= 0)
            Debug.LogError("There is no room prefabs");
        //todo same room cannot be set, after making more room prefab, modify
        RoomObjectIndex = UnityEngine.Random.Range(0, lobbyStructure.RoomPrefabList.Count);
    }

    public void SetBattleResultData(BattleResult battleResult)
    {
        switch (battleResult)
        {
            case BattleResult.Win:
                isEnemy = false;
                isCleared = true;
                //DestroyRoomObjects();
                //RoomSetting(currentRoomNumber);
                break;
            
            case BattleResult.Flee:
                break;
        }
    }

    public void OpenBox(int boxNum)
    {
        var boxes = new List<Box>
        {
            box1,box2,box3,box4
        };
        if (boxNum > 0 && boxNum <= boxes.Count)
        {
            boxes[boxNum - 1].status = BoxStatus.OPENED;
        }
        
    }
}

