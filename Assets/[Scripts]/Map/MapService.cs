using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using static Unity.VisualScripting.StickyNote;
using Cinemachine;

public enum RoomType
{
    Blue,
    Red,
    Green,
    Yellow
}

public class MapService : MonoBehaviour, IMapService
{
    #region GameManager
    protected IGameManager gameMgr;
    protected IGlobalEventService globalEvent;
    #endregion

    #region MapObjects
    [SerializeField] private int NumOfLobby = 50;
    [SerializeField] private MapStructure MapPrefab;
   
    private Dictionary<int, MapObject> mapObjectDic;
   
    private Camera _camera;
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CinemachineConfiner2D _cinemachineConfiner2D;
  
    #endregion

    #region Map
    private Dictionary<int, Map> Maps = new Dictionary<int, Map>();
    private int currentLevel;
    private int previousLevel;
    private int currentRoomNumber;
    
    #endregion

    public void Init(IGameManager gameManager)
    {
        
        _camera = Camera.main;
        _virtualCamera = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>();
        _cinemachineConfiner2D = FindAnyObjectByType<CinemachineConfiner2D>();
        gameMgr = gameManager;
        globalEvent = gameMgr.GetService<IGlobalEventService>();
        globalEvent.MapCollisionGlobal += MapUpdateHandler;
        globalEvent.BoxCollisionGlobal += BoxCollisionHandler;
        globalEvent.BattleResultGlobal += GlobalEvent_BattleResultGlobal;

        

        #region Making Levels
        // currentLevel variable should be assigned only in MapSetting!!
        currentLevel = 0;
        previousLevel = 0;
        currentRoomNumber = 0;
 
        for (int i = 0; i <= NumOfLobby; i++)
        {
 
            Map map = new Map(i, MapPrefab);
            Maps.Add(i, map);
        }
        #endregion

        #region Intantiate Maps and Setting
        var parentObject = new GameObject("MapObject");
        mapObjectDic = new Dictionary<int, MapObject>();
        for (int lobbyNum = 0; lobbyNum <= NumOfLobby; lobbyNum++)
        {
            var levelObj = new GameObject("Level" + lobbyNum);
            levelObj.transform.SetParent(parentObject.transform);
            Map lobbyData = Maps[lobbyNum];
            // start room
            if (lobbyNum == 0)
            {
                mapObjectDic[lobbyNum] = GameObject.Instantiate(MapPrefab.StartRoomPrefab, Vector3.zero, Quaternion.identity).GetComponent<MapObject>();
                mapObjectDic[lobbyNum].SetCurrentMapData(lobbyData);
                
                
            }
            else
            {
                var lobbyStructurePrefab = MapPrefab.LobbyList[lobbyData.MapLevel];
                var lobbyPrefab = lobbyStructurePrefab.LobbyPrefab;
                MapObject mapObject = GameObject.Instantiate(lobbyPrefab, Vector3.zero, Quaternion.identity).GetComponent<MapObject>();
                mapObjectDic[lobbyNum] = mapObject;
                mapObjectDic[lobbyNum].SetCurrentMapData(lobbyData);
                for (int roomNum = 0;  roomNum < lobbyData.RoomDataList.Count; roomNum++)
                {
                    Room roomData = lobbyData.RoomDataList[roomNum];
                    var roomPrefab = lobbyStructurePrefab.RoomPrefabList[roomData.RoomObjectIndex];
                    mapObject.RoomDic[roomNum] = GameObject.Instantiate(roomPrefab, Vector3.zero, Quaternion.identity).GetComponent<RoomObject>();
                    mapObject.RoomDic[roomNum].SetRoomData(roomData);
                    mapObject.RoomDic[roomNum].Init(gameMgr);
                    mapObject.RoomDic[roomNum].gameObject.SetActive(false);
                    mapObject.RoomDic[roomNum].gameObject.name= "Room" + roomNum;
                    mapObject.RoomDic[roomNum].transform.SetParent(levelObj.transform);
                }
                

            }
           
            mapObjectDic[lobbyNum].Init(gameMgr);
            mapObjectDic[lobbyNum].SetActive(false);
            mapObjectDic[lobbyNum].transform.SetParent(levelObj.transform);
        }

        #endregion

        MapSetting(currentLevel);
        //SaveToJson();
        //_objectPool = mapObject._objectPool;
        //ChangeCameraPosition(_fLobbySpawnPoint); 

    }

   

    public void OnDisable()
    {
        globalEvent.MapCollisionGlobal -= MapUpdateHandler;
        globalEvent.BoxCollisionGlobal -= BoxCollisionHandler;
        globalEvent.BattleResultGlobal -= GlobalEvent_BattleResultGlobal;

    }

    public void Update()
    {
        _virtualCamera.transform.position = _camera.transform.position;
    }

    public void BoxCollisionHandler(IGameManager sender, BoxCollisionEventArgs boxCollisionEvent)
    {
        Debug.Log(boxCollisionEvent.box.name);
        CollectableBox box = boxCollisionEvent.box;
        int boxNum = boxCollisionEvent.box.BoxNum;
        Maps[currentLevel].RoomDataList[currentRoomNumber].OpenBox(boxNum);
        RoomObject roomObject = mapObjectDic[currentLevel].RoomDic[currentRoomNumber];
        roomObject.OpenBoxByData(boxNum);
     
    }

    public void MapUpdateHandler(IGameManager sender, MapEventArgs e)
    {
        MapArgType mapType = e.mapArgType;
        var mapNum = e.MapNum;
        SavePlayerPosition(e);
        
        previousLevel = currentLevel;
        switch (mapType)
        {
            case MapArgType.UPSTAIR:
                
                //MapSetting(currentLevel + 1);
                MapSettingWithFadeInOut(currentLevel + 1);

                             
                break;
            case MapArgType.DOWNSTAIR:
                

                //MapSetting(currentLevel - 1);
                MapSettingWithFadeInOut(currentLevel - 1);
                
                break;
            case MapArgType.ELEVATOR:
                currentLevel = mapNum;
                
                break;
            case MapArgType.ROOM1:
            case MapArgType.ROOM2:
            case MapArgType.ROOM3:
            case MapArgType.ROOM4:
            case MapArgType.ROOM5:
            case MapArgType.ROOM6:
            case MapArgType.ROOM7:
            case MapArgType.ROOM8:
                int roomIndex = mapType - MapArgType.ROOM1;
                if (Maps[currentLevel].isCleared && !Maps[currentLevel].RoomDataList[roomIndex].isOpened)
                    return;

                //RoomSetting(roomIndex);
                RoomSettingWithFadeInOut(roomIndex);


                break;

            case MapArgType.DESTROYENEMY:
                
                break;
            case MapArgType.DESTROYTRESURE:
                
                break;
            case MapArgType.ROOMEXIT:

                //MapSetting(currentLevel);
                MapSettingWithFadeInOut(currentLevel);
                break;

            case MapArgType.ENEMY:


                break;
            case MapArgType.ITEM:


                break;
        }

    }
    private void GlobalEvent_BattleResultGlobal(IGameManager sender, BattleResultEventArgs args)
    {
        Map currentMapData = Maps[currentLevel];
        Room currentRoomData = currentMapData.RoomDataList[currentRoomNumber];
        RoomObject roomObject = mapObjectDic[currentLevel].RoomDic[currentRoomNumber];

        currentRoomData.SetBattleResultData(args.result);
        roomObject.DestoryEnemyByData();
        switch (args.result)
        {
            case BattleResult.Win:

                break;
            case BattleResult.Lose:
                break;
            case BattleResult.Flee:
                break;
        }
        //RoomSetting(currentRoomNumber);
        RoomSettingWithFadeInOut(currentRoomNumber);
    }

    private void SavePlayerPosition(MapEventArgs e)
    {
       


        switch (e.mapArgType)
        {
            case MapArgType.UPSTAIR:
            case MapArgType.DOWNSTAIR:
            case MapArgType.ELEVATOR:
            case MapArgType.ROOM1:
            case MapArgType.ROOM2:
            case MapArgType.ROOM3:
            case MapArgType.ROOM4:
            case MapArgType.ROOM5:
            case MapArgType.ROOM6:
            case MapArgType.ROOM7:
            case MapArgType.ROOM8:
                {
                    PlayerCharacter playerCharacter = gameMgr.GetService<IPlayerControllerService>().PlayerCharacter;

                    // align only for X or Y direction
                    Vector2 AlignedDirection = AutoAlignToLargerAxis(e.CollisionDirection);
                    Vector3 directionOffset = AlignedDirection * 0.1f;
                    Vector2 playerSavePosition = Vector2.zero;

                    do
                    {
                        playerSavePosition = playerCharacter.transform.position - directionOffset;
                        playerCharacter.transform.position = playerSavePosition;

                        // check new position whether it is safe or not for back to previous map
                    } while (playerCharacter.IsOverlappingSomethingTilemapCollider());

                    Maps[currentLevel].playerPositionSaved = playerSavePosition;
                }
                break;
        }
    }
    public static Vector2 AutoAlignToLargerAxis(Vector2 input)
    {
        float magnitude = input.magnitude;

        if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
        {
            return new Vector2(Mathf.Sign(input.x) * magnitude, 0);
        }
        else
        {
            return new Vector2(0, Mathf.Sign(input.y) * magnitude);
        }
    }
    public void MapSettingWithFadeInOut(int level)
    {
        gameMgr.GetService<IVFXService>().ScreenFadeInOut(MapSetting, level);
    }
    public void MapSetting(int level)
    {
        DisableCurrentMap();
        Map currentMap = Maps[level];
        currentRoomNumber = -1;
        
        currentLevel = level;

        MapObject mapObject = mapObjectDic[currentLevel];
        mapObject.SetActive(true);
        globalEvent.RaiseMapUpdatedGlobal(gameMgr, new MapUpadateArg(currentLevel, currentRoomNumber));


        MovePlayerToLobby(previousLevel, currentLevel);

        mapObject.SetCurrentMapData(currentMap);
        mapObject.SetDoorLights();
        mapObject.SetBlocks();
        mapObject.SetOpenedSigns();

    }

    private void MovePlayerToLobby(int previousLeve, int targetLevel)
    {
        Vector2 playerPosition = Maps[targetLevel].playerPositionSaved;

        
        if (playerPosition == Map.InitPositionVector || Mathf.Abs(targetLevel - previousLeve) > 1 )
        {
            // use prefab position saved
            playerPosition = mapObjectDic[targetLevel]._lobbySpawnPoint.transform.position;
        }
        else
        {
            playerPosition = Maps[targetLevel].playerPositionSaved;
        }
        globalEvent.RaisePlayerTeleportGlobal(gameMgr, playerPosition);
        _cinemachineConfiner2D.m_BoundingShape2D = mapObjectDic[targetLevel].GetComponent<PolygonCollider2D>();
        ChangeCameraPosition(playerPosition);
    }

    public void MapUpdate()
    {

    }

    public void ChangeCameraPosition(Vector3 taraget)
    {
        _camera.transform.localPosition = new Vector3((float)taraget.x - 0.5f, (float)taraget.y + 1, _camera.transform.localPosition.z);
    }
    public void RoomSettingWithFadeInOut(int roomNum)
    {
        gameMgr.GetService<IVFXService>().ScreenFadeInOut(RoomSetting, roomNum);
    }
    public void RoomSetting(int roomNum)
    {
        
        DisableCurrentMap();
        currentRoomNumber = roomNum;
        Map currentMapData = Maps[currentLevel];
        Room currentRoomData = currentMapData.RoomDataList[currentRoomNumber];
        
        RoomObject roomObject = mapObjectDic[currentLevel].RoomDic[currentRoomNumber];
        globalEvent.RaiseMapUpdatedGlobal(gameMgr, new MapUpadateArg(currentLevel, currentRoomNumber));

        // if this room is not opened
        if (!currentRoomData.isOpened)
        {
            currentMapData.OpenRoom(currentRoomNumber);
            // after opening room, raise buff event
            globalEvent.RaiseBuffGlobal(gameMgr, new BuffEventArgs(currentRoomData.roomType));

            if (currentMapData.numOfOpenedRoom >= currentMapData.MaxOpenedRoomNum)
            {
                currentMapData.MapClearSequence();
            }
        }

        mapObjectDic[currentLevel].SetActiveRoom(currentRoomNumber, true);
 
        MovePlayerToRoom(roomObject);
    }

    private void MovePlayerToRoom(RoomObject roomObject)
    {


        Vector2 playerPosition = roomObject.SpawnPoint.transform.position;

        _cinemachineConfiner2D.m_BoundingShape2D = roomObject.GetComponent<PolygonCollider2D>();
        globalEvent.RaisePlayerTeleportGlobal(gameMgr, playerPosition);
        ChangeCameraPosition(playerPosition);
    }

    public void DisableCurrentMap()
    {
        mapObjectDic[previousLevel].SetActive(false);
        mapObjectDic[previousLevel].SetActiveRoom(currentRoomNumber, false);
    }

    public void SaveToJson()
    {
        string potion = "";
        for (var i = 0; i < 50; i++)
        {
            potion += JsonUtility.ToJson(Maps[i]);
            for (var j = 0; j < 8; j++)
            {
                potion += JsonUtility.ToJson(Maps[i].RoomDataList[j]);
            }
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SaveData.json", potion);
    }

    public void LoadToUnity()
    {
        if(System.IO.File.Exists(Application.persistentDataPath + "/SaveData.json"))
        {
            
        }
    }

}
