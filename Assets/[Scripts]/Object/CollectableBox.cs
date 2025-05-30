using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBox : CollectableItem
{
    public bool IsOpen;
    public int BoxNum;
    public override void Init(IGameManager gameMgr)
    {
        base.Init(gameMgr);
        InitSpriteAndCollider();

        GetComponentInChildren<Animator>().keepAnimatorStateOnDisable = true;
    }

    //private void OnEnable()
    //{
    //    GetComponentInChildren<Animator>().SetBool("Opened", IsOpen);
    //}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsOpen)
            return;
        Debug.Log("collide!");


        //GetComponentInChildren<Animator>().SetBool("Opened", true);
        // for updating map data, send this box ref.
        globalEvent.RaiseBoxCollisionGlobal(gameMgr, new BoxCollisionEventArgs(this));
        
        //GetAndShowRandomItem();
        //IsOpen = true;
    }

    public void GetAndShowRandomItem()
    {
        // get random item.
        ItemInfo randItem = gameMgr.GetService<IInventoryService>().GetRandomItemInfoByLevel(itemInfo.ScriptableItem.ItemLevel);
        // show item.
        var itemRiseEffect = ObjectFactory.CreateItemRiseEffect(randItem);

        itemRiseEffect.transform.position = this.transform.position;
        itemRiseEffect.Init(gameMgr);
        // raise event.
        globalEvent.RaisePickupItemGlobal(gameMgr, new ItemEventArgs(randItem, 1));
    }

    internal void Open()
    {
        IsOpen = true;
        GetComponentInChildren<Animator>().SetBool("Opened", true);
        GetAndShowRandomItem();
    }
}
