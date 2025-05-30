using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipe : GItem
{
    public override void Init(IGameManager gameMgr)
    {
        base.Init(gameMgr);
        InitCraftingRecipe();
    }

    private void InitCraftingRecipe()
    {
        GetComponent<Image>().sprite = itemInfo.ScriptableItem.Sprite;
        gameObject.name = itemInfo.ScriptableItem.name;
    }

    public void OnClickRecipe()
    {
        gameMgr.GetService<ICraftingUIHandler>().OnClickedCraftRecipeIcon((RecipeInfo)itemInfo);
    }
}
