using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRiseEffect : GItem
{
    public override void Init(IGameManager gameMgr)
    {
        base.Init(gameMgr);
        InitSpriteAndCollider();
    }

    private void InitSpriteAndCollider()
    {
        if (itemInfo.ScriptableItem != null && itemInfo.ScriptableItem.Sprite != null)
        {
            // add sprite and collider
            GetComponentInChildren<SpriteRenderer>().sprite = itemInfo.ScriptableItem.Sprite;
        }
        else
        {
            Debug.LogWarning($"{GetType().Name} Scriptable Object is not set!");
        }
    }
}
