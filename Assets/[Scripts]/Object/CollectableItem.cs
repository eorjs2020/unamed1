using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CollectableItem : GItem
{
    //private BoxCollider2D boxCollider2D;
    //private SpriteRenderer sprite;

    // for testing. should be deleted later
    //private void Awake()
    //{
    //    InitSpriteAndCollider();
    //}
    public override void Init(IGameManager gameMgr)
    {
        base.Init(gameMgr);
        InitSpriteAndCollider();
    }

    protected void InitSpriteAndCollider()
    {
        if (itemInfo.ScriptableItem!= null && itemInfo.ScriptableItem.Sprite != null)
        {
            // add sprite and collider
            GetComponentInChildren<SpriteRenderer>().sprite = itemInfo.ScriptableItem.Sprite;
            //sprite.sprite = scritableItem.Sprite;

            // setting collider size
            GetComponentInChildren<BoxCollider2D>().size = itemInfo.ScriptableItem.Sprite.bounds.size;
            //collider.size = scritableItem.Sprite.bounds.size;
        }
        else
        {
            Debug.LogWarning($"{GetType().Name} Scriptable Object is not set!");
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("collide!");
    //    GetComponentInChildren<Animator>().SetBool("Opened", true);
    //    GetAndShowRandomItem();
    //}

    //private void GetAndShowRandomItem()
    //{
    //    // get random item.
    //    ItemInfo randItem = gameMgr.GetService<IInventoryService>().GetRandomItemInfoByLevel(itemInfo.ScriptableItem.ItemLevel);
    //    // show item.
    //    var itemRiseEffect = ObjectFactory.CreateItemRiseEffect(randItem);

    //    itemRiseEffect.transform.position = this.transform.position;
    //    itemRiseEffect.Init(gameMgr);
    //    // raise event.
    //    globalEvent.RaisePickupItemGlobal(gameMgr, new ItemEventArgs(randItem, 1));
    //}
}
