using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class InvenItemInfo
{
    public int ItemNo { get; private set; }
    public ItemInfo ItemInfo { get; private set; }
    public int count;

    public InvenItemInfo(ItemInfo itemInfo, int count)
    {
        this.ItemNo = itemInfo.ScriptableItem.ItemId;
        this.ItemInfo = itemInfo;
        this.count = count;
    }
}

public class InventoryService :MonoBehaviour, IInventoryService
{
    [SerializeField]
    //private Dictionary<ItemInfo, InvenItemInfo> ItemInventoryDic = new Dictionary<ItemInfo, InvenItemInfo>();
    private Dictionary<int, InvenItemInfo> itemInventoryDic = new Dictionary<int, InvenItemInfo>();
    public Dictionary<int, InvenItemInfo> ItemInventoryDic
    { 
        get { return itemInventoryDic; } 
    }

    protected IGameManager gameMgr;
    protected IGlobalEventService globalEvent;
    protected IInventoryUIHandler inventoryUIHandler;

    [Header("Weapon Slot")]
    [SerializeField] private GameObject _slotsParent;
    [SerializeField] private List<InvenItem> _equippedWeaponInfos = new List<InvenItem>();
    public List<InvenItem> EquippedWeaponInfos
    {
        get { return _equippedWeaponInfos; }
    }
    [SerializeField] private List<Image> _equippedWeaponImages = new List<Image>();

    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
        globalEvent = gameMgr.GetService<IGlobalEventService>();
        inventoryUIHandler = gameMgr.GetService<IInventoryUIHandler>();

        Image[] slotImages = _slotsParent.GetComponentsInChildren<Image>();
        _equippedWeaponImages.Clear();
        _equippedWeaponImages.AddRange(slotImages);

        // Tmp Test Data for Crafting
        AddItemToInventory(ItemInfo.Blueberry, 3);
        AddItemToInventory(ItemInfo.Cristal, 3);
        AddItemToInventory(ItemInfo.PotionBlue, 3);
        AddItemToInventory(ItemInfo.PotionRed, 3);
        AddItemToInventory(ItemInfo.Strawberry, 3);
        
        AddItemToInventory(WeaponInfo.WoodenClub, 1);
        AddItemToInventory(WeaponInfo.YellowSword, 1);

        globalEvent.PickupItemGlobal += PickupItem;
        globalEvent.RemoveItemGlobal += RemoveItem;
    }

    private void OnDisable()
    {
        globalEvent.PickupItemGlobal -= PickupItem;
        globalEvent.RemoveItemGlobal -= RemoveItem;
    }

    private void PickupItem(IGameManager sender, ItemEventArgs args)
    {
        // if item box
       // if (args.ItemInfo.ScriptableItem.InvenItemType == Defines.InvenItemType.Box)
       // {
       //     AddRandomItemByItemLevlel(args.ItemInfo.ScriptableItem.ItemLevel, args.Count);
       // }
       // else
       // {
            AddItemToInventory(args.ItemInfo, args.Count);
       // }
    }

    private void RemoveItem(IGameManager sender, ItemEventArgs args)
    {
        RemoveItemFromInventory(args.ItemInfo, args.Count);
    }

    //private void AddRandomItemByItemLevlel(ItemLevel itemLevel, int count)
    //{
    //    ItemInfo.ItemListOderedByLevel.TryGetValue(itemLevel, out var itemList);

    //    ItemInfo itemInfo = itemList[UnityEngine.Random.Range(0, itemList.Count)];
    //    AddItemToInventory(itemInfo, count);
    //}


    public ItemInfo GetRandomItemInfoByLevel(ItemLevel itemLevel)
    {
        ItemInfo.ItemListOderedByLevel.TryGetValue(itemLevel, out var itemList);

        ItemInfo itemInfo = itemList[UnityEngine.Random.Range(0, itemList.Count)];

        return itemInfo;
    }

    public void AddItemToInventory(ItemInfo itemInfo, int count)
    {
        InvenItemInfo item;

        // if there is already item
        //if(ItemInventoryDic.TryGetValue(itemInfo, out item))
        if(itemInventoryDic.TryGetValue(itemInfo.ScriptableItem.ItemId, out item))
        {
            item.count += count;
        }
        else
        {
            //ItemInventoryDic.Add(itemInfo, item = new InvenItemInfo(itemInfo, count));
            itemInventoryDic.Add(itemInfo.ScriptableItem.ItemId, item = new InvenItemInfo(itemInfo, count));
        }
        
        inventoryUIHandler.AddItem(item);
        Debug.Log("Get Item named " + item.ItemInfo.ScriptableItem.name);
    }

    //public List<InvenItem> GetEquipedWeapons()
    //{
    //    return _equippedWeaponInfos;
    //}


    private void RemoveItemFromInventory(ItemInfo itemInfo, int count)
    {
        InvenItemInfo item;

        // if there is already item
        //if (ItemInventoryDic.TryGetValue(itemInfo, out item))
        if (itemInventoryDic.TryGetValue(itemInfo.ScriptableItem.ItemId, out item))
        {
            item.count -= count;
            if (item.count <= 0) 
            {
                //ItemInventoryDic.Remove(itemInfo);
                itemInventoryDic.Remove(itemInfo.ScriptableItem.ItemId);
            }
        }
        else
        {
            Debug.LogError($"Player doesn't have {itemInfo.ScriptableItem.name}!!!");
        }
        inventoryUIHandler.ReduceItem(item);
    }

    
    #region Weapon Slot
    public void EquipWeapon(InvenItem invenItem)
    {
        bool isWeaponEquipped = false;

        for (int index = 0; index < _equippedWeaponInfos.Count; index++)
        {
            if (_equippedWeaponInfos[index] == null)
            {
                _equippedWeaponInfos[index] = invenItem;
                _equippedWeaponImages[index].sprite = invenItem.ItemInfo.ScriptableItem.Sprite;
                isWeaponEquipped = true;
                break;
            }
        }
        
        // TODO Generate UI Message Event 
        if (!isWeaponEquipped) Debug.Log($"No available slot.");
    }

    public void UnequippedWeapon(int index)
    {
        if (_equippedWeaponInfos[index] != null)
        {
            _equippedWeaponImages[index].sprite = null;
            InvenItem selectedInvenItem = _equippedWeaponInfos[index];
            selectedInvenItem.UpdateUIText("");
            selectedInvenItem.IsEquipped = false;
            _equippedWeaponInfos[index] = null;
        }
    }


    #endregion
}
