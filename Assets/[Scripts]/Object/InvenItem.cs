using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class InvenItem : GItem
{
    [SerializeField] private Defines.InvenItemType _invenItemType = Defines.InvenItemType.None;
    [SerializeField] private int Count = 0;
    [SerializeField] private int itemId = 0;
    [SerializeField] private bool _isEquipped = false; 
    public bool IsEquipped
    {
        get { return _isEquipped; }
        set
        {
            _isEquipped = value;
        }
    }

    public override void Init(IGameManager gameMgr)
    {
        base.Init(gameMgr);
        InitInvenItem();
    }

    private void InitInvenItem()
    {
        GetComponent<Image>().sprite = itemInfo.ScriptableItem.Sprite;
        gameObject.name = itemInfo.ScriptableItem.name;
        itemId = itemInfo.ScriptableItem.ItemId;
        _invenItemType = itemInfo.ScriptableItem.InvenItemType;
    }

    public void SetCount(int count)
    {
        Count = count;
        UpdateUI();
    }

    public int GetCount()
    {
        return Count;
    }

    public void ReduceInvenItemCount(int count)
    {
        Count -= count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = Count.ToString();
    }

    public void UpdateUIText(string text)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void OnClickItem()
    {
        //Debug.Log($"InvenItem Clicked. ItemId = [{itemId}], Name = [{name}]");
        
        gameMgr.GetService<IWeaponPanelUIHandler>().SetDetailPanelInfo(this);
    }

}
