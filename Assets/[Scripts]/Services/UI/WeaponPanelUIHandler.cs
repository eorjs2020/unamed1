using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
방은 4가지 속성 스텟.
전투바 관여 안함.
공격수치는 무조건 무기 공격력 기반.
힘기반 무기는 무기공격력 자체가 높
공격력 = str + 10~20(무기공격력).

힘 →  힘기반 아이탬들 공격력 상승 / (히든효과) 무기 공격력상승 (빨강)
덱스 → 덱스 기반 아이템 공격력 상승 / (히든효과) 확률적으로 치명적 공격가능(내구도 한번만 소비) 1.5배 공격 (초록색)
AGI → 회피력 상승 / (히든효과) 확률적으로 2번 공격가능(내구도 두번소비) (노란색)
LUCK → 모든확률 증가 (크래프팅시 일부 재료 반환 확률 상승) /   (히든효과) 공격 성공 확률 증가 (보라색)
그리고 같은 버프가 4개 이상모일시 추가효과 존재
 */
public class WeaponPanelUIHandler : MonoBehaviour, IWeaponPanelUIHandler
{
    protected IGameManager gameMgr;

    public GameObject ItemDetailPanel;
    public TextMeshProUGUI DetailTitleText;
    public TextMeshProUGUI DetailDescText;
    public GameObject UseButton;
    public GameObject EquipButton;
    public TextMeshProUGUI[] CharacterStatTexts;

    private InvenItem _invenItem;
    
    public void Init(IGameManager gameManager)
    {
        gameMgr = gameManager;
    }

    private void Start()
    {
        UpdateCharacterStatPanel();
    }

    public void UpdateCharacterStatPanel()
    {
        PlayerCharacter playerCharacter = gameMgr.GetService<IPlayerControllerService>().PlayerCharacter;
        CharacterStatTexts[0].text = "Str : " + playerCharacter.CharacterStatStr;
        CharacterStatTexts[1].text = "Dex : " + playerCharacter.CharacterStatDex;
        CharacterStatTexts[2].text = "Agi : " + playerCharacter.CharacterStatAgi;
        CharacterStatTexts[3].text = "Luck : " + playerCharacter.CharacterStatLuck;
    }
    
    // Processes when equipped weapon clicked
    public void OnClickedEquippedWeapon(int index)
    {
        gameMgr.GetService<IInventoryService>().UnequippedWeapon(index);
    }

    public void SetDetailPanelInfo(InvenItem invenItem)
    {
        if (invenItem == null)
        {
            ItemDetailPanel.SetActive(false);
            return;
        }
        
        ItemDetailPanel.SetActive(true);
        
        _invenItem = invenItem;
        ItemInfo itemInfo = _invenItem.ItemInfo;
        
        if (itemInfo.ScriptableItem.InvenItemType == Defines.InvenItemType.None)
        {
            //Debug.Log($"Type = {itemInfo.ScriptableItem.InvenItemType}");
            SetDetailPanelButton(invenItem.ItemInfo,false);
        } 
        else if (itemInfo.ScriptableItem.InvenItemType == Defines.InvenItemType.Misc)
        {
            //Debug.Log($"Type = {itemInfo.ScriptableItem.InvenItemType}");
            SetDetailPanelButton(invenItem.ItemInfo,false);
        } 
        else if (itemInfo.ScriptableItem.InvenItemType == Defines.InvenItemType.Consumable)
        {
            //Debug.Log($"Type = {itemInfo.ScriptableItem.InvenItemType}");
            SetDetailPanelButton(invenItem.ItemInfo,false);
        } 
        else if (itemInfo.ScriptableItem.InvenItemType == Defines.InvenItemType.Weapon)
        {
            SetDetailPanelButton(invenItem.ItemInfo,true);
        } 
        else if (itemInfo.ScriptableItem.InvenItemType == Defines.InvenItemType.Material)
        {
            //Debug.Log($"Type = {itemInfo.ScriptableItem.InvenItemType}");
            SetDetailPanelButton(invenItem.ItemInfo,false);
        }
    }

    private void SetDetailPanelButton(ItemInfo itemInfo, bool isWeapon)
    {
        DetailTitleText.text = itemInfo.ScriptableItem.ItemName;
        DetailDescText.text = itemInfo.ScriptableItem.Description;
        
        UseButton.SetActive(!isWeapon);
        EquipButton.SetActive(isWeapon);
    }

    public void OnClickUseButton()
    {
        Debug.Log($"{GetType()}::OnClickUseButton.");
        
        ItemDetailPanel.SetActive(false);
    }
    
    public void OnClickEquipButton()
    {
        //Debug.Log($"{GetType()}::OnClickEquipButton.");
        
        if (!_invenItem.IsEquipped)
        {
            gameMgr.GetService<IInventoryService>().EquipWeapon(_invenItem);
            _invenItem.IsEquipped = true;
            _invenItem.UpdateUIText("E");
        }
        else
        {
            // TODO Message Service
            Debug.Log($"Already equipped weapon.");
        }
        
        ItemDetailPanel.SetActive(false);
    }
}
