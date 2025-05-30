using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler :MonoBehaviour, IInventoryUIHandler
{
    [SerializeField]
    private RectTransform scrollviewContent;
    [SerializeField]
    private GameObject inventoryWeaponPanel;
    [SerializeField]
    private GameObject inventoryConsumePanel;
    [SerializeField]
    private GameObject inventoryETCPanel;

    [SerializeField]
    private List<InvenItem> invenItemList = new List<InvenItem>();

    protected IGameManager gameMgr;
    protected IGlobalEventService globalEvent;

    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
        globalEvent = gameMgr.GetService<IGlobalEventService>();
        
        
    }

    public void AddItem(InvenItemInfo invenItemInfo)
    {
        //InvenItem found = invenItemList.Find(val => val.ItemInfo == invenItemInfo.ItemInfo);
        InvenItem found = invenItemList.Find(val => val.ItemInfo.ScriptableItem.ItemId == invenItemInfo.ItemNo);
        if (found == null)
        {
            found = ObjectFactory.CreateInvenItem(invenItemInfo.ItemInfo);
        }
        
        found.Init(gameMgr);
        found.SetCount(invenItemInfo.count);

        switch(invenItemInfo.ItemInfo.ScriptableItem.InvenItemType)
        {
            case Defines.InvenItemType.Weapon:
                found.transform.SetParent(inventoryWeaponPanel.transform, false);
                break;
            case Defines.InvenItemType.Consumable:
                found.transform.SetParent(inventoryConsumePanel.transform, false);
                break;
            case Defines.InvenItemType.Material:
                found.transform.SetParent(inventoryETCPanel.transform, false);
                break;
        }

        //found.transform.SetParent(inventoryWeaponPanel.transform, false);
        invenItemList.Add(found);
    }

    public void ReduceItem(InvenItemInfo invenItemInfo)
    {
        //InvenItem found = invenItemList.Find(val => val.ItemInfo == invenItemInfo.ItemInfo);
        InvenItem found = invenItemList.Find(val => val.ItemInfo.ScriptableItem.ItemId == invenItemInfo.ItemNo);
        found.SetCount(invenItemInfo.count);
        if (invenItemInfo.count <= 0) 
        {
            invenItemList.Remove(found);
            Destroy(found.gameObject);
        }
        
    }

    public void UpdateInventoryUI(Dictionary<ItemInfo, InvenItemInfo> inven)
    {

    }

    public List<InvenItem> GetInvenItems()
    {
        return invenItemList;
    }
}
