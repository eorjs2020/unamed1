using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingService : MonoBehaviour
{
    protected IGameManager gameMgr;
    protected ICraftingUIHandler craftingUIHandler;
    
    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
        craftingUIHandler = gameMgr.GetService<ICraftingUIHandler>();
        
        // Test Purpose
        AddCraftingUIBtn();
    }

    public void AddCraftingUIBtn()
    {
        Debug.Log("AddCraftingUIBtn");
        //craftingUIHandler.AddCraftItemUI();
    }
}
