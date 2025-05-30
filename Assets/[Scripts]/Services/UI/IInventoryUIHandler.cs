using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryUIHandler : IGameService
{
    public void AddItem(InvenItemInfo item);
    public void ReduceItem(InvenItemInfo item);
    public List<InvenItem> GetInvenItems();
}
