using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LobbyStructure
{
    public GameObject LobbyPrefab;
    public List<GameObject> RoomPrefabList;
}
[System.Serializable]
public struct MapStructure
{
    public GameObject StartRoomPrefab;
    public List<LobbyStructure> LobbyList;
}
