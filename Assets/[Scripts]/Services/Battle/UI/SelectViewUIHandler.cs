using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SelectViewUIHandler : MonoBehaviour, ISelectViewUIHandler
{
    protected IGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;
    //protected IPlayerControllerService playerControllerService;

    [SerializeField]
    private GameObject SelectViewBackPannel;

    [SerializeField]
    private List<Button> CardButtonList;

    [SerializeField]
    private TextMeshProUGUI HeadText;
    [SerializeField]
    private TextMeshProUGUI BodyText;
    [SerializeField]
    private TextMeshProUGUI ArmText;

    [SerializeField]
    private GameObject WeaponContent;


    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.GetMainGameManager().GetService<IGlobalEventService>();

        globalEvent.BattleStateUpdatedGlobal += GlobalEvent_BattleStateUpdatedGlobal;
        globalEvent.SelectedWeaponInfoUpdatedGlobal += GlobalEvent_SelectedWeaponInfoUpdatedGlobal;
        globalEvent.WeaponStatsInfoUpdatedGlobal += GlobalEvent_WeaponStatsInfoUpdatedGlobal;
    }

   

    private void OnDisable()
    {
        globalEvent.BattleStateUpdatedGlobal -= GlobalEvent_BattleStateUpdatedGlobal;
        globalEvent.SelectedWeaponInfoUpdatedGlobal -= GlobalEvent_SelectedWeaponInfoUpdatedGlobal;
        globalEvent.WeaponStatsInfoUpdatedGlobal -= GlobalEvent_WeaponStatsInfoUpdatedGlobal;
    }

    private void GlobalEvent_BattleStateUpdatedGlobal(IGameManager sender, BattleStateEventArgs args)
    {

        switch (args.battleStateType)
        {
            case BattleStateType.init_done:
                break;
            case BattleStateType.ready:
                SelectViewBackPannel.SetActive(false);
                SetCardButtonInteractive(false);
                break;
            case BattleStateType.select_weapon:
                SelectViewBackPannel.SetActive(true);
                SetCardButtonInteractive(false);
                break;
            case BattleStateType.select_card:
                //SelectViewBackPannel.SetActive(true);
                SetCardButtonInteractive(true);
                break;
            case BattleStateType.loading:
                break;
            case BattleStateType.bar:
                SelectViewBackPannel.SetActive(false);
                break;
        }

    }

    private void GlobalEvent_SelectedWeaponInfoUpdatedGlobal(IGameManager sender, WeaponInfoEventArgs args)
    {
        ScriptableWeapon scriptableWeapon = args.scriptableWeapon;
        //WeaponInfo selectedWeapon = args.weaponItemInfo as WeaponInfo;
        HeadText.text = scriptableWeapon.MaxPower.ToString();
        BodyText.text = scriptableWeapon.MaxPower.ToString();
        ArmText.text = scriptableWeapon.MaxPower.ToString();
    }

    private void GlobalEvent_WeaponStatsInfoUpdatedGlobal(IGameManager sender, WeaponStatsInfoEventArgs args)
    {
        HeadText.text = "Damage : " + args.weaponAttackStats.HeadAttackPower.minPower + " - " + args.weaponAttackStats.HeadAttackPower.maxPower + " \n" + "AttackChance : " + args.weaponAttackStats.HeadHitChance.MinPercentage + " - " + args.weaponAttackStats.HeadHitChance.MaxPercentage;
        BodyText.text = "Damage : " + args.weaponAttackStats.BodyAttackPower.minPower + " - " + args.weaponAttackStats.BodyAttackPower.maxPower + " \n" +
            "AttackChance : " + args.weaponAttackStats.BodyHitChance.MinPercentage + " - " + args.weaponAttackStats.BodyHitChance.MaxPercentage;
        ArmText.text = "Damage : " + args.weaponAttackStats.ArmAttackPower.minPower + " - " + args.weaponAttackStats.ArmAttackPower.maxPower + " \n" +
            "AttackChance : " + args.weaponAttackStats.ArmHitChance.MinPercentage + " - " + args.weaponAttackStats.ArmHitChance.MaxPercentage;
    }

    public void AddWeaponButton(ItemInfo weapon)
    {
        WeaponButton weaponButton = ObjectFactory.CreateWeaponBtn(weapon);
        weaponButton.transform.parent = WeaponContent.transform;
        weaponButton.Init(battleGameMgr);
    }

    public void Onscrol(Vector2 val)
    {
        Debug.Log(val);
    }

    private void SetCardButtonInteractive(bool val)
    {
        CardButtonList.ForEach(button => button.interactable = val);
    }

    [VisibleEnum(typeof(CardType))]
    public void OnClickCard(int cardNum)
    {
        CardType cardType = (CardType)cardNum;
        globalEvent.RaiseSelectedCardUpdatedGlobal(battleGameMgr, new SelectedCardUpdateEventArgs(cardType));
        //switch (cardType)
        //{
        //    case CardType.HEAD:
        //        globalEvent.RaiseSelectedCardUpdatedGlobal(battleGameMgr, new SelectedCardUpdateEventArgs(CardType.HEAD));
        //        break;
        //    case CardType.BODY:
        //        break;
        //    case CardType.ARM:
        //        break;
        //}
        //battleGameMgr.SetBattleState(BattleStateType.bar);
        //globalEvent.RaiseBattleStateUpdatedGlobal(battleGameMgr, new BattleStateEventArgs(BattleStateType.bar));
    }


}
