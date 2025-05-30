using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : GItem
{
    public override void Init(IGameManager gameMgr)
    {
        //base.Init(gameMgr);
        this.gameMgr = (BattleGameManager)gameMgr; ;
        isInit = true;
        globalEvent = this.gameMgr.GetMainGameManager().GetService<IGlobalEventService>();
        InitWeaponButton();

    }

    private void InitWeaponButton()
    {
        GetComponent<Image>().sprite = itemInfo.ScriptableItem.Sprite;
        gameObject.name = itemInfo.ScriptableItem.name;
    }

    public void OnButtonClick()
    {
        //int maxPower = ((ScriptableWeapon)(itemInfo.ScriptableItem)).MaxPower;
        //int minPower = ((ScriptableWeapon)(itemInfo.ScriptableItem)).MinPower;
        globalEvent.RaiseSelectedWeaponInfoUpdatedGlobal(gameMgr, new WeaponInfoEventArgs(itemInfo.ScriptableItem as ScriptableWeapon));
        gameMgr.SetBattleState(BattleStateType.select_card);

    }
}
