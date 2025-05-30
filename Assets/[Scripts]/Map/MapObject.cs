using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapObject : Object
{
    public Dictionary<int, RoomObject> RoomDic = new Dictionary<int, RoomObject>();
    public Map MapData;
    public List<GameObject> DoorLights;
    public List<GameObject> Doors;
    public List<GameObject> DoorBlocks;
    public List<GameObject> DoorOpenedSigns;

    // DoorPositions can be used for moving player between room and lobby later, if we need
    public List<GameObject> DoorPositions;

    //public GameObject _roomSpawnPoint;
    public GameObject _lobbySpawnPoint;
    //public GameObject _RoomPoint;
    //public List<GameObject> _Item;
    //public RoomType _RoomType;
    public GameObject _objectPool;
    //public RoomObject _RoomInfo;
    //public GameObject _collider;


    public override void Init(IGameManager gameMgr)
    {
        base.Init(gameMgr);

        if (MapData.isStartRoom)
            return;

        Transform doorParent = transform.Find("Door");
        Transform lightParent = transform.Find("DoorLight");
        Transform blockParent = transform.Find("DoorBlock");
        Transform openSignParent = transform.Find("DoorOpenSign");
        Transform DoorPositionParent = transform.Find("DoorPosition");
        if (doorParent == null)
        {
            Debug.LogError("Door object not found");
            return;
        }

        // Iterate through all child objects under "Door" and add them to the list
        foreach (Transform child in doorParent)
        {
            Doors.Add(child.gameObject);
        }
        foreach (Transform child in lightParent)
        {
            DoorLights.Add(child.gameObject);
        }
        foreach (Transform child in blockParent)
        {
            DoorBlocks.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in openSignParent)
        {
            DoorOpenedSigns.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in DoorPositionParent)
        {
            DoorPositions.Add(child.gameObject);
        }
    }

    public void SetCurrentMapData(Map map)
    {
        MapData = map;

    }

    public void SetDoorLights()
    {
        if (MapData == null)
        {
            Debug.LogError("MapData is null!");
        }
        var roomList = MapData.RoomDataList;
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].isRevealed)
            {
                switch (roomList[i].roomType)
                {
                    case RoomType.Red:
                        DoorLights[i].GetComponent<Tilemap>().color = Color.red;
                        break;
                    case RoomType.Green:
                        DoorLights[i].GetComponent<Tilemap>().color = Color.green;
                        break;
                    case RoomType.Blue:
                        DoorLights[i].GetComponent<Tilemap>().color = Color.blue;
                        break;
                    case RoomType.Yellow:
                        DoorLights[i].GetComponent<Tilemap>().color = Color.yellow;
                        break;
                    default:
                        break;
                }

            }
            else
            {
                DoorLights[i].GetComponent<Tilemap>().color = Color.gray;
            }
        }

    }

    public void SetBlocks()
    {
        if (MapData == null)
        {
            Debug.LogError("MapData is null!");
        }
        var roomList = MapData.RoomDataList;
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].isBlocked)
            {
                DoorBlocks[i].gameObject.SetActive(true);
            }
            else
                DoorBlocks[i].gameObject.SetActive(false);
        }
    }

    public void SetOpenedSigns()
    {
        if (MapData == null)
        {
            Debug.LogError("MapData is null!");
        }
        var roomList = MapData.RoomDataList;
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].isOpened)
            {
                switch (roomList[i].roomType)
                {
                    case RoomType.Red:
                        DoorOpenedSigns[i].GetComponent<Tilemap>().color = Color.red;
                        break;
                    case RoomType.Green:
                        DoorOpenedSigns[i].GetComponent<Tilemap>().color = Color.green;
                        break;
                    case RoomType.Blue:
                        DoorOpenedSigns[i].GetComponent<Tilemap>().color = Color.blue;
                        break;
                    case RoomType.Yellow:
                        DoorOpenedSigns[i].GetComponent<Tilemap>().color = Color.yellow;
                        break;
                    default:
                        break;
                }
                DoorOpenedSigns[i].gameObject.SetActive(true);
            }
            else
                DoorOpenedSigns[i].gameObject.SetActive(false);
        }
    }

    public void SetActive(bool val)
    {
        gameObject.SetActive(val);
    }

    public void SetActiveRoom(int roomNum, bool val)
    {
        if (roomNum < 0)
            return;
        if (RoomDic.Count <= 0)
            return;
        RoomDic[roomNum].gameObject.SetActive(val);
    }
}
