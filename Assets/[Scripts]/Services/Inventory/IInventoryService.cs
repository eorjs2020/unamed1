using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryService : IGameService
{
    Dictionary<int, InvenItemInfo> ItemInventoryDic { get; }
    List<InvenItem> EquippedWeaponInfos { get; }
    public void AddItemToInventory(ItemInfo itemInfo, int count);
    //public List<InvenItem> GetEquipedWeapons();
    public ItemInfo GetRandomItemInfoByLevel(ItemLevel itemLevel);
    public void EquipWeapon(InvenItem invenItem);
    public void UnequippedWeapon(int index);
}
