using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GItem : Object
{
    //[SerializeField]
    //protected ScriptableItem scritableItem;
    [SerializeField]
    protected ItemInfo itemInfo;

    //public ScriptableItem ScriptableItem
    //{
    //    set { scritableItem = value; }
    //    get { return scritableItem; }
    //}

    public ItemInfo ItemInfo
    {
        get { return itemInfo; }
        set { itemInfo = value; }
    }



    public override void Init(IGameManager gameMgr)
    {
        base.Init(gameMgr);
    }

    
   

}
