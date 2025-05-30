using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class HitChance
{
    [SerializeField]
    public float MinPercentage;
    [SerializeField]
    public float MaxPercentage;

    public HitChance(float min, float max)
    {
        MinPercentage = min;
        MaxPercentage = max;
    }
    public float GetAttackChanceBewtwenRandomly()
    {
        float randomValue = UnityEngine.Random.Range(MinPercentage, MaxPercentage);
        return CustomUtility.RoundToTwoDecimalPlaces(randomValue);
    }
}

public class AttackPower
{
    public float minPower;
    public float maxPower;

    public AttackPower(float minPower = 0, float maxPower = 0)
    {
        this.minPower = minPower;
        this.maxPower = maxPower;
    }

    public float GetPowerBewtwenRandomly()
    {
        float randomValue = UnityEngine.Random.Range(minPower, maxPower);
        return CustomUtility.RoundToTwoDecimalPlaces(randomValue);
    }
}

// WeaponAttackStats
public class WeaponAttackStats
{
    public AttackPower HeadAttackPower { get; private set; }
    public AttackPower BodyAttackPower { get; private set; }
    public AttackPower ArmAttackPower { get; private set; }

    public HitChance HeadHitChance { get; private set; }
    public HitChance BodyHitChance { get; private set; }
    public HitChance ArmHitChance { get; private set; }

    public float HeadAttackPowerMultiplier { get; private set; }
    public float BodyAttackPowerMultiplier { get; private set; }
    public float ArmAttackPowerMultiplier { get; private set; }

    public CardType SelectedCard { get; private set; }
    public float AttackSuccessRate { get; private set; }
    public SegmentOverlaped overlapedSegemt { get; private set; }

    private Dictionary<CardType, AttackPower> attackPowerDictionary;
    private float TotalAttackChanceIncreased;


    public WeaponAttackStats(
        HitChance headHitChance,
        HitChance bodyHitChance,
        HitChance armHitChance,
        float headAttackPowerMultiplier,
        float bodyAttackPowerMultiplier,
        float armAttackPowerMultiplier)
    {
        HeadHitChance = headHitChance;
        BodyHitChance = bodyHitChance;
        ArmHitChance = armHitChance;
        HeadAttackPowerMultiplier = headAttackPowerMultiplier;
        BodyAttackPowerMultiplier = bodyAttackPowerMultiplier;
        ArmAttackPowerMultiplier = armAttackPowerMultiplier;

    }

    // Set power by using scriptableWeapon.
    public void SetWeaponStat(ScriptableWeapon scriptableWeapon)
    {
        HeadAttackPower = new AttackPower(scriptableWeapon.MinPower * HeadAttackPowerMultiplier, scriptableWeapon.MaxPower * HeadAttackPowerMultiplier);
        BodyAttackPower = new AttackPower(scriptableWeapon.MinPower * BodyAttackPowerMultiplier, scriptableWeapon.MaxPower * BodyAttackPowerMultiplier);
        ArmAttackPower = new AttackPower(scriptableWeapon.MinPower * ArmAttackPowerMultiplier, scriptableWeapon.MaxPower * ArmAttackPowerMultiplier);

        attackPowerDictionary = new Dictionary<CardType, AttackPower>
        {
            { CardType.HEAD, HeadAttackPower },
            { CardType.BODY, BodyAttackPower },
            { CardType.ARM, ArmAttackPower }
        };

    }

    public void SetSelectedCard(CardType cardType)
    {
        SelectedCard = cardType;
    }

    public void InitTotalAttackChanceincrement()
    {
        TotalAttackChanceIncreased = 0.0f;
    }
    public void IncreseAttackChance(float val)
    {
        TotalAttackChanceIncreased += val;
    }


    public void SetAttackSuccessRate(float attackSuccessRate, SegmentOverlaped overlaped)
    {
        //todo need to delete
        AttackSuccessRate = attackSuccessRate;
        overlapedSegemt = overlaped;
    }

    public float GetCalculateAttackValue()
    {
        float DamageCalculated = 0;
        if (attackPowerDictionary == null)
        {
            Debug.LogError("Attack power is not set. Call SetWeaponStat first.");
        }
        else
        {
            if (attackPowerDictionary.TryGetValue(SelectedCard, out AttackPower selectedAttackPower))
            {
                HitChance AttackChance;
                switch(SelectedCard)
                {
                    case CardType.HEAD:
                        AttackChance = HeadHitChance;
                        break;
                    case CardType.BODY:
                        AttackChance = BodyHitChance;
                        break;
                    case CardType.ARM:
                        AttackChance = ArmHitChance;
                        break;
                    default:
                        AttackChance = BodyHitChance;
                        break;
                }
                float attackChance = AttackChance.GetAttackChanceBewtwenRandomly();
                Debug.Log(TotalAttackChanceIncreased);
                attackChance += TotalAttackChanceIncreased;

                float randomPer = UnityEngine.Random.Range(0.0f, 1.0f);
                //Debug.Log(per);
                // todo changing standard rate to card that player choose
                Debug.Log (attackChance + " / " + randomPer);
                if (attackChance >= randomPer)
                {
                    DamageCalculated = selectedAttackPower.GetPowerBewtwenRandomly();
                }
            }
            else
            {
                Debug.LogError("Invalid card type selected.");
            }
        }
        return DamageCalculated;
    }
}


public class WeaponService : MonoBehaviour, IWeaponService
{
    protected IGameManager battleGameMgr;
    protected IGlobalEventService globalEvent;
    //protected IPlayerControllerService playerControllerService;
    public ScriptableWeapon SelectedWeapon { get; set; }
    public WeaponAttackStats WeaponAttackStats { get; set; }

    [SerializeField, Header("[ Default Attack Chance]")]
    private HitChance HeadAttackChance;
    [SerializeField]
    private HitChance BodyAttackChance;
    [SerializeField]
    private HitChance ArmAttackChance;
    [SerializeField, Header("[ Default Attack Power Multiplier]")]
    private float HeadAttackPowerMultiplier;
    [SerializeField]
    private float BodyAttackPowerMultiplier;
    [SerializeField]
    private float ArmAttackPowerMultiplier;

    [SerializeField, Header("[Value for increasing chance value. 0 ~ 1]")]
    private float DefaultAttackChanceIncrementValue;
    // for creaseing bonus. For example, some item can increase attack chance
    public float AttackChanceIncrementBonus = 0;
    [SerializeField, Header("[Value for decreasing chance value. -1 ~ 0]")]
    private float DefaultAttackChanceDecrementValue;
    // for creaseing bonus. For example, some item can decrease attack chance
    public float AttackChanceDecrementBonus = 0;


    private CardType selectedCard;
    private float barPercentageByPlayer;
    

    public void Init(IGameManager gameManager)
    {
        battleGameMgr = (BattleGameManager)gameManager;
        globalEvent = battleGameMgr.GetMainGameManager().GetService<IGlobalEventService>();

        // making BaiscAttackStats
        WeaponAttackStats = new WeaponAttackStats(HeadAttackChance, BodyAttackChance, ArmAttackChance, HeadAttackPowerMultiplier, BodyAttackPowerMultiplier, ArmAttackPowerMultiplier);

        globalEvent.BattleStateUpdatedGlobal += GlobalEvent_BattleStateUpdatedGlobal;
        globalEvent.SelectedWeaponInfoUpdatedGlobal += GlobalEvent_SelectedWeaponInfoUpdatedGlobal;
        globalEvent.SelectedCardUpdatedGlobal += GlobalEvent_SelectedCardUpdatedGlobal;
        globalEvent.BarPercentageUpdateGlobal += GlobalEvent_BarPercentageUpdateGlobal;
        globalEvent.AttackChanceIncrementUpdateGlobal += GlobalEvent_AttackChanceIncrementUpdateGlobal;
    }

    

    private void OnDisable()
    {
        globalEvent.BattleStateUpdatedGlobal -= GlobalEvent_BattleStateUpdatedGlobal;
        globalEvent.SelectedWeaponInfoUpdatedGlobal -= GlobalEvent_SelectedWeaponInfoUpdatedGlobal;
        globalEvent.SelectedCardUpdatedGlobal -= GlobalEvent_SelectedCardUpdatedGlobal;
        globalEvent.BarPercentageUpdateGlobal -= GlobalEvent_BarPercentageUpdateGlobal;
        globalEvent.AttackChanceIncrementUpdateGlobal -= GlobalEvent_AttackChanceIncrementUpdateGlobal;
    }

    private void GlobalEvent_BattleStateUpdatedGlobal(IGameManager sender, BattleStateEventArgs args)
    {
        switch (args.battleStateType)
        {
            case BattleStateType.init_done:
                
                // getting equiped weapons
                var equipedWeaponList = battleGameMgr.GetMainGameManager().GetService<IInventoryService>().EquippedWeaponInfos;
                // if Player doesn't have any weapon. use Fist.
                if (equipedWeaponList[0] == null)
                    battleGameMgr.GetService<ISelectViewUIHandler>().AddWeaponButton(WeaponInfo.Fist);

                // set every weapon player equiped
                equipedWeaponList.ForEach(weapon =>
                {
                    if (weapon != null)
                    {
                        battleGameMgr.GetService<ISelectViewUIHandler>().AddWeaponButton(weapon.ItemInfo);
                    }
                });
                //battleGameMgr.GetService<ISelectViewUIHandler>().AddWeaponButton(WeaponInfo.WoodenClub);
                Debug.Log("Weapons are set done!");
                break;
        }
    }

    // when weapon is seleceted, call it
    private void GlobalEvent_SelectedWeaponInfoUpdatedGlobal(IGameManager sender, WeaponInfoEventArgs args)
    {
        SelectedWeapon = args.scriptableWeapon;
        WeaponAttackStats.SetWeaponStat(SelectedWeapon);
        globalEvent.RaiseWeaponStatsInfoUpdatedGlobal(battleGameMgr, new WeaponStatsInfoEventArgs(this.WeaponAttackStats));
    }

    // Set selected card
    private void GlobalEvent_SelectedCardUpdatedGlobal(IGameManager sender, SelectedCardUpdateEventArgs args)
    {
        // set car and calculate damage.
        WeaponAttackStats.SetSelectedCard(args.CardType);
        // when card selected, increased attack chance should be initialized
        WeaponAttackStats.InitTotalAttackChanceincrement();
        // todo / after select card, should get bar percentage for calculating attack chance.
        battleGameMgr.SetBattleState(BattleStateType.bar);
    }

    // Bar Action done.
    private void GlobalEvent_BarPercentageUpdateGlobal(IGameManager sender, BarPercentageUpdateEventArgs args)
    {
        WeaponAttackStats.SetAttackSuccessRate(args.BarPercentage, args.Overlaped);
        Debug.Log(Enum.GetName(typeof(SegmentOverlaped),args.Overlaped));

        battleGameMgr.SetBattleState(BattleStateType.playerAttackEffectAnimation);
    }
    private void GlobalEvent_AttackChanceIncrementUpdateGlobal(IGameManager sender, AttackChanceIncrementEventArgs args)
    {
        WeaponAttackStats.IncreseAttackChance(args.AttackChanceIncrementValue + AttackChanceIncrementBonus);
    }

    public float GetDamageCalculated()
    {
        return WeaponAttackStats.GetCalculateAttackValue();
    }

    public float GetDefaultAttackChanceIncrementValue()
    {
        return DefaultAttackChanceIncrementValue;
    }

    public float GetDefaultAttackChanceDecrementValue()
    {
        return DefaultAttackChanceDecrementValue;
    }
}
