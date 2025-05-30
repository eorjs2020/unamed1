using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomObject : Object
{
    private Room roomData;

    public List<GameObject> ItemPositions;
    public GameObject EnemyParent;
    public GameObject ItemParent;
    public Transform SpawnPoint;


    public override void Init(IGameManager gameMgr)
    {
        base.Init(gameMgr);

        if (!roomData.isEnemy)
            return;



        Enemy enemy;
        enemy = ObjectFactory.CreateFieldEnemyByMapLevel((EnemyLevel)roomData.Mapdata.MapLevel);
        enemy.Init(gameMgr);
        enemy.gameObject.transform.position = EnemyParent.transform.position;
        enemy.gameObject.SetActive(true);
        enemy.transform.SetParent(EnemyParent.transform);


        ItemParent = new GameObject("ItemParnet");
        ItemParent.transform.SetParent(transform);

        
        var boxDataArray = new List<Box>{ roomData.box1, roomData.box2, roomData.box3, roomData.box4 };

        for (int i = 0; i < boxDataArray.Count; i++)
        {
            if (boxDataArray[i].DoesExist)
            {
                CollectableBox box = ObjectFactory.CreateCollectableBox(boxDataArray[i].level);
                box.Init(gameMgr);
                box.BoxNum = i + 1; 

                box.transform.position = ItemPositions[i].transform.position;
                box.transform.SetParent(ItemParent.transform);

                box.gameObject.SetActive(true);
            }
        }
    }


    public void SetRoomData(Room roomData)
    {
        this.roomData = roomData;
    }

    public void DestoryEnemyByData()
    {
        if (!roomData.isEnemy)
            Destroy(EnemyParent.transform.GetChild(0).gameObject);
    }

    public void OpenBoxByData(int boxnum)
    {
        var boxDatas = new List<Box> { roomData.box1, roomData.box2, roomData.box3, roomData.box4 };
        var collectableBoxes = ItemParent.GetComponentsInChildren<CollectableBox>();
        if (boxDatas[boxnum-1].status == BoxStatus.OPENED)
        {
            collectableBoxes.FirstOrDefault(cBox => cBox.BoxNum == boxnum).Open();
        }
       
    }
}
